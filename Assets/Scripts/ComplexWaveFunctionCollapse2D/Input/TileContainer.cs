using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexWaveFunctionCollapse2D
{
    // public class TileContainer
    // {
    //     public TileBase Tile { get; set; }
    //     public int X { get; set; }
    //     public int Y { get; set; }
    //     public TileContainer(TileBase tile, int x, int y)
    //     {
    //         this.Tile = tile;
    //         this.X = x;
    //         this.Y = y;
    //     }
    // }
    
    public class TileContainer
    {
        public TileValue Tile { get; set; }
        public Direction direction;
        public int X { get; set; }
        public int Y { get; set; }
        public TileContainer(TileValue tile, int x, int y,Direction direction)
        {
            this.Tile = tile;
            this.X = x;
            this.Y = y;
        }
    }
}

