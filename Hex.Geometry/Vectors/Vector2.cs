using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector2 : I2dPositionable
    {
        public double XPos { get; }
        public double YPos { get; }

        public Vector2(double x, double y)
        {
            XPos = x;
            YPos = y;
        }

        public override string ToString() => $"{XPos},{YPos}";

        public I2dPositionable Subtract(I2dPositionable other) => new Vector2(XPos - other.XPos, YPos - other.YPos);
        public I2dPositionable Add(I2dPositionable other) => new Vector2(XPos + other.XPos, YPos + other.YPos);
        public I2dPositionable Multiply(I2dPositionable other) => new Vector2(XPos * other.XPos, YPos * other.YPos);
        public I2dPositionable Multiply(double other) => new Vector2(XPos * other, YPos * other);

        public bool Equals(I2dPositionable other) => other is I2dPositionable && this.XPos == other.XPos && this.YPos == other.YPos;        
    }
}
