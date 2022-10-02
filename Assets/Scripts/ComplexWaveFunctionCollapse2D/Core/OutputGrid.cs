using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace ComplexWaveFunctionCollapse2D
{
    public class OutputGrid
    {
        Dictionary<int,HashSet<int>> indexPossiblePatternDict = new Dictionary<int, HashSet<int>>();
        public int width{get;set;}
        public int height{get;set;}
        private int maxNumberOfPatterns = 0;

        public OutputGrid(int width,int height,int numberOfPatterns)
        {
            this.width=width;
            this.height=height;
            this.maxNumberOfPatterns=numberOfPatterns;
            ResetAllPossibilities();
        }

        public void ResetAllPossibilities()
        {
            HashSet<int> allPossiblePatternsList = new HashSet<int>();
            allPossiblePatternsList.UnionWith(Enumerable.Range(0,this.maxNumberOfPatterns).ToList());

            indexPossiblePatternDict.Clear();
            for (int i = 0; i < height*width; i++)
            {
                indexPossiblePatternDict.Add(i, new HashSet<int>(allPossiblePatternsList));
            }
        }

        public bool CheckCellExists(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            return indexPossiblePatternDict.ContainsKey(index);
        }

        private int GetIndexFromCoordinates(Vector2Int position)
        {
            return position.x+width*position.y;
        }

        private Vector2Int GetCoordinatesFromIndex(int index)
        {
            return new Vector2Int(index%width,index/width);
        }

        public bool CheckIfCellIsCollapsed(Vector2Int position)
        {
            return GetPossibleValueForPosition(position).Count <= 1;
        }

        public HashSet<int> GetPossibleValueForPosition(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            if(indexPossiblePatternDict.ContainsKey(index))
            {
                return indexPossiblePatternDict[index];
            }
            else
            {
                return new HashSet<int>();
            }
            
        }

        internal void PrintResultsToConsole()
        {
            StringBuilder builder = null;
            List<string> list = new List<string>();
            for (int row = 0; row < this.height; row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < this.width; col++)
                {
                    var result = GetPossibleValueForPosition(new Vector2Int(col, row));
                    if (result.Count == 1)
                    {
                        builder.Append(result.First() + " ");
                    }
                    else
                    {
                        string newString = "";
                        foreach (var item in result)
                        {
                            newString += item + ",";
                        }
                        builder.Append(newString);
                    }
                }
                list.Add(builder.ToString());
            }
            list.Reverse();
            foreach (var item in list)
            {
                Debug.Log(item);
            }
            Debug.Log("---");
        }

        public bool IsGridSolved()
        {
            return !indexPossiblePatternDict.Any(x => x.Value.Count>1);
        }

        internal bool IsPositionValid(Vector2Int position)
        {
            return Helper.ValidateCoordinates(position.x,position.y,width,height);
        }

        public Vector2Int GetRandomCell()
        {
            int RndIndex = UnityEngine.Random.Range(0,indexPossiblePatternDict.Count);
            return GetCoordinatesFromIndex(RndIndex);
        }

        public void SetPatternAtPosition(int x,int y,int patternIndex)
        {
            int index = GetIndexFromCoordinates(new Vector2Int(x,y));
            indexPossiblePatternDict[index] = new HashSet<int>() {patternIndex};
        }

        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = Helper.CreateJaggedArray<int[][]>(height,width);
            if(!IsGridSolved())
            {
                return Helper.CreateJaggedArray<int[][]>(0,0);
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int index = GetIndexFromCoordinates(new Vector2Int(col,row));
                    returnGrid[row][col] = indexPossiblePatternDict[index].First();
                }
            }
            return returnGrid;
        }
    }
}

