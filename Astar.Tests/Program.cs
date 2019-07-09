using Tapas;

namespace Astar.Tests
{
    public class Test_assert_equal
    {
        [Test]
        public void Equal()
        {
            var astar = new AstarRunner();
            Assert.Equal(true, astar != null);
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
