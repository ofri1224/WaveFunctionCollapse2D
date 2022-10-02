using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class Pattern
    {
        private int _index;
        private int[][] _grid;

        public string HashIndex{get;set;}
        public int Index { get => _index; }

        public Pattern(int[][] grid,string HashCode,int index)
        {
            _grid = grid;
            HashIndex = HashCode;
            _index = index;
        }

        public void SetGridValue(int x, int y, int value)
        {
            _grid[y][x] = value;
        }
        
        public int GetGridValue(int x,int y)
        {
            return _grid[y][x];
        }

        public bool CheckValAtPos(int x,int y,int value)
        {
            return value.Equals(GetGridValue(x,y));
        }

        internal bool ComparePatterns(Direction dir,Pattern pattern)
        {
            int[][] Grid1 = GetGridValuesInDir(dir);
            int[][] Grid2 = pattern.GetGridValuesInDir(dir.GetOppositeDirection());

            for (int x = 0; x < Grid1.Length; x++)
            {
                for (int y = 0; y < Grid1[0].Length; y++)
                {
                    if(Grid1[x][y]!=Grid2[x][y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[][] GetGridValuesInDir(Direction dir)
        {
            int[][] gridPart;
            switch (dir)
            {
                
                case Direction.Up:
                    gridPart = Helper.CreateJaggedArray<int[][]>(_grid.Length-1,_grid.Length);
                    ReadGridPart(0,_grid.Length,1,_grid.Length,gridPart);
                    break;
                case Direction.Down:
                    gridPart = Helper.CreateJaggedArray<int[][]>(_grid.Length-1,_grid.Length);
                    ReadGridPart(0,_grid.Length,0,_grid.Length-1,gridPart);
                    break;
                case Direction.Left:
                    gridPart = Helper.CreateJaggedArray<int[][]>(_grid.Length,_grid.Length-1);
                    ReadGridPart(0,_grid.Length-1,0,_grid.Length,gridPart);
                    break;
                case Direction.Right:
                    gridPart = Helper.CreateJaggedArray<int[][]>(_grid.Length,_grid.Length-1);
                    ReadGridPart(1,_grid.Length,0,_grid.Length,gridPart);
                    break;
                default:
                        return _grid;
            }
            return gridPart;
        }

        private void ReadGridPart(int Xmin, int Xmax,int Ymin,int Ymax,int[][] grid)    
        {
            List<int> tempList = new List<int>();

            for (int y = Ymin; y < Ymax; y++)
            {
                for (int x = Xmin; x < Xmax; x++)
                {
                    tempList.Add(_grid[y][x]);
                }
            }
            
            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % grid.Length;
                int y = i / grid.Length;
                grid[x][y] = tempList[i];
            }
        }
    }
}

