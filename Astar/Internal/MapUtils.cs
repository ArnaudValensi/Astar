namespace AStar.Internal
{
    internal static class MapUtils
    {
        public static int CoordsToIndex(int x, int y, int sizeX)
        {
            return y * sizeX + x;
        }

        public static Vector2Int IndexToCoords(int index, int sizeX)
        {
            return new Vector2Int(index % sizeX, index / sizeX);
        }
    }
}