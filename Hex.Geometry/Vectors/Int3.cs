using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Int3 : I2dIndexGettable, IEquatable<Int3>
    {
        public int XIndex { get; }
        public int YIndex { get; }
        public int ZIndex { get; }

        public Int3(int x, int y, int z)
        {
            XIndex = x;
            YIndex = y;
            ZIndex = z;
        }

        public override string ToString() => $"{XIndex},{YIndex},{ZIndex}";

        public Int3 Subtract(Int3 other) => new(XIndex - other.XIndex, YIndex - other.YIndex, ZIndex - other.ZIndex);
        public Int3 Add(Int3 other) => new(XIndex + other.XIndex, YIndex + other.YIndex, ZIndex + other.ZIndex);
        public Int3 Multiply(Int3 other) => new(XIndex * other.XIndex, YIndex * other.YIndex, ZIndex * other.ZIndex);
        public Int3 Multiply(int other) => new(XIndex * other, YIndex * other, ZIndex * other);

        public override bool Equals(object obj) => obj is Int3 @int && Equals(@int);

        public bool Equals(Int3 other)
        {
            return XIndex == other.XIndex &&
                   YIndex == other.YIndex &&
                   ZIndex == other.ZIndex;
        }

        public override int GetHashCode() => HashCode.Combine(XIndex, YIndex, ZIndex);

        public static Int3 operator +(Int3 a, Int3 b) => a.Add(b);
        public static Int3 operator -(Int3 a, Int3 b) => a.Subtract(b);
        public static Int3 operator *(Int3 a, Int3 b) => a.Multiply(b);
        public static Int3 operator *(Int3 a, int b) => a.Multiply(b);

        public static bool operator ==(Int3 left, Int3 right) => left.Equals(right);

        public static bool operator !=(Int3 left, Int3 right) => !(left == right);
    }
}
