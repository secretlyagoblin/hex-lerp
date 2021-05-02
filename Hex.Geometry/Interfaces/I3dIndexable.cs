using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dIndexable : I2dIndexable
    {
        int ZIndex { get; }
    }
}
