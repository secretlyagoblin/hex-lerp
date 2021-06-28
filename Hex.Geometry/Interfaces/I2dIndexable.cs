using Hex.Geometry.Vectors;
using System;
using System.Collections.Generic;
using System.Text;



namespace Hex.Geometry.Interfaces
{
    public interface I2dIndexable : I2dIndexGettable, I2dIntGettable
    {

    }

    public interface I2dIndexGettable
    {
        int XIndex { get; }
        int YIndex { get; }
    }

    public interface I2dIntGettable
    {
        public Int2 Index2d {get;} 
    }
}
