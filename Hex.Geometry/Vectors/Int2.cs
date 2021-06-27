using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Int2 : I2dIndexGettable, IEquatable<Int2>
    {
        public int XIndex { get; }
        public int YIndex { get; }

        public Int2(int x, int y)
        {
            XIndex = x;
            YIndex = y;
        }

        public override string ToString() => $"{XIndex},{YIndex}";

        public Int2 Subtract(Int2 other) => new(XIndex - other.XIndex, YIndex - other.YIndex);
        public Int2 Add(Int2 other) => new(XIndex + other.XIndex, YIndex + other.YIndex);
        public Int2 Multiply(Int2 other) => new(XIndex * other.XIndex, YIndex * other.YIndex);
        public Int2 Multiply(int other) => new(XIndex * other, YIndex * other);

        public bool Equals(Int2 other) => XIndex == other.XIndex && this.YIndex == other.YIndex;
        public override bool Equals(object obj) => obj is Int2 @int && Equals(@int);

        public static Int2 Subtract(Int2 a, Int2 b) => a.Subtract(b);
        public static Int2 Add(Int2 a, Int2 b) => a.Add(b);
        public static Int2 Multiply(Int2 a, Int2 b) => a.Multiply(b);
        public static Int2 Multiply(Int2 a, int b) => a.Multiply(b);

        public override int GetHashCode() => HashCode.Combine(XIndex, YIndex);

        public static Int2 operator +(Int2 a, Int2 b) => Add(a, b);
        public static Int2 operator -(Int2 a, Int2 b) => Subtract(a, b);
        public static Int2 operator *(Int2 a, Int2 b) => Multiply(a, b);
        public static Int2 operator *(Int2 a, int b) => Multiply(a, b);

        public static bool operator ==(Int2 left, Int2 right) => left.Equals(right);
        public static bool operator !=(Int2 left, Int2 right) => !(left == right);
    }
}
