using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TMEditorMap.Helpers;
using TMFormat.Enums;
using TMFormat.Formats;

namespace TMEditorMap.Engine
{
    public class MapTile
    {
        SpriteBatch _spriteBatch;

        public MapTile(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void DrawTileBase(int floor_current, int x, int y, float tmpX, float tmpY, bool drawFloor = true)
        {
            int index = MapManager.MapBase.Floors[floor_current][x, y].item.IndexAnimation;
            Vector2 offset = Vector2.Zero;
            int items = 0;

            if (drawFloor)
            {
                if (MapManager.MapBase.Floors[floor_current][x, y].item.Id > 1)
                {
                    _spriteBatch.Draw(MapManager.MapBase.Floors[floor_current][x, y].item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX, tmpY), null, Color.White);
                }
            }

            if (MapManager.MapBase.Floors[floor_current][x, y].items != null)
            {
                foreach (var item in MapManager.MapBase.Floors[floor_current][x, y].items)
                {
                    if (item.isOffset)
                    {
                        offset = new Vector2(-8, -8);
                    }

                    switch ((TypeItem)item.Type)
                    {
                        case TypeItem.Border: // 1 Tile
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                        case TypeItem.Field: // 1 Tile
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                        case TypeItem.Item: // 1 Tile

                            if (!item.isOffset)
                            {
                                _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            }

                            break;
                        case TypeItem.Tree: // 4 Tiles
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                        case TypeItem.Door: // 4 Tiles
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                        case TypeItem.Wall: // 4 Tiles
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                        case TypeItem.Stair: // 4 Tiles
                            _spriteBatch.Draw(item.Sprites[index].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                            break;
                    }

                    items++;
                }
            }
        }

        public void DrawTileTop(int floor_current, int x, int y, float tmpX, float tmpY, bool drawAll = false)
        {
            Vector2 offset = Vector2.Zero;
            int items = 0;

            if (MapManager.MapBase.Floors[floor_current][x, y].item != null)
            {
                if (MapManager.MapBase.Floors[floor_current][x, y].items != null)
                {
                    foreach (var item in MapManager.MapBase.Floors[floor_current][x, y].items)
                    {
                        switch ((TypeItem)item.Type)
                        {
                            case TypeItem.Door: // 4 Tiles
                                if (drawAll)
                                {
                                    _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite1, Utils.GetTileDestine(tmpX, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                                }

                                if (item.Textures[item.IndexAnimation].Texture2 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y].isTop = item.isTop2;

                                if (item.Textures[item.IndexAnimation].Texture3 != null)
                                    MapManager.MapBase.Floors[floor_current][x, y - 1].isTop = item.isTop3;

                                if (item.Textures[item.IndexAnimation].Texture4 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y - 1].isTop = item.isTop4;

                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite2, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite3, Utils.GetTileDestine(tmpX, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite4, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                break;
                            case TypeItem.Wall: // 4 Tiles
                                if (drawAll)
                                {
                                    _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite1, Utils.GetTileDestine(tmpX, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                                }

                                if (item.Textures[item.IndexAnimation].Texture2 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y].isTop = item.isTop2;

                                if (item.Textures[item.IndexAnimation].Texture3 != null)
                                    MapManager.MapBase.Floors[floor_current][x, y - 1].isTop = item.isTop3;

                                if (item.Textures[item.IndexAnimation].Texture4 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y - 1].isTop = item.isTop4;

                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite2, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite3, Utils.GetTileDestine(tmpX, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite4, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                break;
                            case TypeItem.Item: // 1 Tiles
                                if (item.isOffset)
                                {
                                    offset = new Vector2(-8, -8);
                                    _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite1, Utils.GetTileDestine(tmpX + offset.X, tmpY + offset.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                                }
                                break;
                            case TypeItem.Stair: // 4 Tiles
                                if (drawAll)
                                {
                                    _spriteBatch.Draw(item.Textures[0].Texture1.ToTexture2D(), Utils.GetTileDestine(tmpX, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                                }

                                if (item.Textures[item.IndexAnimation].Texture2 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y].isTop = item.isTop2;

                                if (item.Textures[item.IndexAnimation].Texture3 != null)
                                    MapManager.MapBase.Floors[floor_current][x, y - 1].isTop = item.isTop3;

                                if (item.Textures[item.IndexAnimation].Texture4 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y - 1].isTop = item.isTop4;

                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite2, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite3, Utils.GetTileDestine(tmpX, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite4, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                break;
                            case TypeItem.Tree: // 4 Tiles
                                if (drawAll)
                                {
                                    _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite1, Utils.GetTileDestine(tmpX, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                                }

                                if (item.Textures[item.IndexAnimation].Texture2 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y].isTop = item.isTop2;

                                if (item.Textures[item.IndexAnimation].Texture3 != null)
                                    MapManager.MapBase.Floors[floor_current][x, y - 1].isTop = item.isTop3;

                                if (item.Textures[item.IndexAnimation].Texture4 != null)
                                    MapManager.MapBase.Floors[floor_current][x - 1, y - 1].isTop = item.isTop4;

                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite2, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite3, Utils.GetTileDestine(tmpX, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                _spriteBatch.Draw(item.Sprites[item.IndexAnimation].Sprite4, Utils.GetTileDestine(tmpX - TMBaseMap.TileSize, tmpY - TMBaseMap.TileSize), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
                                break;
                        }
                        items++;
                    }
                }
            }
        }

    }
}
