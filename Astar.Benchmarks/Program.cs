using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BigSeed.Math;
using BigSeed.Utils;

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
        
        public AstarBenchmark()
        {
            endX = sizeX - 1;
            endY = sizeY - 1;
            
            mapInfo = new DefaultMapInfo(sizeX, sizeY);

            int threeQuarter = sizeX / 4 * 3;
            for (int y = 1; y < sizeY; y++)
            {
                mapInfo.SetWalkable(threeQuarter, y, false);
            }
            
            pathfinding = new PathFinding(mapInfo);
        }
        
        string MapToString(int sizeX, int sizeY, int startX, int startY, int endX, int endY, List<int> cellIndexes, DefaultMapInfo pathMapLayer)
        {
            StringBuilder[] map = new StringBuilder[sizeY];

            for (int i = 0; i < sizeY; i++)
            {
                map[i] = new StringBuilder(new String('_', sizeX));
            }

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (!pathMapLayer.IsWalkable(x, y))
                    {
                        map[y][x] = 'o';
                    }
                }
            }

            foreach (var cellIndexe in cellIndexes)
            {
                Vector2Int position = MapUtils.IndexToCoords(cellIndexe, sizeX);

                map[position.Y][position.X] = '#';
            }

            map[startY][startX] = 'S';
            map[endY][endX] = 'E';

            return String.Join("\n", map.Select(sb => sb.ToString()));
        }

        [Benchmark]
        public List<int> SystemDiagnostic()
        {
            var path = pathfinding.FindPath(startX, startY, endX, endY);
            
//            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, mapInfo);
//            Console.WriteLine(result);

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
