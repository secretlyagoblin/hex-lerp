using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry.Interfaces
{
    public interface IHexSetQueryable<T>: IEnumerable<IHexBlerpable<T>> where T : IBlerpable<T>
    {
        public IHexBlerpable<T> this[I3dIndexable index] {get;}
    }
}
