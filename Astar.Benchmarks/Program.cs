using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Astar.Benchmarks
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]    
    public class AstarBenchmark
    {
        DefaultMapInfo mapInfo;
        PathFinding pathfinding;
        int sizeX = 256;
        int sizeY = 256;
        int startX = 0;
        int startY = 0;
        int endX;
        int endY;
        List<int> path;

        public AstarBenchmark()
        {
            endX = sizeX - 1;
            endY = sizeY - 1;
            
            mapInfo = new DefaultMapInfo(sizeX, sizeY);
            path = new List<int>();

            int threeQuarter = sizeX / 4 * 3;
            for (int y = 1; y < sizeY; y++)
            {
                mapInfo.SetWalkable(threeQuarter, y, false);
            }
            
            pathfinding = new PathFinding(mapInfo);
        }

        [Benchmark]
        public List<int> SystemDiagnostic()
        {
            pathfinding.FindPath(startX, startY, endX, endY, path);

            return path;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<AstarBenchmark>();
        }
    }
}
