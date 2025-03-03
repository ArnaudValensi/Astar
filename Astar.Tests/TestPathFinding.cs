using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStar.Internal;
using Tapas;

namespace Astar.Tests
{
    public class TestPathFinding
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

            var mapInfo = new DefaultMapInfo(sizeX, sizeY);
            var pathfinding = new PathFinding(mapInfo);
            var path = new List<int>();
            pathfinding.FindPath(startX, startY, endX, endY, path);

            Console.WriteLine($"path null? {path == null}");

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

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, mapInfo);

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

            var mapInfo = new DefaultMapInfo(sizeX, sizeY);

            mapInfo.SetWalkable(3, 1, false);
            mapInfo.SetWalkable(3, 2, false);
            mapInfo.SetWalkable(4, 2, false);
            mapInfo.SetWalkable(5, 2, false);
            mapInfo.SetWalkable(6, 2, false);
            mapInfo.SetWalkable(7, 2, false);

            var pathfinding = new PathFinding(mapInfo);
            var path = new List<int>();
            pathfinding.FindPath(startX, startY, endX, endY, path);

            string expected =
                "___________\n" +
                "___oE###___\n" +
                "___ooooo#__\n" +
                "_______#___\n" +
                "_______S___\n" +
                "___________";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, mapInfo);

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

            var mapInfo = new DefaultMapInfo(sizeX, sizeY);

            mapInfo.SetWalkable(3, 1, false);
            mapInfo.SetWalkable(4, 1, false);
            mapInfo.SetWalkable(4, 2, false);

            var pathfinding = new PathFinding(mapInfo);
            var path = new List<int>();
            pathfinding.FindPath(startX, startY, endX, endY, path);

            string expected =
                "_______\n" +
                "___oo_E\n" +
                "____o#_\n" +
                "S####__";

            string result = MapToString(sizeX, sizeY, startX, startY, endX, endY, path, mapInfo);

            Assert.Equal(expected, result);
        }

        [Test]
        public void No_end()
        {
            int sizeX = 5;
            int sizeY = 1;
            int startX = 0;
            int startY = 0;
            int endX = 4;
            int endY = 0;

            var mapInfo = new DefaultMapInfo(sizeX, sizeY);

            mapInfo.SetWalkable(2, 0, false);

            var pathfinding = new PathFinding(mapInfo);
            var path = new List<int>();
            pathfinding.FindPath(startX, startY, endX, endY, path);

            Assert.Equal(0, path.Count);
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
    }

}
