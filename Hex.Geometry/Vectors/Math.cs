using System.Collections;
using System.Collections.Generic;

namespace Hex.Geometry.Vectors
{

    public static class Math
    {
        public static readonly double ScaleY = System.Math.Sqrt(3.0) * 0.5;
        public static readonly double HalfHex = 0.5f / System.Math.Cos(System.Math.PI / 180.0 * 30.0);

        public static Vector3 ToPosition3d(this HexIndex3d pos)
        {
            var twoD = pos.Get2dIndex().GetPosition2d();
            return new Vector3(twoD.X, twoD.Y, 0);            
        }

        public static Vector2 GetPosition2d(this HexIndex2d index2d)
        {
            var isOdd = index2d.YIndex % 2 != 0;

            return new Vector2(
                index2d.XIndex - (isOdd ? 0 : 0.5f),
                index2d.YIndex * ScaleY);
        }

        public static HexIndex3d Get3dIndex(this HexIndex2d index2d)
        {
            var x = index2d.XIndex - (index2d.YIndex - (index2d.YIndex & 1)) / 2;
            var z = index2d.YIndex;
            var y = -x - z;
            return new HexIndex3d(x, y, z);
        }

        public static HexIndex2d Get2dIndex(this HexIndex3d index3d)
        {
            var col = index3d.XIndex + (index3d.ZIndex - (index3d.ZIndex & 1)) / 2;
            var row = index3d.ZIndex;
            return new HexIndex2d(col, row);
        }

        /// <summary>
        /// Rotate a given hex around 0,0,0 by 60 degrees
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static HexIndex3d Rotate60(this HexIndex3d index)
        {
            return new HexIndex3d(-index.YIndex, -index.ZIndex, -index.XIndex);
        }

        public static HexIndex3d NestMultiply(this HexIndex3d index, int amount)
        {
            return index * (amount + 1) + (index.Rotate60() * amount);
        }

        public static HexIndex3d[] GenerateRosetteLinear(this HexIndex3d index, int radius)
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

            var output = new HexIndex3d[count];

            count = 0;

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = System.Math.Max(-radius, -q - radius);
                int r2 = System.Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    var vec = new HexIndex3d(q, r, -q - r) + index;
                    output[count] = vec;
                    count++;
                }

            }

            return output;
        }

        private static readonly HexIndex3d[] _directions = new HexIndex3d[]
        {
            new HexIndex3d(1,-1,0),
            new HexIndex3d(0,-1,1),
            new HexIndex3d(-1,0,1),
            new HexIndex3d(-1,1,0),
            new HexIndex3d(0,1,-1),
            new HexIndex3d(1,0,-1)
        };

        public static HexIndex3d[] GenerateRosetteCircular(this HexIndex3d index, int radius)
        {
            var results = new List<HexIndex3d>() { index };

            for (int i = 1; i < radius; i++)
            {
                results.AddRange(index.GenerateRing(i));
            }

            return results.ToArray();
        }

        public static HexIndex3d[] GenerateRing(this HexIndex3d index, int radius)
        {
            var resultsCount = radius == 0 ? 1 : (radius) * 6;

            var results = new HexIndex3d[resultsCount];

            var currentPos = index + (_directions[4] * radius);
            //var lastPos = currentPos;
            currentPos = currentPos + (_directions[0] * ((int)System.Math.Floor(radius * 0.5)));

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

                    if (ringStart == currentPos)
                        return results;
                }

                i = i < 5 ? (i + 1) : 0;
            }
        }

        public static double Blerp(float a, float b, float c, Vector3 weight)
        {
            return a * weight.X + b * weight.Y + c * weight.Z;
        }

        public static Vector2 Blerp(Vector2 a, Vector2 b, Vector2 c, Vector3 weight)
        {
            var x = a.X * weight.X + b.X * weight.Y + c.X * weight.Z;
            var y = a.Y * weight.X + b.Y * weight.Y + c.Y * weight.Z;

            return new Vector2(x, y);
        }

        public static Vector3 Blerp(Vector3 a, Vector3 b, Vector3 c, Vector3 weight)
        {
            var x = a.X * weight.X + b.X * weight.Y + c.X * weight.Z;
            var y = a.Y * weight.X + b.Y * weight.Y + c.Y * weight.Z;
            var z = a.Z * weight.X + b.Z * weight.Y + c.Z * weight.Z;

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
            if (weight.X >= weight.Y && weight.X >= weight.Z)
            {
                return a;
            }
            else if (weight.Y >= weight.Z && weight.Y >= weight.X)
            {
                return b;
            }
            else
            {
                return c;
            }
        }


    }
}