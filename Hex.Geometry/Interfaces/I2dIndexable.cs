using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I2dIndexable : IEquatable<I2dIndexable>
    {
        int XIndex { get; }
        int YIndex { get; }

        public I2dIndexable Subtract(I2dIndexable other);
        public I2dIndexable Add(I2dIndexable other);
        public I2dIndexable Multiply(I2dIndexable other);
        public I2dIndexable Multiply(int other);

        public static I2dIndexable Subtract(I2dIndexable a, I2dIndexable b) => a.Subtract(b);
        public static I2dIndexable Add(I2dIndexable a, I2dIndexable b) => a.Add(b);
        public static I2dIndexable Multiply(I2dIndexable a, I2dIndexable b) => a.Multiply(b);
        public static I2dIndexable Multiply(I2dIndexable a, int b) => a.Multiply(b);

        public static I2dIndexable operator +(I2dIndexable a, I2dIndexable b) => Add(a, b);
        public static I2dIndexable operator -(I2dIndexable a, I2dIndexable b) => Subtract(a, b);
        public static I2dIndexable operator *(I2dIndexable a, I2dIndexable b) => Multiply(a, b);
        public static I2dIndexable operator *(I2dIndexable a, int b) => Multiply(a, b);


    }
}
