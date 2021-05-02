using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dPositionable : I2dPositionable
    {
        double ZPos { get; }
    }
}
