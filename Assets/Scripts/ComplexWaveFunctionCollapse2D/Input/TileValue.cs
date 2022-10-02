using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexWaveFunctionCollapse2D
{
    public class TileValue : IValue<Tile>
    {
        Tile tile;
        //Matrix4x4 transform;
        Direction _direction;
        public TileValue(Tile tile,Matrix4x4 transform)
        {
            this.tile = tile;
            _direction = transform.rotation.eulerAngles.z.RotationToDirection();
            //this.transform = transform;
        }

        public Tile Value {get{
            if(this.tile==null){return null;}
            //this.tile.transform = this.transform;
            return this.tile;
        }}
        public Direction direction { get => _direction;}

        public bool Equals(IValue<Tile> x, IValue<Tile> y)
        {
            return x.Value.sprite == y.Value.sprite && x.Value.transform == y.Value.transform;
            //return x == y;
        }

        public bool Equals(IValue<Tile> other)
        {
            return other.Value.transform == this.Value.transform && other.Value.sprite==this.Value.sprite;
        }

        public int GetHashCode(IValue<Tile> obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.tile.GetHashCode();
        }
    }
}
