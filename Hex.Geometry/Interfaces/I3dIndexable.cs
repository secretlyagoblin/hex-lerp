using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dIndexable : I2dIndexable
    {
        int ZIndex { get; }

        public I3dIndexable Subtract(I3dIndexable other);
        public I3dIndexable Add(I3dIndexable other);
        public I3dIndexable Multiply(I3dIndexable other);
        public I3dIndexable Multiply3d(int other);

        public static I3dIndexable Subtract(I3dIndexable a, I3dIndexable b) => a.Subtract(b);
        public static I3dIndexable Add(I3dIndexable a, I3dIndexable b) => a.Add(b);
        public static I3dIndexable Multiply(I3dIndexable a, I3dIndexable b) => a.Multiply(b);
        public static I3dIndexable Multiply(I3dIndexable a, int b) => a.Multiply3d(b);

        public static I3dIndexable operator +(I3dIndexable a, I3dIndexable b) => Add(a, b);
        public static I3dIndexable operator -(I3dIndexable a, I3dIndexable b) => Subtract(a, b);
        public static I3dIndexable operator *(I3dIndexable a, I3dIndexable b) => Multiply(a, b);
        public static I3dIndexable operator *(I3dIndexable a, int b) => Multiply(a, b);
    }
}
