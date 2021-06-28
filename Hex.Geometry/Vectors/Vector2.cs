using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector2 : I2dPositionGettable, IEquatable<Vector2>
    {
        public double XPos { get; }
        public double YPos { get; }

        public Vector2(double x, double y)
        {
            XPos = x;
            YPos = y;
        }

        public override string ToString() => $"{XPos},{YPos}";

        public Vector2 Subtract(Vector2 other) => new(XPos - other.XPos, YPos - other.YPos);
        public Vector2 Add(Vector2 other) => new(XPos + other.XPos, YPos + other.YPos);
        public Vector2 Multiply(Vector2 other) => new(XPos * other.XPos, YPos * other.YPos);
        public Vector2 Multiply(double other) => new(XPos * other, YPos * other);

        public bool Equals(Vector2 other) => this.XPos.EqualsWithinTolerance(other.XPos) && this.YPos.EqualsWithinTolerance(other.YPos);

        public override int GetHashCode() => HashCode.Combine(XPos, YPos);

        public static Vector2 operator +(Vector2 a, Vector2 b) => a.Add(b);
        public static Vector2 operator -(Vector2 a, Vector2 b) => a.Subtract(b);
        public static Vector2 operator *(Vector2 a, Vector2 b) => a.Multiply(b);
        public static Vector2 operator *(Vector2 a, double b) => a.Multiply(b);

        public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

        public override bool Equals(object obj) => obj is Vector2 vector && Equals(vector);
    }
}
