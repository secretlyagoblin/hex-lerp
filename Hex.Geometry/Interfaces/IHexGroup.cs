using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry.Interfaces
{
    public interface IHexGroup<T> where T : IHexData<T>
    {
        IHexGroup<T> Subdivide(int amount);
        IEnumerable<IHex<T>> GetHexes();
    }
}
