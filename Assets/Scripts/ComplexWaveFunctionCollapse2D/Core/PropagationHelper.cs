using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class PropagationHelper
    {
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        bool cellWithNoSolutionPresent;
        SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
        Queue<VectorPair> pairsToPropagate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet;}
        public Queue<VectorPair> PairsToPropagate { get => pairsToPropagate;}
        public PropagationHelper(OutputGrid outputGrid, CoreHelper coreHelper)
        {
            this.outputGrid=outputGrid;
            this.coreHelper=coreHelper;
        }
        public bool CheckIfPairShouldBeProcessed(VectorPair propagatePair)
        {
            return outputGrid.CheckCellExists(propagatePair.CellToPropagatePosition) && !propagatePair.CellPreviouslyChecked();
        }

        public void AnalyzePropagationResults(VectorPair propagatePair, int startCount,int newPossiblePatternCount)
        {
            if(newPossiblePatternCount>1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagatePair.CellToPropagatePosition,propagatePair.BaseCellPosition);
                AddToLowEntropySet(propagatePair.CellToPropagatePosition);
            }
            if(newPossiblePatternCount==0)
            {
                cellWithNoSolutionPresent = true;
            }
            if(newPossiblePatternCount==1)
            {
                cellWithNoSolutionPresent=coreHelper.CheckCellSolutionForCollisions(propagatePair.CellToPropagatePosition,outputGrid);
            }
        }

        internal void EnqueueUncollapsedNeighbors(VectorPair propagatePair)
        {
            var uncollapsedNeighbors = coreHelper.AreNeighborsCollapsed(propagatePair,outputGrid);
            foreach (var Neighbor in uncollapsedNeighbors)
            {
                pairsToPropagate.Enqueue(Neighbor);
            }
        }

        private void AddToLowEntropySet(Vector2Int cellToPropagatePosition)
        {
            var elementOfLowEntropySet = lowEntropySet.Where(x=>x.Position==cellToPropagatePosition).FirstOrDefault();
            if(elementOfLowEntropySet==null&& !outputGrid.CheckIfCellIsCollapsed(cellToPropagatePosition))
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropagatePosition,outputGrid);
                lowEntropySet.Add(new LowEntropyCell(cellToPropagatePosition,entropy));
            }
            else
            {
                lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.Entropy= coreHelper.CalculateEntropy(cellToPropagatePosition,outputGrid);
                lowEntropySet.Add(elementOfLowEntropySet);
            }
        }

        public void AddNewPairsToPropagateQueue(Vector2Int cellToPropagatePosition, Vector2Int baseCellPosition)
        {
            var list = coreHelper.CreateFourDirectionsNeighbors(cellToPropagatePosition,baseCellPosition);
            foreach (var item in list)
            {
                pairsToPropagate.Enqueue(item);
            }
        }

        public bool CheckForConflicts()
        {
            return cellWithNoSolutionPresent;
        }
        public void SetConflictFlag()
        {
            cellWithNoSolutionPresent = true;
        }
    }
}
