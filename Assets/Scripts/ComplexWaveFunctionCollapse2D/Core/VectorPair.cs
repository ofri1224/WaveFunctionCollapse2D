using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class VectorPair
    {
        public Vector2Int BaseCellPosition { get; set; }
        public Vector2Int CellToPropagatePosition { get; set; }

        public Vector2Int PreviousCellPosition {get;set;}
        public Direction DirectionFromBase { get; set; }
        public VectorPair(Vector2Int baseCellPosition, Vector2Int cellToPropagatePosition,Direction directionFromBase, Vector2Int previousCellPosition)
        {
            this.BaseCellPosition = baseCellPosition;
            this.CellToPropagatePosition = cellToPropagatePosition;
            this.DirectionFromBase = directionFromBase;
            this.PreviousCellPosition = previousCellPosition;
        }
        public bool CellPreviouslyChecked()
        {
            return PreviousCellPosition == CellToPropagatePosition;
        }
    }
}

