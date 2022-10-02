using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse2D
{
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValuesManager<TileBase> valueManager;
        public Tilemap OutputImage => throw new System.NotImplementedException();
        public TileMapOutput(ValuesManager<TileBase> valueManager, Tilemap image)
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
            int[][] valueGrid = manager.ConvertPatternToValues<TileBase>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = (TileBase)this.valueManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    this.outputImage.SetTile(new Vector3Int(col,row,0),tile);
                }
            }
        }
    }
}

