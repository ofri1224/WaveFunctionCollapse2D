using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public class PatternManager
    {
        Dictionary<int , PatternData> patternDataIndexDict;
        Dictionary<int , PatternNeighbors> patternPossibleNeighborsIndexDict;
        int patternSize = -1;
        IFIndNeighborStrategy strategy;

        public PatternManager(int patternSize)
        {
            this.patternSize = patternSize;
        }

        public void ProcessGrid<T>(ValuesManager<T> valuesManager, bool equalsWeight, string strategyName = null)
        {
            NeighborStrategyFactory strategyFactory = new NeighborStrategyFactory();
            strategy = strategyFactory.CreateInstance(strategyName == null ? patternSize.ToString() : strategyName);
            CreatePatterns(valuesManager,strategy,equalsWeight);
        }

        internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth= outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;
            int valueGridWidth = patternOutputWidth +patternSize-1;
            int valueGridHeight = patternOutputHeight +patternSize-1;
            int[][] valueGrid = Helper.CreateJaggedArray<int[][]>(valueGridHeight,valueGridWidth);
            for (int row = 0; row < patternOutputHeight; row++)
            {
                for (int col = 0; col < patternOutputWidth; col++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;
                    GetPatternValues(patternOutputWidth,patternOutputHeight,valueGrid,row,col,pattern);
                }
            }
            return valueGrid;
        }

        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int row, int col, Pattern pattern)
        {
            if(row==patternOutputHeight-1 && col==patternOutputWidth-1)
            {
                for (int row_1 = 0; row_1 < patternSize; row_1++)
                {
                    for (int col_1 = 0; col_1 < patternSize; col_1++)
                    {
                        valueGrid[row+row_1][col+col_1]=pattern.GetGridValue(col_1,row_1);
                    }
                }
            }
            else if(row==patternOutputHeight - 1)
            {
                for (int row_1 = 0; row_1 < patternSize; row_1++)
                {
                    valueGrid[row+row_1][col]=pattern.GetGridValue(0,row_1);
                }
            }
            else if(col==patternOutputWidth - 1)
            {
                for (int col_1 = 0; col_1 < patternSize; col_1++)
                {
                    valueGrid[row][col+col_1]=pattern.GetGridValue(col_1,0);
                }
            }
            else
            {
                valueGrid[row][col] = pattern.GetGridValue(0,0);
            }
        }

        private void CreatePatterns<T>(ValuesManager<T> valuesManager, IFIndNeighborStrategy strategy, bool equalsWeight)
        {
            PatternDataResult patternFinderResult = PatternFinder.GetPatternDataFromGrid<T>(valuesManager, patternSize, equalsWeight);
            
            // TESTING CODE
            StringBuilder builder;
            List<string> lst = new List<string>();
            for (int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    builder.Append(patternFinderResult.GetIndexAt(col,row)+" ");
                }
                lst.Add(builder.ToString());
            }
            lst.Reverse();
            foreach (var item in lst)
            {
                Debug.Log(item);
            }
            // TESTING CODE

            patternDataIndexDict = patternFinderResult.PatternIndexDict;
            GetPatternNeighBors(patternFinderResult,strategy);
        }

        private void GetPatternNeighBors(PatternDataResult patternFinderResult, IFIndNeighborStrategy strategy)
        {
            patternPossibleNeighborsIndexDict = PatternFinder.FindPossibleNeighborsForAllPatterns(strategy,patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDict[index];
        }

        public HashSet<int> GetPossibleNeighborsForPatternInDir(int patternIndex,Direction dir)
        {
            return patternPossibleNeighborsIndexDict[patternIndex].GetNeighborsInDir(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }

        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }

        public int GetNumberOfPatterns()
        {
            return patternDataIndexDict.Count;
        }
    }
}
