﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry.Interfaces
{
    public interface IHexSetQueryable<T>: IEnumerable<IHex<T>> where T : IHexData<T>
    {
        public IHex<T> this[I3dIndexable index] {get;}

        public IHexSetQueryable<T> Duplicate(IEnumerable<IHex<T>> set);
    }
}
