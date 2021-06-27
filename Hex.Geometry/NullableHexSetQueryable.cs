using Hex.Geometry.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry
{
    public class NullableHexSetQueryable<T> : IHexSetQueryable<T> where T : IHexData<T>
    {
        private readonly T _defaultValue;

        private readonly Dictionary<I3dIndexable, IHex<T>> _hexes = new();

        public NullableHexSetQueryable(IEnumerable<IHex<T>> hexes, T defaultValue)
        {
            _defaultValue = defaultValue;

            foreach (var hex in hexes)
            {
                _hexes[hex] = hex;
            }
        }

        public IHex<T> this[I3dIndexable index] => 
            _hexes.TryGetValue(index, out var value) ? 
            value : 
            new Hex<T>(index,_defaultValue);

        public IHexSetQueryable<T> Duplicate(IEnumerable<IHex<T>> set)
        {
            return new NullableHexSetQueryable<T>(set, _defaultValue);
        }

        public IEnumerator<IHex<T>> GetEnumerator() => _hexes.Select(x => x.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _hexes.Select(x => x.Value).GetEnumerator();
    }
}
