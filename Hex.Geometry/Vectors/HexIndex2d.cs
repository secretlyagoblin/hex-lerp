using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct HexIndex2d : IEquatable<HexIndex2d>, I2dIndexable
    {
        public HexIndex2d(int x, int y)
        {
            XIndex = x;
            YIndex = y;
        }

        public int XIndex { get; }
        public int YIndex { get; }

        public static HexIndex2d Add(HexIndex2d a, HexIndex2d b) => new HexIndex2d(a.XIndex + b.XIndex, a.YIndex + b.YIndex);
        public static HexIndex2d operator +(HexIndex2d a, HexIndex2d b) => Add(a, b);

        public static HexIndex2d Multiply(HexIndex2d a, HexIndex2d b) => new HexIndex2d(a.XIndex * b.XIndex, a.YIndex * b.YIndex);
        public static HexIndex2d operator *(HexIndex2d a, HexIndex2d b) => Multiply(a, b);
        public static HexIndex2d Multiply(HexIndex2d a, int b) => new HexIndex2d(a.XIndex * b, a.YIndex * b);

        public static HexIndex2d operator *(HexIndex2d a, int b) => Multiply(a, b);
        public static bool operator ==(HexIndex2d left, HexIndex2d right) => left.Equals(right);
        public static bool operator !=(HexIndex2d left, HexIndex2d right) => !(left == right);

        public override bool Equals(object obj) => obj is HexIndex2d @int && Equals(@int);

        public bool Equals(HexIndex2d other)
        {
            return XIndex == other.XIndex &&
                   YIndex == other.YIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + XIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + YIndex.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{XIndex},{YIndex}";
        }
    }
}
