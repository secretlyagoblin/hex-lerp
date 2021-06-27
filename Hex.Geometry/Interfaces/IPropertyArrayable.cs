using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Geometry.Interfaces
{
    public interface IPropertyArrayable
    {
        public object[] ToPropertyValueArray();

        public string[] ToKeys();
    }
}
