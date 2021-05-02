using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry
{
    public interface I3dIndexable : I2dIndexable
    {
        int ZIndex { get; }
    }
}
