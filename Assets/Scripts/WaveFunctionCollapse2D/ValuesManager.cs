using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public class ValuesManager<T>
    {
        int[][] _grid;
        Dictionary<int, IValue<T>> valueIndexDict = new Dictionary<int, IValue<T>>();
        int index = 0;

        public ValuesManager(IValue<T>[][] grid)
        {
            CreateIndicesGrid(grid);

        }

        private void CreateIndicesGrid(IValue<T>[][] grid)
        {
            _grid = Helper.CreateJaggedArray<int[][]>(grid.Length, grid[0].Length);
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    SetIndexToGridPosition(grid, row, col);
                }
            }
        }

        private void SetIndexToGridPosition(IValue<T>[][] grid, int row, int col)
        {
            if (valueIndexDict.ContainsValue(grid[row][col]))
            {
                var key = valueIndexDict.FirstOrDefault(x => x.Value.Equals(grid[row][col]));
                _grid[row][col] = key.Key;
            }
            else
            {
                _grid[row][col] = index;
                valueIndexDict.Add(_grid[row][col], grid[row][col]);
                index++;
            }
        }

        public int GetGridValue(int x, int y)
        {
            if (x >= _grid[0].Length || y >= _grid.Length || x < 0 || y < 0)
            {
                throw new System.Exception($"Grid does not contain x:{x} y:{y} value");
            }
            return _grid[y][x];
        }
        public IValue<T> GetValueFromIndex(int index)
        {
            if (valueIndexDict.ContainsKey(index))
            {
                return valueIndexDict[index];
            }
            throw new System.Exception($"index {index} does not exist in dictionary");
        }

        public int GetGridValueWithOffset(int x, int y)
        {
            int yMax = _grid.Length;
            int xMax = _grid[0].Length;
            if (x < 0 && y < 0)
            {
                return GetGridValue(xMax + x, yMax + y);
            }
            if(x<0&&y>=yMax)
            {
                return GetGridValue(xMax + x,y - yMax);
            }
            if(x>=xMax && y<0)
            {
                return GetGridValue(x - xMax,y+yMax);
            }
            if (x>=xMax && y>=yMax)
            {
                return GetGridValue(x-xMax,y-yMax);
            }
            if(x<0)
            {
                return GetGridValue(xMax + x,y);
            }
            if(x>= xMax)
            {
                return GetGridValue(x - xMax,y);
            }
            if(y<0)
            {
                return GetGridValue(x,yMax + y);
            }
            if(y >= yMax)
            {
                return GetGridValue(x,y - yMax);
            }
            return GetGridValue(x,y);       
        }

        public int[][] GetPatternValuesFromGridPos(int x,int y,int patternSize)
        {
            int[][] result = Helper.CreateJaggedArray<int[][]>(patternSize,patternSize);
            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    result[row][col] = GetGridValueWithOffset(x+col,y+row);
                }
            }
            return result;
        }

        public Vector2 GetGridSize()
        {
            if(_grid == null)
            {
                return Vector2.zero;
            }
            return new Vector2(_grid[0].Length,_grid.Length);
        }
    }
}