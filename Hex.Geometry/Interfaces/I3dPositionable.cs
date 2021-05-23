using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dPositionable : I2dPositionable, IEquatable<I3dPositionable>
    {
        double ZPos { get; }

        public I3dPositionable Subtract(I3dPositionable other);
        public I3dPositionable Add(I3dPositionable other);
        public I3dPositionable Multiply(I3dPositionable other);
        public I3dPositionable Multiply3d(double other);

        public static I3dPositionable Subtract(I3dPositionable a, I3dPositionable b) => a.Subtract(b);
        public static I3dPositionable Add(I3dPositionable a, I3dPositionable b) => a.Add(b);
        public static I3dPositionable Multiply(I3dPositionable a, I3dPositionable b) => a.Multiply(b);
        public static I3dPositionable Multiply(I3dPositionable a, double b) => a.Multiply3d(b);

        public static I3dPositionable operator +(I3dPositionable a, I3dPositionable b) => Add(a, b);
        public static I3dPositionable operator -(I3dPositionable a, I3dPositionable b) => Subtract(a, b);
        public static I3dPositionable operator *(I3dPositionable a, I3dPositionable b) => Multiply(a, b);
        public static I3dPositionable operator *(I3dPositionable a, double b) => Multiply(a, b);
    }
}
