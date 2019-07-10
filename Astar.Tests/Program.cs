using Tapas;

namespace Astar.Tests
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            TapasRunner.Create(args)
//                .AddTest<TestPathFinding>()
                .AddTest<TestHeap>()
                .Run();
        }
    }
}
