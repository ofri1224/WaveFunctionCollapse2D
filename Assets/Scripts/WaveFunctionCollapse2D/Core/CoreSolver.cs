using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace WaveFunctionCollapse2D
{
    public class CoreSolver 
    {
        PatternManager patternManager;
        OutputGrid outputGrid;
        CoreHelper coreHelper;

        PropagationHelper propagationHelper;

        public CoreSolver(OutputGrid outputGrid,PatternManager patternManager)
        {
            this.outputGrid=outputGrid;
            this.patternManager=patternManager;
            this.coreHelper = new CoreHelper(this.patternManager);
            this.propagationHelper = new PropagationHelper(this.outputGrid,this.coreHelper);
        }

        public void Propagate()
        {
            while (propagationHelper.PairsToPropagate.Count>0)
            {
                var propagatePair = propagationHelper.PairsToPropagate.Dequeue();
                if(propagationHelper.CheckIfPairShouldBeProcessed(propagatePair))
                {
                    ProcessCell(propagatePair);
                }
                if(propagationHelper.CheckForConflicts() || outputGrid.IsGridSolved())
                {
                    return;
                }
            }
            if(propagationHelper.CheckForConflicts() &&
            propagationHelper.PairsToPropagate.Count==0 && 
            propagationHelper.LowEntropySet.Count==0)
            {
                propagationHelper.SetConflictFlag();
            }
        }

        private void ProcessCell(VectorPair propagatePair)
        {
            if(outputGrid.CheckIfCellIsCollapsed(propagatePair.CellToPropagatePosition))
            {
                propagationHelper.EnqueueUncollapsedNeighbors(propagatePair);
            }
            else
            {
                PropagateNeighbor(propagatePair);
            }
        }

        private void PropagateNeighbor(VectorPair propagatePair)
        {
            var possibleValuesAtNeighbor = outputGrid.GetPossibleValueForPosition(propagatePair.CellToPropagatePosition);
            int startCount = possibleValuesAtNeighbor.Count;

            RemoveImpossibleNeighbors(propagatePair,possibleValuesAtNeighbor);

            int newPossiblePatternCount = possibleValuesAtNeighbor.Count;
            propagationHelper.AnalyzePropagationResults(propagatePair,startCount,newPossiblePatternCount);
        }

        private void RemoveImpossibleNeighbors(VectorPair propagatePair, HashSet<int> possibleValuesAtNeighbor)
        {
            HashSet<int> possibleIndices = new HashSet<int>();
            foreach (var patternIndexAtBase in outputGrid.GetPossibleValueForPosition(propagatePair.BaseCellPosition))
            {
                var possibleNeighborsForBase = patternManager.GetPossibleNeighborsForPatternInDir(patternIndexAtBase,propagatePair.DirectionFromBase);
                possibleIndices.UnionWith(possibleNeighborsForBase);
            }
            
            possibleValuesAtNeighbor.IntersectWith(possibleIndices);
        }

        public Vector2Int GetLowestEntropyCell()
        {
            if(propagationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }
            else
            {
                var lowestEntropyElement = propagationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropyElement.Position;
                propagationHelper.LowEntropySet.Remove(lowestEntropyElement);
                return returnVector;
            }
        }
        public void CollapseCell(Vector2Int cellCoordinates)
        {
            var possibleValue = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();
            if(possibleValue.Count==0||possibleValue.Count ==1)
            {
                return;
            }
            else
            {
                int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
                outputGrid.SetPatternAtPosition(cellCoordinates.x,cellCoordinates.y,possibleValue[index]);
            }

            if(!coreHelper.CheckCellSolutionForCollisions(cellCoordinates,outputGrid))
            {
                propagationHelper.AddNewPairsToPropagateQueue(cellCoordinates,cellCoordinates);
            }
            else
            {
                propagationHelper.SetConflictFlag();
            }
        }
        public bool IsSolved()
        {
            return outputGrid.IsGridSolved();
        }

        public bool CheckForConflicts()
        {
            return propagationHelper.CheckForConflicts();
        }
    }
}

