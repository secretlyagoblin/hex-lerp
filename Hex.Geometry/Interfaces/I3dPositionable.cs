using Hex.Geometry.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I3dPositionable : I3dPositionGettable, IVector3Gettable
    {
    }

    public interface I3dPositionGettable : I2dPositionGettable
    {
        double ZPos { get; }
    }

    public interface IVector3Gettable
    {
        public Vector3 Position3d { get; }
    }


}