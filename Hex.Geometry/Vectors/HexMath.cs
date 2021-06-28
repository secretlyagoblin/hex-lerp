using Hex.Geometry.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Hex.Geometry.Vectors
{

    public static class HexMath
    {
        public static readonly double ScaleY = System.Math.Sqrt(3.0) * 0.5;
        public static readonly double HalfHex = 0.5f / System.Math.Cos(System.Math.PI / 180.0 * 30.0);
        public const double TOLERANCE = 0.0000001;

        public static bool EqualsWithinTolerance(this double a, double b) => System.Math.Abs(a - b) < TOLERANCE;

        public static Vector2 GetHexPosition2d(this Int3 index3d) => index3d.Get2dHexIndex().GetHexPosition2d();

        public static Vector2 GetHexPosition2d(this Int2 index2d)
        {
            var isOdd = index2d.YIndex % 2 != 0;

            return new Vector2(
                index2d.XIndex - (isOdd ? 0 : 0.5f),
                index2d.YIndex * ScaleY);
        }

        public static Int3 Get3dHexIndex(this Int2 index2d)
        {
            var x = index2d.XIndex - (index2d.YIndex - (index2d.YIndex & 1)) / 2;
            var z = index2d.YIndex;
            var y = -x - z;
            return new Int3(x, y, z);
        }

        public static Int2 Get2dHexIndex(this Int3 index3d)
        {
            var col = index3d.XIndex + (index3d.ZIndex - (index3d.ZIndex & 1)) / 2;
            var row = index3d.ZIndex;
            return new Int2(col, row);
        }

        /// <summary>
        /// Rotate a given hex around 0,0,0 by 60 degrees
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Int3 RotateHex60(this I3dIndexable index)
        {
            return new Int3(-index.YIndex, -index.ZIndex, -index.XIndex);
        }

        public static Int3 NestMultiply(this I3dIndexable index, int amount)
        {
            return index.Index3d * (amount + 1) + (index.RotateHex60() * amount);
        }

        public static Int3[] GenerateRosetteLinear(this Int3 index, int radius)
        {
            //calculate rosette size without any GC :(

            var count = 0;

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = System.Math.Max(-radius, -q - radius);
                int r2 = System.Math.Min(radius, -q + radius);

                for (int r = r1; r <= r2; r++)
                {
                    count++;
                }
            }

            //Do the whole thing again this time making an array            

            var output = new Int3[count];

            count = 0;

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = System.Math.Max(-radius, -q - radius);
                int r2 = System.Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    var vec = new Int3(q, r, -q - r) + index;
                    output[count] = vec;
                    count++;
                }

            }

            return output;
        }

        private static readonly Int3[] _directions = new Int3[]
        {
            new Int3(1,-1,0),
            new Int3(0,-1,1),
            new Int3(-1,0,1),
            new Int3(-1,1,0),
            new Int3(0,1,-1),
            new Int3(1,0,-1)
        };

        public static Int3[] GenerateRosetteCircular(this Int3 index, int radius)
        {
            var results = new List<Int3>() { index };

            for (int i = 1; i < radius; i++)
            {
                results.AddRange(index.GenerateRing(i));
            }

            return results.ToArray();
        }

        public static Int3[] GenerateRing(this Int3 index, int radius)
        {
            var resultsCount = radius == 0 ? 1 : (radius) * 6;

            var results = new Int3[resultsCount];

            var currentPos = index + (_directions[4] * radius);
            //var lastPos = currentPos;
            currentPos += (_directions[0] * ((int)System.Math.Floor(radius * 0.5)));

            var ringStart = currentPos;

            var i = 0;
            var count = 0;

            while (true)
            {
                for (int j = (count == 0 ? (int)System.Math.Floor(radius * 0.5f) : 0); j < radius; j++)
                {

                    //Debug.Log($"Added Cell {currentPos}");
                    results[count] = (currentPos);
                    currentPos += _directions[i];
                    //Debug.DrawLine(lastPos.Position3d, currentPos.Position3d, Color.green * 0.5f, 100f);
                    //lastPos = currentPos;
                    count++;

                    if (ringStart.Equals(currentPos))
                        return results;
                }

                i = i < 5 ? (i + 1) : 0;
            }
        }

        public static double Blerp(double a, double b, double c, Vector3 weight)
        {
            return a * weight.XPos + b * weight.YPos + c * weight.ZPos;
        }

        public static Vector2 Blerp(I2dPositionable a, I2dPositionable b, I2dPositionable c, Vector3 weight)
        {
            var x = a.XPos * weight.XPos + b.XPos * weight.YPos + c.XPos * weight.ZPos;
            var y = a.YPos * weight.XPos + b.YPos * weight.YPos + c.YPos * weight.ZPos;

            return new Vector2(x, y);
        }

        public static Vector3 Blerp(I3dPositionable a, I3dPositionable b, I3dPositionable c, Vector3 weight)
        {
            var x = a.XPos * weight.XPos + b.XPos * weight.YPos + c.XPos * weight.ZPos;
            var y = a.YPos * weight.XPos + b.YPos * weight.YPos + c.YPos * weight.ZPos;
            var z = a.ZPos * weight.XPos + b.ZPos * weight.YPos + c.ZPos * weight.ZPos;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Choose either A, B or C based on the weight.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static T BlerpChoice<T>(T a, T b, T c, Vector3 weight)
        {
            if (weight.XPos >= weight.YPos && weight.XPos >= weight.ZPos)
            {
                return a;
            }
            else if (weight.YPos >= weight.ZPos && weight.YPos >= weight.XPos)
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        public static int DistanceTo(this I3dIndexable a, I3dIndexable b)
        {
            return (System.Math.Abs(a.XIndex - b.XIndex) + System.Math.Abs(a.YIndex - b.YIndex) + System.Math.Abs(a.ZIndex - b.ZIndex)) / 2;
        }

        public static double Dot2d(this Vector2 a, Vector2 b)
        {
            return (a.XPos * b.XPos) + (a.YPos * b.YPos);
        }


    }
}