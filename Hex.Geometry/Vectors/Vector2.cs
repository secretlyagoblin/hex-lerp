using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Vector2 : I2dPositionable, IEquatable<Vector2>
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

        public static Vector2 Subtract(Vector2 a, Vector2 b) => new Vector2(a.XPos - b.XPos, a.YPos - b.YPos);
        public static Vector2 Add(Vector2 a, Vector2 b) => new Vector2(a.XPos + b.XPos, a.YPos + b.YPos);
        public static Vector2 Multiply(Vector2 a, Vector2 b) => new Vector2(a.XPos * b.XPos, a.YPos * b.YPos);
        public static Vector2 Multiply(Vector2 a, double b) => new Vector2(a.XPos * b, a.YPos * b);

        public override bool Equals(object obj) => obj is Vector2 vector && Equals(vector);

        public bool Equals(Vector2 other)
        {
            return XPos == other.XPos &&
                   YPos == other.YPos;
        }

        public override int GetHashCode()
        {
            int hashCode = -1416500419;
            hashCode = hashCode * -1521134295 + XPos.GetHashCode();
            hashCode = hashCode * -1521134295 + YPos.GetHashCode();
            return hashCode;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right) => Add(left,right);
        public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(left,right);
        public static Vector2 operator *(Vector2 left, Vector2 right) => Multiply(left, right);
        public static Vector2 operator *(Vector2 left, double right) => Multiply(left, right);

        public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);
    }
}
