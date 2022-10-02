using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse2D;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace different
{
    public class Test : MonoBehaviour
    {
        public Tilemap inputTilemap;
        public Tilemap outputTilemap;
        public int patternSize;
        public int maxIteration=500;
        public int outputHeight=5;
        public int outputWidth=5;
        public bool EqualWeights=false;
        public bool Wrap=false;
        private WFCCore core;
        //private ValuesManager<TileBase> valuesManager;
        private ValuesManager<TileBase> valuesManager;
        private TileMapOutput output;
        private PatternManager manager;
        public void CreateWFC()
        {
            InputReader reader = new InputReader(inputTilemap);
            IValue<TileBase>[][] grid = reader.ReadInputToGrid();
            Debug.Log(grid.Length);
            Debug.Log(grid[0].Length);
            valuesManager = new ValuesManager<TileBase>(grid);
            //valuesManager = new ValuesManager<Tile>(grid);
            //Debug.Log(valuesManager.GetGridSize());
            manager = new PatternManager(patternSize);
            manager.ProcessGrid(valuesManager,EqualWeights);
            core = new WFCCore(outputWidth,outputHeight,maxIteration,manager);
        }
        public void CreateTilemap()
        {
            output = new TileMapOutput(valuesManager,outputTilemap);
            var result = core.CreateOutputGrid();
            output.CreateOutput(manager,result,outputWidth,outputHeight);
        }

        public void SaveTilemap()
        {
            if(output.OutputImage!=null)
            {
                outputTilemap = output.OutputImage;
                GameObject objectToSave = outputTilemap.gameObject;

                PrefabUtility.SaveAsPrefabAsset(objectToSave,"Assets/Saved/output.prefab");
            }
        }
    }

}
