using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using TMEditorMap.Models;
using TMFormat.Enums;
using TMFormat.Formats;

namespace TMEditorMap.Engine
{
    public static class MapManager
    {
        #region Propiedades

        public static int FloorDefault = 7;
        public static int FloorCurrent = FloorDefault;

        public static TMBaseMap MapBase;
        public static List<TMSprite> Items;
        public static CameraManager Camera;
        static MapTile mapTile;

        #endregion

        public static void Init(SpriteBatch spriteBatch)
        {
            mapTile = new MapTile(spriteBatch);
            Camera = new CameraManager();
            Camera.ToMove(0,0);
        }

        public static void Draw(GameTime time) 
        {
            if (MapBase != null)
            {
                onDrawFloorCurrent();
            }
        }

        static void onDrawFloorCurrent()
        {
            // DRAW FLOOR LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Scroll.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Scroll.Y);

                    if (MapBase.Floors[FloorCurrent][x, y].item != null)
                    {
                        mapTile.DrawTileBase(FloorCurrent, x, y, tmpX, tmpY);
                    }
                }
            }

            // DRAW TOP LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - Camera.Scroll.X);
                    float tmpY = ((y * TMBaseMap.TileSize) - Camera.Scroll.Y);

                    if (MapBase.Floors[FloorCurrent][x, y].item != null)
                    {
                        mapTile.DrawTileTop(FloorCurrent, x, y, tmpX, tmpY);
                    }
                }
            }
        }
    }
}
