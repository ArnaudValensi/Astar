using Microsoft.Xna.Framework;
using Tapas;

namespace Astar.Tests
{
    public class Test_assert_equal
    {
        [Test]
        public void Equal()
        {
            var pathfinding = new Pathfinding();
            var result = pathfinding.FindPath(Vector2.Zero, Vector2.Zero);

            Assert.Equal(true, result == null);
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            TapasRunner.Create(args)
              .AddTest<Test_assert_equal>()
              .Run();
        }
    }
}
