using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Vectors
{
    public readonly struct Int3 : I3dIndexable
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

        public I3dIndexable Subtract(I3dIndexable other) => new Int3(XIndex - other.XIndex, YIndex - other.YIndex, ZIndex - other.ZIndex);
        public I3dIndexable Add(I3dIndexable other) => new Int3(XIndex + other.XIndex, YIndex + other.YIndex, ZIndex + other.ZIndex);
        public I3dIndexable Multiply(I3dIndexable other) => new Int3(XIndex * other.XIndex, YIndex * other.YIndex, ZIndex * other.ZIndex);
        public I3dIndexable Multiply3d(int other) => new Int3(XIndex * other, YIndex * other, ZIndex * other);

        public I2dIndexable Subtract(I2dIndexable other) => new Int2(XIndex - other.XIndex, YIndex - other.YIndex);
        public I2dIndexable Add(I2dIndexable other) => new Int2(XIndex + other.XIndex, YIndex + other.YIndex);
        public I2dIndexable Multiply(I2dIndexable other) => new Int2(XIndex * other.XIndex, YIndex * other.YIndex);
        public I2dIndexable Multiply(int other) => new Int2(XIndex * other, YIndex * other);

        public bool Equals(I2dIndexable other) => !(other is I3dIndexable threeDee && threeDee.ZIndex != ZIndex) && 
            other is I2dIndexable && 
            this.XIndex == other.XIndex && 
            this.YIndex == other.YIndex;

        public bool Equals(I3dIndexable other) => other is I3dIndexable && 
            this.XIndex == other.XIndex &&
            this.YIndex == other.YIndex && 
            this.ZIndex == other.ZIndex;
    }
}
