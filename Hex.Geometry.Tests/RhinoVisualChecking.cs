using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hex.Geometry.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CallCorrectIEquatable()
        {
            Assert.IsFalse(new Int3(0, 1, 2).Equals(new Int3(0, 1, 1)));
            Assert.IsFalse(new Int3(0, 1, 1).Equals(new Int3(0, 1, 2)));
            Assert.IsTrue(new Int3(0, 1, 2).Equals(new Int3(0, 1, 2)));

            I3dIndexable a = new Int3(0, 0, 1);
            I3dIndexable b = new Int3(0, 0, 2);

            Assert.IsFalse(a.Equals(b));

            a = new Int3(2, 1, 2);
            b = new Int3(2, 1, 2);

            Assert.IsTrue(a.Equals(b));

            I2dIndexable b2d = new Int2(2, 1);

            Assert.IsTrue(a.Equals(b2d));
        }

        [Test]
        public void DrawRosette()
        {
            var hex = new Int3(0, 0, 0);
            var rosette = hex.GenerateRosetteCircular(33);
            Print(rosette, nameof(DrawRosette));

            Assert.Pass();
        }

        [Test]
        public void MakeIsland()
        {
            var rng = new System.Random(23467243);

            var hex = new Int2(0,0).Get3dHexIndex();

            var rosette = hex
                .GenerateRosetteCircular(5)
                .Select(x => new Hex<Terrain>(x,new Terrain((rng.NextDouble()*3), rng.Next())))
                .ToList();

            rosette.AddRange(hex.GenerateRing(5).Select(x => new Hex<Terrain>(x,new Terrain())));

            var hexGroup = new HexGroup<Terrain>(rosette);
            var subGroup = hexGroup.Subdivide(3);

            Print(subGroup.GetHexes(), nameof(MakeIsland));

            Assert.Pass();
        }

        private void Print(IEnumerable<I3dIndexable> hexes, string name)
        {
            var path = Path.Combine("..", "..", "..", "rhino", "exports", $"{name}.hexindex");

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }            

            var sb = new StringBuilder();

            foreach (var hex in hexes)
            {
                var pos3d = hex;

                sb.AppendLine($"{pos3d}");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private void Print<T>(IEnumerable<IHexBlerpable<T>> hexes, string name) where T : IBlerpable<T>
        {
            var path = Path.Combine("..", "..", "..", "rhino", "exports", $"{name}.hex");

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            var sb = new StringBuilder();

            foreach (var hex in hexes)
            {

                sb.AppendLine($"{hex.GetHexPosition2d()},{hex.Payload}");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}