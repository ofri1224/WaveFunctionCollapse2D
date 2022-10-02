using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class CoreHelper
    {
        float totalFreq = 0;
        float totalFreqLog = 0;
        PatternManager patternManager;
        public CoreHelper(PatternManager patternManager)
        {
            this.patternManager = patternManager;            
        }

        public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
        {
            List<float> ValueFreqFractions = GetWeightListFromIndices(possibleValues);
            float rndVal = UnityEngine.Random.Range(0,ValueFreqFractions.Sum());
            float sum = 0;
            int index = 0;
            foreach (float frequency in ValueFreqFractions)
            {
                sum+=frequency;
                if(rndVal <= sum)
                {
                    return index;
                }
                index++;
            }
            return index-1;
        }

        public List<float> GetWeightListFromIndices(List<int> possibleValues)
        {
            List<float> ValueFrequencies = possibleValues.Aggregate(new List<float>(),(acc,val)=> {
                acc.Add(patternManager.GetPatternFrequency(val));
                return acc;
            },
            acc => acc).ToList();
            return ValueFrequencies;
        }

        public List<VectorPair> CreateFourDirectionsNeighbors(Vector2Int cellCoords,Vector2Int PrevCellCoords)
        {
            List<VectorPair> list = new List<VectorPair>();
            {
                new VectorPair(cellCoords,cellCoords+new Vector2Int(1,0),Direction.Right,PrevCellCoords);
                new VectorPair(cellCoords,cellCoords+new Vector2Int(-1,0),Direction.Left,PrevCellCoords);
                new VectorPair(cellCoords,cellCoords+new Vector2Int(0,1),Direction.Up,PrevCellCoords);
                new VectorPair(cellCoords,cellCoords+new Vector2Int(0,-1),Direction.Down,PrevCellCoords);
            }
            return list;
        }

        public List<VectorPair> CreateFourDirectionsNeighbors(Vector2Int cellCoords)
        {
            return CreateFourDirectionsNeighbors(cellCoords,cellCoords);
        }

        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid)
        {
            float sum = 0;
            foreach (var PossibleIndex in outputGrid.GetPossibleValueForPosition(position))
            {
                totalFreq+=this.patternManager.GetPatternFrequency(PossibleIndex);
                sum+=patternManager.GetPatternFrequencyLog2(PossibleIndex);
            }
            totalFreqLog = Mathf.Log(totalFreq,2);
            return totalFreqLog-(sum/totalFreq);
        }

        public List<VectorPair> AreNeighborsCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return CreateFourDirectionsNeighbors(pairToCheck.CellToPropagatePosition,pairToCheck.BaseCellPosition)
                .Where(x => 
                    outputGrid.IsPositionValid(x.CellToPropagatePosition) &&
                    !outputGrid.CheckIfCellIsCollapsed(x.CellToPropagatePosition)
                    ).ToList();
        }

        public bool CheckCellSolutionForCollisions(Vector2Int cellCoordinates,OutputGrid outputGrid)
        {
            foreach (VectorPair neighbor in CreateFourDirectionsNeighbors(cellCoordinates))
            {
                if(!outputGrid.IsPositionValid(neighbor.CellToPropagatePosition))
                {
                    continue;
                }
                HashSet<int> possibleIndices = new HashSet<int>();
                foreach (var patternIndexAtNeighbor in outputGrid.GetPossibleValueForPosition(neighbor.CellToPropagatePosition))
                {
                    var possibleNeighborsForBase = patternManager.GetPossibleNeighborsForPatternInDir(patternIndexAtNeighbor,neighbor.DirectionFromBase.GetOppositeDirection());
                    possibleIndices.UnionWith(possibleNeighborsForBase);
                }
                if(!possibleIndices.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

