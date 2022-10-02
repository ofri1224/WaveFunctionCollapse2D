using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse2D
{
    public class InputReader : IInputReader<TileBase>
    {
        private Tilemap _inputTilemap;
        public InputReader(Tilemap input)
        {
            _inputTilemap = input;
        }
        public IValue<TileBase>[][] ReadInputToGrid()
        {
            TileBase[][] grid =  ReadInputTileMap();

            TileBaseValue[][] valuesGrid = null;
            if(grid!=null)
            {
                valuesGrid = (TileBaseValue[][])Helper.CreateJaggedArray<TileBaseValue[][]>(grid.Length,grid[0].Length);
                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[0].Length; col++)
                    {
                        valuesGrid[row][col] = new TileBaseValue(grid[row][col]);
                    }
                }
            }
            return valuesGrid;
        }

        private TileBase[][] ReadInputTileMap()
        {
            InputImageParameters imageParameters = new InputImageParameters(_inputTilemap);
            return CreateTileBaseGrid(imageParameters);
        }

        private TileBase[][] CreateTileBaseGrid(InputImageParameters imageParameters)
        {
            TileBase[][] inputTilesGrid = Helper.CreateJaggedArray<TileBase[][]>(imageParameters.Height,imageParameters.Width);
            for (int row = 0; row < imageParameters.Height; row++)
            {
                for (int col = 0; col < imageParameters.Width; col++)
                {
                    inputTilesGrid[row][col] = imageParameters.tilesStack.Dequeue().Tile;
                }
            }

            return inputTilesGrid;
        }

        public class InputImageParameters
        {
            Vector2Int? bottomRightTileCoords = null;
            Vector2Int? topLeftTileCoords = null;
            BoundsInt inputTileMapBounds;
            TileBase[] inputTileMapTilesArray;
            Queue<TileContainer> _tilesStack = new Queue<TileContainer>();
            private int width = 0, height = 0;
            private Tilemap inputTileMap;

            public Queue<TileContainer> tilesStack { get => _tilesStack; set => _tilesStack = value; }
            public int Height { get => height;}
            public int Width { get => width;}
            public InputImageParameters(Tilemap inputTileMap)
            {
                this.inputTileMap = inputTileMap;
                this.inputTileMapBounds = this.inputTileMap.cellBounds;
                this.inputTileMapTilesArray = this.inputTileMap.GetTilesBlock(this.inputTileMapBounds);
                ExtractNonEmptyTiles();
                VerifyInputTiles();
            }

            private void VerifyInputTiles()
            {
                if(topLeftTileCoords==null || bottomRightTileCoords==null)
                {
                    throw new System.Exception("WFC: Input tilemap is empty");
                }
                int minX = bottomRightTileCoords.Value.x;
                int maxX = topLeftTileCoords.Value.x;
                int minY = bottomRightTileCoords.Value.y;
                int maxY = topLeftTileCoords.Value.y;

                width = Math.Abs(maxX - minX)+1;
                height = Math.Abs(maxY - minY)+1;

                int tileCount = width*height;
                if (tilesStack.Count != tileCount)
                {
                    throw new System.Exception("WFC: Tilemap has empty fields");
                }
                if (tilesStack.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
                {
                    throw new System.Exception("WFC: Tilemap image should be a filled rectangle");
                }
            }

            private void ExtractNonEmptyTiles()
            {
                for (int row = 0; row < inputTileMapBounds.size.y; row++)
                {
                    for (int col = 0; col < inputTileMapBounds.size.x; col++)
                    {
                        int index = col + (row*inputTileMapBounds.size.x);
                        TileBase tile = inputTileMapTilesArray[index];
                        if(bottomRightTileCoords==null&&tile!=null)
                        {
                            bottomRightTileCoords = new Vector2Int(col,row);
                        }
                        if(tile!=null)
                        {
                            tilesStack.Enqueue(new TileContainer(tile,col,row));
                            topLeftTileCoords = new Vector2Int(col,row);
                        }
                    }
                }
            }
        }
    }
}
