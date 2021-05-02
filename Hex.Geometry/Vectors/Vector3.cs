using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector3 : I3dPositionable
    {
        public Vector3(double x, double y, double z)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
        }

        public double XPos { get; }
        public double YPos { get; }
        public double ZPos { get; }

        public override string ToString()
        {
            return $"{XPos},{YPos},{ZPos}";
        }
    }
}
