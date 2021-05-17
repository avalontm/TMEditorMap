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

        public static readonly int FloorDefault = 7;
        public static int FloorCurrent = FloorDefault;

        public static TMBaseMap MapBase;
        public static List<TMSprite> Items;
        public static CameraManager Camera;

        public static bool UseDebug =false;
        public static bool UseAnimtaion = true;

        static MapTile mapTile;
        static int TimeItem = 250;

        #endregion

        public static void Init(SpriteBatch spriteBatch)
        {
            mapTile = new MapTile(spriteBatch);
            Camera = new CameraManager();
            Camera.ToMove(0,0);
        }

        public static void Update(GameTime time)
        {
            if (MapBase != null && MapBase.Floors.Count > 0)
            {
                if (UseAnimtaion)
                {
                    onAnimateFloorCurrent(time);
                }
            }
        }

        public static void Draw(GameTime time) 
        {
            if (MapBase != null && MapBase.Floors.Count > 0)
            {
                if (!isDungeon())
                {
                    for (int z = FloorCurrent; z <= FloorDefault; z++)
                    {
                        onDrawFloor(z);
                    }
                }

                onDrawFloorCurrent();

            }
        }


        static bool isDungeon()
        {
            if (FloorCurrent > FloorDefault)
            {
                return true;
            }
            return false;
        }

        static void onAnimateFloorCurrent(GameTime gameTime)
        {
            // UPDATE TILE LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    if (MapBase.Floors[FloorCurrent][x, y].item != null)
                    {
                        var _item = MapBase.Floors[FloorCurrent][x, y].item;

                        if (_item.isAnimation)
                        {
                            _item.TimeAnimation += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * _item.AniSpeed);

                            if (_item.TimeAnimation > TimeItem)//FPS?
                            {
                                _item.TimeAnimation = 0;
                                _item.IndexAnimation++;

                                if (_item.IndexAnimation == _item.Textures.Count)
                                {
                                    _item.IndexAnimation = 0;
                                }
                            }
                        }
                    }

                    if (MapBase.Floors[FloorCurrent][x, y].items != null)
                    {
                        for (var a = 0; a < MapBase.Floors[FloorCurrent][x, y].items.Count; a++)
                        {
                            var _item = MapBase.Floors[FloorCurrent][x, y].items[a];

                            if (_item.isAnimation)
                            {
                                _item.TimeAnimation += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * _item.AniSpeed);

                                if (_item.TimeAnimation > TimeItem) //FPS?
                                {
                                    _item.TimeAnimation = 0;
                                    _item.IndexAnimation++;

                                    if (_item.IndexAnimation == _item.Sprites.Count)
                                    {
                                        _item.IndexAnimation = 0;
                                    }
                                }
                            }
                        }
                    }

                } //Y
            } //X
        }

        static void onDrawFloorCurrent()
        {
            // DRAW FLOOR LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - (Camera.Scroll.X * TMBaseMap.TileSize));
                    float tmpY = ((y * TMBaseMap.TileSize) - (Camera.Scroll.Y * TMBaseMap.TileSize));

                    if (MapBase.Floors[FloorCurrent][x, y].item != null)
                    {
                        mapTile.DrawTileBase(FloorCurrent, x, y, tmpX, tmpY, Color.White);
                    }
                }
            }

            // DRAW TOP LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - (Camera.Scroll.X * TMBaseMap.TileSize));
                    float tmpY = ((y * TMBaseMap.TileSize) - (Camera.Scroll.Y * TMBaseMap.TileSize));

                    if (MapBase.Floors[FloorCurrent][x, y].item != null)
                    {
                        mapTile.DrawTileTop(FloorCurrent, x, y, tmpX, tmpY, Color.White);
                    }
                }
            }
        }

        static void onDrawFloor(int FloorIndex)
        {
            int _floor = FloorDefault;

            // DRAW FLOOR LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - (Camera.Scroll.X * TMBaseMap.TileSize));
                    float tmpY = ((y * TMBaseMap.TileSize) - (Camera.Scroll.Y * TMBaseMap.TileSize));

                    tmpX += (TMBaseMap.TileSize * (_floor - FloorIndex));
                    tmpY += (TMBaseMap.TileSize * (_floor - FloorIndex));

                    if (MapBase.Floors[FloorIndex][x, y].item != null)
                    {
                        mapTile.DrawTileBase(FloorIndex, x, y, tmpX, tmpY, Color.DarkGray);
                    }
                }
            }

            // DRAW TOP LAYER
            for (int y = Camera.Screen.Y; y < Camera.Screen.Height; y++)
            {
                for (int x = Camera.Screen.X; x < Camera.Screen.Width; x++)
                {
                    //COORDENADAS
                    float tmpX = ((x * TMBaseMap.TileSize) - (Camera.Scroll.X * TMBaseMap.TileSize));
                    float tmpY = ((y * TMBaseMap.TileSize) - (Camera.Scroll.Y * TMBaseMap.TileSize));

                    tmpX += (TMBaseMap.TileSize * (_floor - FloorIndex));
                    tmpY += (TMBaseMap.TileSize * (_floor - FloorIndex));

                    if (MapBase.Floors[FloorIndex][x, y].item != null)
                    {
                        mapTile.DrawTileTop(FloorIndex, x, y, tmpX, tmpY, Color.DarkGray);
                    }
                }
            }
        }
    }
}
