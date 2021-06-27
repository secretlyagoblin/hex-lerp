using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry.Interfaces
{
    public interface IHex<T> : I3dIndexable, I2dPositionable where T: IHexData<T>
    {
        T Payload { get; }
    }
}
