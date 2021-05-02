using Hex.Geometry.Blerpables;
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
        public void DrawRosette()
        {
            var hex = new HexIndex3d(0, 0, 0);
            var rosette = hex.GenerateRosetteCircular(33);
            Print(rosette, nameof(DrawRosette));

            Assert.Pass();
        }

        [Test]
        public void MakeIsland()
        {
            var rng = new System.Random(23467243);

            var hex = new HexIndex2d(0,0).Get3dIndex();
            var rosette = hex
                .GenerateRosetteCircular(5)
                .Select(x => new Hex<Blerpable>(x,new Terrain((rng.NextDouble()*3), rng.Next())))
                .ToList();

            rosette.AddRange(hex.GenerateRing(5).Select(x => new Hex<Blerpable>(x, Blerpable.Default(rosette[0].Payload))));

            var hexGroup = new HexGroup(rosette);
            var subGroup = hexGroup.Subdivide(3);

            Print(subGroup.GetHexes(), nameof(MakeIsland));

            Assert.Pass();
        }

        private void Print(HexIndex3d[] hexes, string name)
        {
            var path = Path.Combine("..", "..", "..", "rhino", "exports", $"{name}.hexindex");

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            

            var sb = new StringBuilder();

            foreach (var hex in hexes)
            {
                var pos3d = hex.ToPosition3d();

                sb.AppendLine($"{pos3d}");
            }

            File.WriteAllText(path, sb.ToString());
        }

        private void Print(List<Hex<Blerpable>> hexes, string name)
        {
            var path = Path.Combine("..", "..", "..", "rhino", "exports", $"{name}.hex");

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            var sb = new StringBuilder();

            foreach (var hex in hexes)
            {
                var pos3d = hex.Index.ToPosition3d();

                sb.AppendLine($"{pos3d},{hex.Payload}");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}