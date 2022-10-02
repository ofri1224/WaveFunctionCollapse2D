using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public static class DirectionHelper
    {
        public static Direction GetOppositeDirection(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    return direction;
            }
        }
        public static Direction RotateClockwise(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Down;
                default:
                    return direction;
            }
        }
        public static Direction RotateAntiClockwise(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Up;
                default:
                    return direction;
            }
        }
        public static float ToRotation(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return 0f;
                case Direction.Down:
                    return 180f;
                case Direction.Left:
                    return 270f;
                case Direction.Right:
                    return 90f;
                default:
                    return -1;
            }
        }
        public static Direction RotationToDirection(this float rotation)
        {
            switch (rotation%360)
            {
                case 0:
                    return Direction.Up;
                case 90:
                    return Direction.Right;
                case 180:
                    return Direction.Down;
                case 270:
                    return Direction.Left;
                default:
                    return RotationToDirection(Mathf.Round(rotation/90f));
            }
        }
    }
    
}