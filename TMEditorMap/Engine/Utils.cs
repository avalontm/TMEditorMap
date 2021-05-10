using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Formats;

namespace TMEditorMap.Engine
{
    public static class Utils
    {
        public static Rectangle GetTileDestine(float x, float y)
        {
            var sRect = new Rectangle((int)x, (int)y, TMBaseMap.TileSize, TMBaseMap.TileSize);
            return sRect;
        }
        public static Rectangle GetTileDestine()
        {
            var sRect = new Rectangle(0, 0, TMBaseMap.TileSize, TMBaseMap.TileSize);
            return sRect;
        }

        public static Rectangle GetTileDestine(Vector2 pos)
        {
            var sRect = new Rectangle((int)pos.X, (int)pos.Y, TMBaseMap.TileSize, TMBaseMap.TileSize);
            return sRect;
        }

        public static Rectangle GetLightDestine(float x, float y)
        {
            var sRect = new Rectangle((int)x, (int)y, 256, 256);
            return sRect;
        }

        public static Rectangle GetLightDestine(Vector2 pos)
        {
            var sRect = new Rectangle((int)pos.X, (int)pos.Y, 256, 256);
            return sRect;
        }
    }
}
