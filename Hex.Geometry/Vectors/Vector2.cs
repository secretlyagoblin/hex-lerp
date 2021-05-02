using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector2 : I2dPositionable
    {
        public Vector2(double x, double y)
        {
            XPos = x;
            YPos = y;
        }

        public double XPos { get; }
        public double YPos { get; }

        public override string ToString()
        {
            return $"{XPos},{YPos}";
        }
    }
}
