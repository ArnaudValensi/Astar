using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tapas;

namespace Astar.Tests
{
    public class TestPathfinding
    {
        [Test]
        public void No_obstacles()
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
                "_______________E";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, grid);

            Assert.Equal(expected, result);
        }

        [Test]
        public void Obstacle_1()
        {
            int sizeX = 11;
            int sizeY = 6;
            int startX = 7;
            int startY = 4;
            int endX = 4;
            int endY = 1;

            var grid = new Grid(sizeX, sizeY);

            grid.GetNode(3, 1).walkable = false;
            grid.GetNode(3, 2).walkable = false;
            grid.GetNode(4, 2).walkable = false;
            grid.GetNode(5, 2).walkable = false;
            grid.GetNode(6, 2).walkable = false;
            grid.GetNode(7, 2).walkable = false;

            var pathfinding = new Pathfinding();
            var path = pathfinding.FindPath(startX, startY, endX, endY, grid);

            string expected =
                "___________\n" +
                "___oE###___\n" +
                "___ooooo#__\n" +
                "_______#___\n" +
                "_______S___\n" +
                "___________";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, grid);

            Assert.Equal(expected, result);
        }

        [Test]
        public void Obstacle_2()
        {
            int sizeX = 7;
            int sizeY = 4;
            int startX = 0;
            int startY = 3;
            int endX = 6;
            int endY = 1;

            var grid = new Grid(sizeX, sizeY);

            grid.GetNode(3, 1).walkable = false;
            grid.GetNode(4, 1).walkable = false;
            grid.GetNode(4, 2).walkable = false;

            var pathfinding = new Pathfinding();
            var path = pathfinding.FindPath(startX, startY, endX, endY, grid);

            string expected =
                "_______\n" +
                "___oo_E\n" +
                "____o#_\n" +
                "S####__";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, grid);

            Assert.Equal(expected, result);
        }

        string MapToString(int sizeX, int sizeY, int startX, int startY, int endX, int endY, List<Node> nodes, Grid grid)
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
                    if (!grid.GetNode(x, y).walkable)
                    {
                        map[y][x] = 'o';
                    }
                }
            }

            foreach (var node in nodes)
            {
                map[node.gridY][node.gridX] = '#';
            }

            map[startY][startX] = 'S';
            map[endY][endX] = 'E';

            return String.Join("\n", map.Select(sb => sb.ToString()));
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            TapasRunner.Create(args)
              .AddTest<TestPathfinding>()
              .Run();
        }
    }
}
