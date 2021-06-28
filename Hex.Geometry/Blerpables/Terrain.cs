using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex.Geometry.Blerpables
{
    public class Terrain : IHexData<Terrain>
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

        public Terrain Blerp(Terrain b, Terrain c, Vector3 weight)
        {
            if (b is Terrain tb && c is Terrain tc)
            {

                return new Terrain(
                    Vectors.HexMath.Blerp(this.SeaLevel, tb.SeaLevel, tc.SeaLevel, weight),
                    Vectors.HexMath.BlerpChoice(this.Zone, tb.Zone, tc.Zone, weight),
                    Vectors.HexMath.Blerp(this.Edge, tb.Edge, tc.Edge, weight)
                    );
            }
            else
            {
                throw new Exception("Misaligned blerp");
            }
        }

        public object[] ToPropertyValueArray()
        {
            return new object[]
            {
                this.SeaLevel,
                this.Zone,
                this.Edge
            };
        }

        public string[] ToKeys()
        {
            return new string[]
            {
                nameof(SeaLevel),
                nameof(Zone),
                nameof(Edge)
            };
        }
    }
}
