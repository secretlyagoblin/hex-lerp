using Hex.Geometry.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface I2dPositionable : I2dPositionGettable, IVector2Gettable
    {

    }

    public interface I2dPositionGettable
    {
        double XPos { get; }
        double YPos { get; }
    }

    public interface IVector2Gettable
    {
        public Vector2 Position2d { get; }
    }
}
