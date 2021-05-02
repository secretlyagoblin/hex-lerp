using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Blerpables
{
    public class Terrain : Blerpable
    {
        public Terrain() { }

        public Terrain(double seaLevel)
        {
            SeaLevel = seaLevel;
        }

        public double SeaLevel { get; } = -5;

        protected override Blerpable Blerp(Blerpable b, Blerpable c, I3dPositionable weight)
        {
            if (b is Terrain tb && c is Terrain tc)
            {

                return new Terrain(Vectors.Math.Blerp(this.SeaLevel, tb.SeaLevel, tc.SeaLevel, weight));
            }
            else
            {
                throw new Exception("Misaligned blerp");
            }
        }
    }
}
