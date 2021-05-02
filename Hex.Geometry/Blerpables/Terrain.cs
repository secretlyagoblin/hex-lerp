using Hex.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Blerpables
{
    public class Terrain : Blerpable
    {
        public Terrain() { }

        public Terrain(double seaLevel, int zone, double edge = 1)
        {
            SeaLevel = seaLevel;
            Zone = zone;
            Edge = edge;

        }

        public double SeaLevel { get; } = -5;
        public int Zone { get; } = -1;
        public double Edge { get; } = 0;

        public override string ToString()
        {
            return $"{SeaLevel},{Zone},{Edge}";
        }

        protected override Blerpable Blerp(Blerpable b, Blerpable c, I3dPositionable weight)
        {
            if (b is Terrain tb && c is Terrain tc)
            {

                return new Terrain(
                    Vectors.Math.Blerp(this.SeaLevel, tb.SeaLevel, tc.SeaLevel, weight),
                    Vectors.Math.BlerpChoice(this.Zone, tb.Zone, tc.Zone, weight),
                    Vectors.Math.Blerp(this.Edge, tb.Edge, tc.Edge, weight)
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
