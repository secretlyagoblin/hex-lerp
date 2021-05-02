using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Blerpables
{
    public abstract class Blerpable
    {
        public static Blerpable Blerp(Blerpable a, Blerpable b, Blerpable c, I3dPositionable weight)
        {
            return a.Blerp(b, c, weight);
        }

        protected abstract Blerpable Blerp(Blerpable b, Blerpable c, I3dPositionable weight);
    }
}
