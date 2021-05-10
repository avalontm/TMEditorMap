using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TMEditorMap.Engine;
using TMFormat.Formats;

namespace TMEditorMap.Models
{
    public class CameraManager
    {
        public Rectangle Screen;
        public Vector2 Scroll;

        public CameraManager()
        {
            Scroll = new Vector2(0, 0);
            ToMove(0,0);
        }

        public void ToMove(int x, int y)
        {
            int _screenWidth = (int)((x+1) + (MapCore.Instance.ActualWidth / TMBaseMap.TileSize));
            int _screenHeight = (int)((y+1) + (MapCore.Instance.ActualHeight / TMBaseMap.TileSize));

            if (MapManager.MapBase != null)
            {
                if (_screenWidth > MapManager.MapBase.mapInfo.Size.X)
                {
                    _screenWidth = (int)MapManager.MapBase.mapInfo.Size.X;
                }

                if (_screenHeight > MapManager.MapBase.mapInfo.Size.Y)
                {
                    _screenHeight = (int)MapManager.MapBase.mapInfo.Size.Y;
                }
            }

            Screen = new Rectangle(x, y, _screenWidth, _screenHeight);
            Scroll = new Vector2(Screen.X * TMBaseMap.TileSize, Screen.Y * TMBaseMap.TileSize);
        }

        public void Update()
        {
            ToMove(Screen.X, Screen.Y);
        }
    }
}
