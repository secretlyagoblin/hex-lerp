using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector3 : I3dPositionGettable, IEquatable<Vector3>
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

        public Vector3 Subtract(Vector3 other) => new(XPos - other.XPos, YPos - other.YPos, ZPos - other.ZPos);
        public Vector3 Add(Vector3 other) => new(XPos + other.XPos, YPos + other.YPos, ZPos + other.ZPos);
        public Vector3 Multiply(Vector3 other) => new(XPos * other.XPos, YPos * other.YPos, ZPos * other.ZPos);
        public Vector3 Multiply(double other) => new(XPos * other, YPos * other, ZPos * other);

        public override bool Equals(object obj) => obj is Vector3 vector && Equals(vector);

        public bool Equals(Vector3 other)
        {
            return XPos.EqualsWithinTolerance(other.XPos) &&
                   YPos.EqualsWithinTolerance(other.YPos) &&
                   ZPos.EqualsWithinTolerance(other.ZPos);
        }

        public override int GetHashCode() => HashCode.Combine(XPos, YPos, ZPos);

        public static Vector3 operator +(Vector3 a, Vector3 b) => a.Add(b);
        public static Vector3 operator -(Vector3 a, Vector3 b) => a.Subtract(b);
        public static Vector3 operator *(Vector3 a, Vector3 b) => a.Multiply(b);
        public static Vector3 operator *(Vector3 a, double b) => a.Multiply(b);

        public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

        public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);
    }
}
