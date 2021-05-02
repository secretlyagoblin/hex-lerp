using Hex.Geometry.Vectors;
using NUnit.Framework;
using System.IO;
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

        private void Print(HexIndex3d[] hexes, string name)
        {
            var path = Path.Combine("..", "..", "..", "rhino", "exports", $"{name}.hex");

            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
                File.Create(path);
            }

            

            var sb = new StringBuilder();

            foreach (var hex in hexes)
            {
                var pos3d = hex.ToPosition3d();

                sb.AppendLine($"{pos3d}");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}