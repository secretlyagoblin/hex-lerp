using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Interfaces
{
    public interface IBlerpable<T>
    {
        T Blerp(T b, T c, I3dPositionable weight);
    }
}
