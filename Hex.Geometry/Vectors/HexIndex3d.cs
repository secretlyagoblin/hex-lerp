using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct HexIndex3d : IEquatable<HexIndex3d>, I3dIndexable
    {
        public HexIndex3d(int x, int y, int z)
        {
            XIndex = x;
            YIndex = y;
            ZIndex = z;
        }

        public int XIndex { get; }
        public int YIndex { get; }
        public int ZIndex { get; }

        public static HexIndex3d Add(HexIndex3d a, HexIndex3d b) => new HexIndex3d(a.XIndex + b.XIndex, a.YIndex + b.YIndex, a.ZIndex + b.ZIndex);
        public static HexIndex3d operator +(HexIndex3d a, HexIndex3d b) => Add(a,b);

        public static HexIndex3d Subtract(HexIndex3d a, HexIndex3d b) => new HexIndex3d(a.XIndex - b.XIndex, a.YIndex - b.YIndex, a.ZIndex - b.ZIndex);
        public static HexIndex3d operator -(HexIndex3d a, HexIndex3d b) => Subtract(a, b);

        public static HexIndex3d Multiply(HexIndex3d a, HexIndex3d b) => new HexIndex3d(a.XIndex * b.XIndex, a.YIndex * b.YIndex, a.ZIndex * b.ZIndex);
        public static HexIndex3d operator *(HexIndex3d a, HexIndex3d b) => Multiply(a, b);
        public static HexIndex3d Multiply(HexIndex3d a, int b) => new HexIndex3d(a.XIndex * b, a.YIndex * b, a.ZIndex * b);
        public static HexIndex3d operator *(HexIndex3d a, int b) => Multiply(a, b);
        public static bool operator ==(HexIndex3d left, HexIndex3d right) => left.Equals(right);
        public static bool operator !=(HexIndex3d left, HexIndex3d right) => !(left == right);

        public override bool Equals(object obj) => obj is HexIndex3d @int && Equals(@int);

        public bool Equals(HexIndex3d other)
        {
            return XIndex == other.XIndex &&
                   YIndex == other.YIndex &&
                   ZIndex == other.ZIndex;
        }

        public override int GetHashCode()
        {
            int hashCode = -307843816;
            hashCode = hashCode * -1521134295 + XIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + YIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + ZIndex.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{XIndex},{YIndex},{ZIndex}";
        }
    }
}
