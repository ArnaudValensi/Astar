using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tapas;

namespace Astar.Tests
{
    public class Test_assert_equal
    {
        [Test]
        public void Equal()
        {
            int sizeX = 16;
            int sizeY = 16;
            int startX = 0;
            int startY = 0;
            int endX = sizeX - 1;
            int endY = sizeY - 1;

            var grid = new Grid(sizeX, sizeY);
            var pathfinding = new Pathfinding();
            var path = pathfinding.FindPath(startX, startY, endX, endY, grid);

            string expected =
                "S_______________\n" +
                "_#______________\n" +
                "__#_____________\n" +
                "___#____________\n" +
                "____#___________\n" +
                "_____#__________\n" +
                "______#_________\n" +
                "_______#________\n" +
                "________#_______\n" +
                "_________#______\n" +
                "__________#_____\n" +
                "___________#____\n" +
                "____________#___\n" +
                "_____________#__\n" +
                "______________#_\n" +
                "_______________#";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path);

            Assert.Equal(expected, result);
        }

        string MapToString(int sizeX, int sizeY, int startX, int startY, int endX, int endY, List<Node> nodes)
        {
            StringBuilder[] map = new StringBuilder[sizeY];

            for (int i = 0; i < sizeY; i++)
            {
                map[i] = new StringBuilder(new String('_', sizeY));
            }

            map[startY][startX] = 'S';
            map[endY][endX] = 'E';

            foreach (var node in nodes)
            {
                map[node.gridY][node.gridX] = '#';
            }

            return String.Join("\n", map.Select(sb => sb.ToString()));
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
