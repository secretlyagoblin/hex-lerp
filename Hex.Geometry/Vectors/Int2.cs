using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Int2 : I2dIndexable
    {
        public int XIndex { get; }
        public int YIndex { get; }

        public Int2(int x, int y)
        {
            XIndex = x;
            YIndex = y;
        }

        public override string ToString() => $"{XIndex},{YIndex}";

        public I2dIndexable Subtract(I2dIndexable other) => new Int2(XIndex - other.XIndex, YIndex - other.YIndex);
        public I2dIndexable Add(I2dIndexable other) => new Int2(XIndex + other.XIndex, YIndex + other.YIndex);
        public I2dIndexable Multiply(I2dIndexable other) => new Int2(XIndex * other.XIndex, YIndex * other.YIndex);
        public I2dIndexable Multiply(int other) => new Int2(XIndex * other, YIndex * other);

        public bool Equals(I2dIndexable other) => other is I2dIndexable && this.XIndex == other.XIndex && this.YIndex == other.YIndex;
    }
}
