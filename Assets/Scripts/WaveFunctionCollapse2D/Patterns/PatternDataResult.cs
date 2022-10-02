using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public class PatternDataResult
    {
        private int[][] patternIndicesGrid;
        public Dictionary<int, PatternData> PatternIndexDict { get; private set; }

        public PatternDataResult(int[][] patternIndicesGrid, Dictionary<int, PatternData> patternIndexDict)
        {
            this.patternIndicesGrid = patternIndicesGrid;
            PatternIndexDict = patternIndexDict;
        }
        public int GetGridLengthX()
        {
            return patternIndicesGrid[0].Length;
        }
        public int GetGridLengthY()
        {
            return patternIndicesGrid.Length;
        }

        public int GetIndexAt(int x,int y)
        {
            return patternIndicesGrid[y][x];
        }

        public int GetNeighborInDir(int x, int y, Direction dir)
        {
            if(!patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y))
            {
                return -1;
            }
            switch (dir)
            {
                case Direction.Up:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y+1))
                    {
                        return GetIndexAt(x,y+1);
                    }
                    return -1;
                case Direction.Down:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y-1))
                    {
                        return GetIndexAt(x,y-1);
                    }
                    return -1;
                case Direction.Left:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x+1,y))
                    {
                        return GetIndexAt(x+1,y);
                    }
                    return -1;
                case Direction.Right:
                    if(patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x-1,y))
                    {
                        return GetIndexAt(x-1,y);
                    }
                    return -1;
                default:
                    return -1;
            }
        }
    }
}