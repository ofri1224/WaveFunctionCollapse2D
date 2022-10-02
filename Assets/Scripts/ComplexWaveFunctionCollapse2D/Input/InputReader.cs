using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexWaveFunctionCollapse2D
{

    public class InputReader : IInputReader<Tile>
    {
        private Tilemap _inputTilemap;
        public InputReader(Tilemap input)
        {
            _inputTilemap = input;
        }
        public IValue<Tile>[][] ReadInputToGrid()
        {
            TileValue[][] grid =  ReadInputTileMap();
            
            TileValue[][] valuesGrid = null;
            if(grid!=null)
            {
                valuesGrid = Helper.CreateJaggedArray<TileValue[][]>(grid.Length,grid[0].Length);
                for (int row = 0; row < grid.Length; row++)
                {
                    for (int col = 0; col < grid[0].Length; col++)
                    {
                        valuesGrid[row][col] = grid[row][col];
                        //valuesGrid[row][col] = new TileValue(grid[row][col],grid[row][col].transform);
                    }
                }
            }
            return valuesGrid;
        }

        private TileValue[][] ReadInputTileMap()
        {
            InputImageParameters imageParameters = new InputImageParameters(_inputTilemap);
            //int amount = imageParameters.tilesStack.Count;
            // for (int i = 0; i < amount; i++)
            // {
            //     TileContainer _tileContainer = imageParameters.tilesStack.Dequeue();
            //     //Debug.Log(_tileContainer.Tile.Value.transform);
            //     imageParameters.tilesStack.Enqueue(_tileContainer);
            // }
            return CreateTileBaseGrid(imageParameters);
        }

        private TileValue[][] CreateTileBaseGrid(InputImageParameters imageParameters)
        {
            TileValue[][] inputTilesGrid = Helper.CreateJaggedArray<TileValue[][]>(imageParameters.Height,imageParameters.Width);
            for (int row = 0; row < imageParameters.Height; row++)
            {
                for (int col = 0; col < imageParameters.Width; col++)
                {
                    inputTilesGrid[row][col] = imageParameters.tilesStack.Dequeue().Tile;
                }
            }
            // Debug.Log(inputTilesGrid.Length);
            // Debug.Log(inputTilesGrid[0].Length);
            return inputTilesGrid;
        }

        public class InputImageParameters
        {
            Vector2Int? bottomRightTileCoords = null;
            Vector2Int? topLeftTileCoords = null;
            BoundsInt inputTileMapBounds;
            TileValue[] inputTileMapTilesArray;
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
                //Debug.Log(inputTileMapBounds.size.x+","+inputTileMapBounds.size.y+","+inputTileMapBounds.size.z);
                int PositionsWithinAmount = inputTileMapBounds.size.x*inputTileMapBounds.size.y;
                this.inputTileMapTilesArray = new TileValue[PositionsWithinAmount];
                BoundsInt.PositionEnumerator Enumerator =  this.inputTileMapBounds.allPositionsWithin;
                for (int i = 0; i < PositionsWithinAmount; i++)
                {
                    Enumerator.MoveNext();
                    this.inputTileMapTilesArray[i] = new TileValue(this.inputTileMap.GetTile<Tile>(Enumerator.Current),this.inputTileMap.GetTransformMatrix(Enumerator.Current));
                    // if(this.inputTileMapTilesArray[i].Value!=null)
                    // {
                        
                    //     //this.inputTileMapTilesArray[i].transform = this.inputTileMap.GetTransformMatrix(Enumerator.Current);
                    //     Debug.Log(this.inputTileMapTilesArray[i].Value.transform);
                    // }
                    
                }
                
                //this.inputTileMap.GetTile<Tile>
                //this.inputTileMapTilesArray = this.inputTileMap.GetTilesBlock(this.inputTileMapBounds);
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
                        TileValue tile = inputTileMapTilesArray[index];
                        if(bottomRightTileCoords==null&&tile.Value!=null)
                        {
                            bottomRightTileCoords = new Vector2Int(col,row);
                        }
                        if(tile.Value!=null)
                        {
                            
                            tilesStack.Enqueue(new TileContainer(tile,col,row,tile.direction));
                            topLeftTileCoords = new Vector2Int(col,row);
                        }
                    }
                }
            }
        }
    }
}
