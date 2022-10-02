using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexWaveFunctionCollapse2D
{
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValuesManager<Tile> valueManager;
        public Tilemap OutputImage{get => outputImage;}
        public TileMapOutput(ValuesManager<Tile> valueManager, Tilemap image)
        {
            this.outputImage = image;
            this.valueManager = valueManager;
        }
        public void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height)
        {
            if(outputValues.Length==0)
            {
                return;
            }
            this.outputImage.ClearAllTiles();
            int[][] valueGrid = manager.ConvertPatternToValues<Tile>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Tile tile = (Tile)this.valueManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    TileData tileData = new TileData();
                    Vector3Int position = new Vector3Int(col,row,0);
                    tile.GetTileData(position,null,ref tileData);
                    this.outputImage.SetTile(position,tile);
                    this.outputImage.SetTransformMatrix(position,tileData.transform);
                }
            }
        }
    }
}

