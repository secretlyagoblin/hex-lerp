using Hex.Geometry.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry
{
    public class NullableHexSetQueryable<T> : IHexSetQueryable<T> where T : IBlerpable<T>
    {
        private readonly T _defaultValue;

        private readonly Dictionary<I3dIndexable, IHexBlerpable<T>> _hexes = new();

        public NullableHexSetQueryable(IEnumerable<IHexBlerpable<T>> hexes, T defaultValue)
        {
            _defaultValue = defaultValue;

            foreach (var hex in hexes)
            {
                _hexes[hex] = hex;
            }
        }

        public IHexBlerpable<T> this[I3dIndexable index] => 
            _hexes.TryGetValue(index, out var value) ? 
            value : 
            new Hex<T>(index,_defaultValue);

        public IEnumerator<IHexBlerpable<T>> GetEnumerator() => _hexes.Select(x => x.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _hexes.Select(x => x.Value).GetEnumerator();
    }
}
