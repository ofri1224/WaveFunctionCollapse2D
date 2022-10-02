using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexWaveFunctionCollapse2D
{
    public class TileBaseValue : IValue<TileBase>
    {
        TileBase tileBase;
        public TileBaseValue(TileBase tileBase)
        {
            this.tileBase = tileBase;
        }

        public TileBase Value => this.tileBase;

        public bool Equals(IValue<TileBase> x, IValue<TileBase> y)
        {
            return x == y;
        }

        public bool Equals(IValue<TileBase> other)
        {
            return other.Value == this.Value;
        }

        public int GetHashCode(IValue<TileBase> obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.tileBase.GetHashCode();
        }
    }
}
