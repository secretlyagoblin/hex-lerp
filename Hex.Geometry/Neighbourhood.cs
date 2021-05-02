using System.Collections;
using System.Collections.Generic;
using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;

namespace Hex.Geometry
{

    public readonly struct Neighbourhood
    {
        public Hex<Blerpable> Center { get; }
        public Hex<Blerpable> N0 { get; }
        public Hex<Blerpable> N1 { get; }
        public Hex<Blerpable> N2 { get; }
        public Hex<Blerpable> N3 { get; }
        public Hex<Blerpable> N4 { get; }
        public Hex<Blerpable> N5 { get; }

        public bool IsBorder { get; }

        #region Static Vertex Lists

        /// <summary>
        /// The neighbourhood around a hex - currently hardcoded, and shouldn't be changed
        /// </summary>
        public static readonly HexIndex3d[] Neighbours = new HexIndex3d[]
        {
            new HexIndex3d(+1,-1,0),
            new HexIndex3d(+1,0,-1),
            new HexIndex3d(0,+1,-1),
            new HexIndex3d(-1,+1,0),
            new HexIndex3d(-1,0,+1),
            new HexIndex3d(0,-1,+1),
        };

        private static Dictionary<int, HexIndex3d[]> _cachedRosettes = new Dictionary<int, HexIndex3d[]>();

        private static HexIndex3d[] GenerateRosette(int radius)
        {
            if (_cachedRosettes.ContainsKey(radius))
                return _cachedRosettes[radius];

            var cells = new List<HexIndex3d>();

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = System.Math.Max(-radius, -q - radius);
                int r2 = System.Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    cells.Add(new HexIndex3d(q, r, -q - r));
                }
            }

            var result = cells.ToArray();

            _cachedRosettes.Add(radius, result);

            return result;
        }


        #endregion

        private static readonly int[] _triangleIndexPairs = new int[]
        {
            0,1,
            1,2,
            2,3,
            3,4,
            4,5,
            5,0
        };

        /// <summary>
        /// Subdivides the grid by one level
        /// </summary>
        /// <returns></returns>
        public Hex<T>[] Subdivide(int scale)
        {
            if (this.IsBorder && Hex.IsInvalid(this.Center))                  
                    return new Hex[0];

            var nestedCenter = this.Center.Index.NestMultiply(scale);

            var floatingNestedCenter = nestedCenter.GetPosition2d()  + this.Center.Index.GetPosition2d().AddNoiseOffset(scale-1);

            var largeHexPoints = new Vector2[]
            {
                 this.N0.Index.NestMultiply(scale).GetPosition2d() + this.N0.Index.GetPosition2d().AddNoiseOffset(scale-1),
                 this.N1.Index.NestMultiply(scale).GetPosition2d() + this.N1.Index.GetPosition2d().AddNoiseOffset(scale-1),
                 this.N2.Index.NestMultiply(scale).GetPosition2d() + this.N2.Index.GetPosition2d().AddNoiseOffset(scale-1),
                 this.N3.Index.NestMultiply(scale).GetPosition2d() + this.N3.Index.GetPosition2d().AddNoiseOffset(scale-1),
                 this.N4.Index.NestMultiply(scale).GetPosition2d() + this.N4.Index.GetPosition2d().AddNoiseOffset(scale-1),
                 this.N5.Index.NestMultiply(scale).GetPosition2d() + this.N5.Index.GetPosition2d().AddNoiseOffset(scale-1)
            };

            //var barycenters
            //
            //for (int i = 0; i < 5; i++)
            //{
            //    (largeHexPoints[i] + floatingNestedCenter + largeHexPoints[i+1])/3
            //}

            HexIndex3d[] indexChildren;

            if (this.IsBorder)
            {
                indexChildren = nestedCenter.GenerateRosetteLinear(scale);
            }
            else
            {
                indexChildren = nestedCenter.GenerateRosetteCircular(scale+1);
            }

            //var generationProducedCells = true;

            //var radius = 1;

            //while (generationProducedCells)
            //{
            //
            //}




            var children = new HexIndex3d[indexChildren.Length];

            for (int i = 0; i < indexChildren.Length; i++)
            {
                var weight = Vector3.zero;
                var index = 0;
                var foundChild = false;

                //for (int u = 0; u < _triangleIndexPairs.Length; u += 2)
                //{
                //    weight = CalculateBarycentricWeight(center, largeHexPoints[_triangleIndexPairs[u]], largeHexPoints[_triangleIndexPairs[u + 1]], actualPosition);
                //    var testX = weight.x;
                //    var testY = weight.y;
                //    var testZ = weight.z;
                //
                //    if (testX >= 0 && testX <= 1 && testY >= 0 && testY <= 1 && testZ >= 0 && testZ <= 1)
                //    {
                //        foundChild = true;
                //        break;
                //    }
                //    index++;
                //}
                //
                //if (!foundChild)
                //{
                //    Debug.LogError($"No containing barycenter detected at {this.Center.Index} - inner hex not contained by outer hex. Using weight {weight}.");
                //}

                var isBorder = this.InterpolateIsBorder(weight, index);

                var payload = isBorder ? this.Center.Payload : this.InterpolateHexPayload(weight, index);

                //Debug.DrawLine(nestedCenter.Position3d, indexChildren[i].Position3d, Color.blue, 100f);


                children[i] = new Hex(
                    indexChildren[i],
                    payload,
                    isBorder,
                    ""//$"{Center.Index}\n{N0.Index}\n{N1.Index}\n{N2.Index}\n{N3.Index}\n{N4.Index}\n{N5.Index}"
                    );
            }

            //var childSubset = ResolveEdgeCases(children);

            return children;
        }

        /// <summary>
        /// Goes through the neighbours in a hardcoded way and lerps the correct data
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="triangleIndex"></param>
        /// <returns></returns>
        private bool InterpolateIsBorder(Vector3 weights, int triangleIndex)
        {
            switch (triangleIndex)
            {
                default:
                case 0: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N0.IsBorder, N1.IsBorder, weights);
                case 1: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N1.IsBorder, N2.IsBorder, weights);
                case 2: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N2.IsBorder, N3.IsBorder, weights);
                case 3: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N3.IsBorder, N4.IsBorder, weights);
                case 4: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N4.IsBorder, N5.IsBorder, weights);
                case 5: return InterpolationHelpers.Blerp<bool>(Center.IsBorder, N5.IsBorder, N0.IsBorder, weights);
            }
        }

        /// <summary>
        /// Goes through the neighbours in a hardcoded way and lerps the correct data
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="triangleIndex"></param>
        /// <returns></returns>
        private HexPayload InterpolateHexPayload(Vector3 weights, int triangleIndex)
        {
            switch (triangleIndex)
            {
                default:
                case 0: return HexPayload.Blerp(Center, N0, N1, weights);
                case 1: return HexPayload.Blerp(Center, N1, N2, weights);
                case 2: return HexPayload.Blerp(Center, N2, N3, weights);
                case 3: return HexPayload.Blerp(Center, N3, N4, weights);
                case 4: return HexPayload.Blerp(Center, N4, N5, weights);
                case 5: return HexPayload.Blerp(Center, N5, N0, weights);
            }
        }

        /// <summary>
        /// Calculates the barycentric weight of the inner triangles.
        /// </summary>
        /// <param name="vertA"></param>
        /// <param name="vertB"></param>
        /// <param name="vertC"></param>
        /// <param name="test"></param>
        /// <returns></returns>
        private static Vector3 CalculateBarycentricWeight(Vector2 vertA, Vector2 vertB, Vector2 vertC, Vector2 test)
        {
            Vector2 v0 = vertB - vertA, v1 = vertC - vertA, v2 = test - vertA;
            float d00 = Vector2.Dot(v0, v0);
            float d01 = Vector2.Dot(v0, v1);
            float d11 = Vector2.Dot(v1, v1);
            float d20 = Vector2.Dot(v2, v0);
            float d21 = Vector2.Dot(v2, v1);
            float denom = d00 * d11 - d01 * d01;
            var v = (d11 * d20 - d01 * d21) / denom;
            var w = (d00 * d21 - d01 * d20) / denom;
            var u = 1.0f - v - w;

            return new Vector3(u, v, w);
        }
    }
}