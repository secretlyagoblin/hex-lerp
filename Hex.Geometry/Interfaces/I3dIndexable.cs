using Hex.Geometry.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dIndexable : I3dIndexGettable, I3dIntGettable
    {

    }

    public interface I3dIndexGettable : I2dIndexGettable
    {
        int ZIndex { get; }
    }

    public interface I3dIntGettable
    {
        public Int3 Index3d { get; }
    }
}
