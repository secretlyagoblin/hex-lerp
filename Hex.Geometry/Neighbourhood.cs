using System.Collections;
using System.Collections.Generic;
using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;

namespace Hex.Geometry
{

    public readonly struct Neighbourhood<T> where T : IHexData<T>
    {
        public Hex<T> Center { get; }
        public Hex<T> N0 { get; }
        public Hex<T> N1 { get; }
        public Hex<T> N2 { get; }
        public Hex<T> N3 { get; }
        public Hex<T> N4 { get; }
        public Hex<T> N5 { get; }

        #region Static Vertex Lists

        /// <summary>
        /// The neighbourhood around a hex - currently hardcoded, and shouldn't be changed
        /// </summary>
        public static readonly Int3[] Neighbours = new Int3[]
        {
            new Int3(+1,-1,0),
            new Int3(+1,0,-1),
            new Int3(0,+1,-1),
            new Int3(-1,+1,0),
            new Int3(-1,0,+1),
            new Int3(0,-1,+1),
        };

        private static Dictionary<int, Int3[]> _cachedRosettes = new Dictionary<int, Int3[]>();

        private static Int3[] GenerateRosette(int radius)
        {
            if (_cachedRosettes.ContainsKey(radius))
                return _cachedRosettes[radius];

            var cells = new List<Int3>();

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = System.Math.Max(-radius, -q - radius);
                int r2 = System.Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    cells.Add(new Int3(q, r, -q - r));
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

        public Neighbourhood(Hex<T> center, Hex<T> n0, Hex<T> n1, Hex<T> n2, Hex<T> n3, Hex<T> n4, Hex<T> n5)
        {
            Center = center;
            N0 = n0;
            N1 = n1;
            N2 = n2;
            N3 = n3;
            N4 = n4;
            N5 = n5;
        }

        /// <summary>
        /// Subdivides the grid by one level
        /// </summary>
        /// <returns></returns>
        public Hex<T>[] Subdivide(int scale)
        {

            var nestedCenter = this.Center.NestMultiply(scale);

            var floatingNestedCenter = nestedCenter.GetHexPosition2d();// + this.Center.Index.GetPosition2d();//.AddNoiseOffset(scale-1);

            var largeHexPoints = new I2dPositionable[]
            {
                 this.N0.NestMultiply(scale).GetHexPosition2d(),// + this.N0.Index.GetPosition2d(),//.AddNoiseOffset(scale-1),
                 this.N1.NestMultiply(scale).GetHexPosition2d(),// + this.N1.Index.GetPosition2d(),//.AddNoiseOffset(scale-1),
                 this.N2.NestMultiply(scale).GetHexPosition2d(),// + this.N2.Index.GetPosition2d(),//.AddNoiseOffset(scale-1),
                 this.N3.NestMultiply(scale).GetHexPosition2d(),// + this.N3.Index.GetPosition2d(),//.AddNoiseOffset(scale-1),
                 this.N4.NestMultiply(scale).GetHexPosition2d(),// + this.N4.Index.GetPosition2d(),//.AddNoiseOffset(scale-1),
                 this.N5.NestMultiply(scale).GetHexPosition2d() //+ this.N5.Index.GetPosition2d()//.AddNoiseOffset(scale-1)
            };

            var indexChildren = nestedCenter.GenerateRosetteCircular(scale+1);

            var children = new List<Hex<T>>();

            for (int i = 0; i < indexChildren.Length; i++)
            {
                I3dPositionable weight = new Vector3(0,0,0);
                var index = 0;
                var foundChild = false;

                var actualPosition = indexChildren[i].GetHexPosition2d();

                for (int u = 0; u < _triangleIndexPairs.Length; u += 2)
                {



                    weight = CalculateBarycentricWeight(floatingNestedCenter, largeHexPoints[_triangleIndexPairs[u]], largeHexPoints[_triangleIndexPairs[u + 1]], actualPosition);
                    var testX = weight.XPos;
                    var testY = weight.YPos;
                    var testZ = weight.ZPos;
                
                    if (testX >= 0 && testX <= 1 && testY >= 0 && testY <= 1 && testZ >= 0 && testZ <= 1)
                    {
                        foundChild = true;
                        break;
                    }
                    index++;
                }
                
                if (!foundChild) continue;
                

                    var payload = InterpolateHexPayload(weight, index);

                    children.Add(new Hex<T>(
                        indexChildren[i],
                        payload
                    ));
                

               

                //Debug.DrawLine(nestedCenter.Position3d, indexChildren[i].Position3d, Color.blue, 100f);



            }

            //var childSubset = ResolveEdgeCases(children);

            return children.ToArray();
        }

        /// <summary>
        /// Goes through the neighbours in a hardcoded way and lerps the correct data
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="triangleIndex"></param>
        /// <returns></returns>
        private T InterpolateHexPayload(I3dPositionable weights, int triangleIndex)
        {
            switch (triangleIndex)
            {
                default:
                case 0: return Center.Payload.Blerp(N0.Payload, N1.Payload, weights);
                case 1: return Center.Payload.Blerp(N1.Payload, N2.Payload, weights);
                case 2: return Center.Payload.Blerp(N2.Payload, N3.Payload, weights);
                case 3: return Center.Payload.Blerp(N3.Payload, N4.Payload, weights);
                case 4: return Center.Payload.Blerp(N4.Payload, N5.Payload, weights);
                case 5: return Center.Payload.Blerp(N5.Payload, N0.Payload, weights);
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
        private static I3dPositionable CalculateBarycentricWeight(I2dPositionable vertA, I2dPositionable vertB, I2dPositionable vertC, I2dPositionable test)
        {
            I2dPositionable v0 = vertB - vertA, v1 = vertC - vertA, v2 = test - vertA;
            var d00 = v0.Dot2d(v0);
            var d01 = v0.Dot2d(v1);
            var d11 = v1.Dot2d(v1);
            var d20 = v2.Dot2d(v0);
            var d21 = v2.Dot2d(v1);
            var denom = d00 * d11 - d01 * d01;
            var v = (d11 * d20 - d01 * d21) / denom;
            var w = (d00 * d21 - d01 * d20) / denom;
            var u = 1.0 - v - w;

            return new Vector3(u, v, w);
        }
    }
}