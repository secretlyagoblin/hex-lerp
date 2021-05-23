using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector3 : I3dPositionable
    {
        public double XPos { get; }
        public double YPos { get; }
        public double ZPos { get; }

        public Vector3(double x, double y, double z)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
        }

        public override string ToString() => $"{XPos},{YPos},{ZPos}";

        public I3dPositionable Subtract(I3dPositionable other) => new Vector3(XPos - other.XPos, YPos - other.YPos, ZPos - other.ZPos);
        public I3dPositionable Add(I3dPositionable other) => new Vector3(XPos + other.XPos, YPos + other.YPos, ZPos + other.ZPos);
        public I3dPositionable Multiply(I3dPositionable other) => new Vector3(XPos * other.XPos, YPos * other.YPos, ZPos * other.ZPos);
        public I3dPositionable Multiply3d(double other) => new Vector3(XPos * other, YPos * other, ZPos * other);

        public I2dPositionable Subtract(I2dPositionable other) => new Vector2(XPos - other.XPos, YPos - other.YPos);
        public I2dPositionable Add(I2dPositionable other) => new Vector2(XPos + other.XPos, YPos + other.YPos);
        public I2dPositionable Multiply(I2dPositionable other) => new Vector2(XPos * other.XPos, YPos * other.YPos);
        public I2dPositionable Multiply(double other) => new Vector2(XPos * other, YPos * other);

        public bool Equals(I2dPositionable other) => other is I2dPositionable && this.XPos == other.XPos && this.YPos == other.YPos;
        public bool Equals(I3dPositionable other) => other is I2dPositionable twoDee && this.Equals(twoDee) && this.ZPos == other.ZPos;
    }
}
