using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public static class PatternFinder
    {
        internal static PatternDataResult GetPatternDataFromGrid<T>(ValuesManager<T> valuesManager, int patternSize, bool equalsWeight,bool wrap)
        {
            Dictionary<string, PatternData> patternHashCodeDict = new Dictionary<string, PatternData>();
            Dictionary<int , PatternData> patternIndexDict = new Dictionary<int, PatternData>();
            Vector2 GridSize = valuesManager.GetGridSize();
            int patternGridSizeX = 0, patternGridSizeY = 0;
            int rowMin = 0, colMin = 0, rowMax = 0, colMax = 0;
            //int rowMin = -1, colMin = -1, rowMax = -1, colMax = -1;
            //if(patternSize < 3)
            //{
                // patternGridSizeX = (int)GridSize.x + 3 - patternSize;
                // patternGridSizeY = (int)GridSize.y + 3 - patternSize;
                // rowMax = patternGridSizeY-1;
                // colMax = patternGridSizeX-1;
            //}
            // else
            // {
            //     patternGridSizeX = (int)GridSize.x + patternSize - 1;
            //     patternGridSizeY = (int)GridSize.y + patternSize - 1;
            //     rowMin = 1-patternSize;
            //     colMin = 1-patternSize;
            //     rowMax = (int)GridSize.y;
            //     colMax = (int)GridSize.x;
            // }
            patternGridSizeX = (int)GridSize.x;
            patternGridSizeY = (int)GridSize.y;
            //rowMin = 0;
            //colMin = 0;
            rowMax = (int)GridSize.y;
            colMax = (int)GridSize.x;
            int[][] PatternIndicesGrid = Helper.CreateJaggedArray<int[][]>(patternGridSizeY,patternGridSizeX);
            int totalFrequency = 0, patternIndex = 0;

            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = colMin; col < colMax; col++)
                {
                    int[][] gridValues = valuesManager.GetPatternValuesFromGridPos(col,row,patternSize);
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if(!patternHashCodeDict.ContainsKey(hashValue))
                    {
                        Pattern pattern = new Pattern(gridValues,hashValue,patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashCodeDict,patternIndexDict,hashValue,pattern);
                    }
                    else
                    {
                        if (!equalsWeight)
                        {
                            patternIndexDict[patternHashCodeDict[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }
                    totalFrequency++;
                    // if(patternSize<3)
                    // {
                    //     PatternIndicesGrid[row+1][col+1] = patternHashCodeDict[hashValue].Pattern.Index;
                    // }
                    // else
                    // {
                    //    PatternIndicesGrid[row+patternSize-1][col+patternSize-1] = patternHashCodeDict[hashValue].Pattern.Index; 
                    // }
                    PatternIndicesGrid[row][col] = patternHashCodeDict[hashValue].Pattern.Index;
                }
            }
            CalculateRelativeFrequency(patternIndexDict,totalFrequency);
            return new PatternDataResult(PatternIndicesGrid,patternIndexDict);
        }

        private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDict, int totalFrequency)
        {
            foreach (var item in patternIndexDict.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }

        private static void AddNewPattern(Dictionary<string, PatternData> patternHashCodeDict, Dictionary<int, PatternData> patternIndexDict, string hashValue, Pattern pattern)
        {
            PatternData data = new PatternData(pattern);
            patternHashCodeDict.Add(hashValue,data);
            patternIndexDict.Add(pattern.Index,data);
        }

        internal static Dictionary<int, PatternNeighbors> FindPossibleNeighborsForAllPatterns(IFIndNeighborStrategy strategy, PatternDataResult patternFinderResult)
        {
            return strategy.FindNeighbors(patternFinderResult);
        }

        public static PatternNeighbors CheckNeighborsInEachDirection(int x,int y, PatternDataResult patternDataResult)
        {
            PatternNeighbors patternNeighbors = new PatternNeighbors();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResult.GetNeighborInDir(x,y,dir);
                if(possiblePatternIndex>0)
                {
                    patternNeighbors.AddPatternToDict(dir,possiblePatternIndex);
                }
            }
            return patternNeighbors;
        }

        public static void AddNeighborsToDict(Dictionary<int ,PatternNeighbors> dict, int patternIndex, PatternNeighbors Neighbors)
        {
            if(!dict.ContainsKey(patternIndex))
            {
                dict.Add(patternIndex,Neighbors);
            }
            else
            {
                dict[patternIndex].AddNeighbor(Neighbors);
            }
        }
    }
}