using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I2dPositionable : IEquatable<I2dPositionable>
    {
        double XPos { get; }
        double YPos { get; }

        public I2dPositionable Subtract(I2dPositionable other);
        public I2dPositionable Add(I2dPositionable other);
        public I2dPositionable Multiply(I2dPositionable other);
        public I2dPositionable Multiply(double other);

        public static I2dPositionable Subtract(I2dPositionable a, I2dPositionable b) => a.Subtract(b);
        public static I2dPositionable Add(I2dPositionable a, I2dPositionable b) => a.Add(b);
        public static I2dPositionable Multiply(I2dPositionable a, I2dPositionable b) => a.Multiply(b);
        public static I2dPositionable Multiply(I2dPositionable a, double b) => a.Multiply(b);

        public static I2dPositionable operator +(I2dPositionable a, I2dPositionable b) => Add(a, b);
        public static I2dPositionable operator -(I2dPositionable a, I2dPositionable b) => Subtract(a, b);
        public static I2dPositionable operator *(I2dPositionable a, I2dPositionable b) => Multiply(a, b);
        public static I2dPositionable operator *(I2dPositionable a, double b) => Multiply(a, b);
    }
}
