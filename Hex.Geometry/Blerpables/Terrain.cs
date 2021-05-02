using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Blerpables
{
    public class Terrain : Blerpable
    {
        public Terrain() { }

        public Terrain(double seaLevel, int zone)
        {
            SeaLevel = seaLevel;
            Zone = zone;
        }

        public double SeaLevel { get; } = -5;
        public int Zone { get; } = -1;

        public override string ToString()
        {
            return $"{SeaLevel},{Zone}";
        }

        protected override Blerpable Blerp(Blerpable b, Blerpable c, I3dPositionable weight)
        {
            if (b is Terrain tb && c is Terrain tc)
            {

                return new Terrain(
                    Vectors.Math.Blerp(this.SeaLevel, tb.SeaLevel, tc.SeaLevel, weight),
                    Vectors.Math.BlerpChoice(this.Zone, tb.Zone, tc.Zone, weight)
                    );
            }
            else
            {
                throw new Exception("Misaligned blerp");
            }
        }

        protected override Blerpable GetDefault()
        {
            return new Terrain();
        }
    }
}
