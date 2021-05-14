using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using TMFormat.Formats;

namespace TMEditorMap.Engine
{
    public class MapCore : WpfGame , INotifyPropertyChanged
    {
        #region Propiedades

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static MapCore Instance { private set; get; }
        IGraphicsDeviceService _graphicsDeviceManager;
        WpfKeyboard _keyboard;
        WpfMouse _mouse;
        RenderTarget2D _rendertarget;
        SpriteBatch _spriteBatch;
        Texture2D _pointTexture;
        TMSprite _itemSelect;
        Vector2 _globalpos;

        Vector2 _screenpos;
        public Vector2 ScreenPos
        {
            get { return _screenpos; }
            set
            {
                _screenpos = value;
                OnPropertyChanged("ScreenPos");
            }
        }

        public Vector2 GlobalPos
        {
            get { return _globalpos; }
            set
            {
                _globalpos = value;
                OnPropertyChanged("GlobalPos");
            }
        }

        public TMSprite ItemSelect
        {
            get { return _itemSelect; }
            set
            {
                _itemSelect = value;
                OnPropertyChanged("ItemSelect");
            }
        }

        public IGraphicsDeviceService DeviceManager
        {
            get { return _graphicsDeviceManager; }
            set
            {
                _graphicsDeviceManager = value;
                OnPropertyChanged("DeviceManager");
            }
        }

        public RenderTarget2D RenderTarget
        {
            get { return _rendertarget; }
            set
            {
                _rendertarget = value;
                OnPropertyChanged("RenderTarget");
            }
        }

        MouseState _mouseState;
        public MouseState MouseState
        {
            get { return _mouseState; }
            set
            {
                _mouseState = value;
                OnPropertyChanged("Mouse");
            }
        }

        KeyboardState _previousState;
        KeyboardState _keyboardState;
        public KeyboardState KeyboardState
        {
            get { return _keyboardState; }
            set
            {
                _keyboardState = value;
                OnPropertyChanged("Keyboard");
            }
        }

        #endregion

        public MapCore()
        {
            Instance = this;
            Debug.WriteLine("[MapCore] Instance");
        }

        protected override void Initialize()
        {
            Instance = this;
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);
            
            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            // content loading now possible
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MapManager.Init(_spriteBatch);

        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            MouseState = _mouse.GetState();
            KeyboardState = _keyboard.GetState();

            GlobalPos = new Vector2((MouseState.X/ TMBaseMap.TileSize) +MapManager.Camera.Scroll.X, (MouseState.Y / TMBaseMap.TileSize) + MapManager.Camera.Scroll.Y);
            ScreenPos = new Vector2((MouseState.X / TMBaseMap.TileSize), (MouseState.Y / TMBaseMap.TileSize));

            if (MapManager.MapBase != null)
            {
                if (KeyboardState.IsKeyDown(Keys.OemPlus) && !_previousState.IsKeyDown(Keys.OemPlus) || KeyboardState.IsKeyDown(Keys.Add) && !_previousState.IsKeyDown(Keys.Add))
                {
                    if (MapManager.FloorCurrent > 0)
                    {
                        MapManager.FloorCurrent--;
                    }
                }
                if (KeyboardState.IsKeyDown(Keys.OemMinus) && !_previousState.IsKeyDown(Keys.OemMinus) || KeyboardState.IsKeyDown(Keys.Subtract) && !_previousState.IsKeyDown(Keys.Subtract))
                {
                    if (MapManager.FloorCurrent < MapManager.MapBase.Floors.Count)
                    {
                        MapManager.FloorCurrent++;
                    }
                }
            }

            _previousState = KeyboardState;
        }

        protected override void Draw(GameTime time)
        {
            base.Draw(time);

            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
           
            MapManager.Draw(time);

            DrawTextureSelect(ScreenPos);
            DrawRectangle(ScreenPos, Color.Red, 2);

            _spriteBatch.End();
        }

        void DrawTextureSelect(Vector2 pos)
        {
            if (ItemSelect != null)
            {
                Rectangle rectangle = new Rectangle((int)pos.X * TMBaseMap.TileSize, (int)pos.Y * TMBaseMap.TileSize, TMBaseMap.TileSize, TMBaseMap.TileSize);
                _spriteBatch.Draw(ItemSelect.Sprites[0].Sprite1, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), Color.White * 0.75f);
            }
        }

        void DrawRectangle(Vector2 pos, Color color, int lineWidth)
        {
            Rectangle rectangle = new Rectangle((int)pos.X * TMBaseMap.TileSize, (int)pos.Y* TMBaseMap.TileSize, TMBaseMap.TileSize, TMBaseMap.TileSize);
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        void DrawBox(Vector2 pos, Color color)
        {
            Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y, TMBaseMap.TileSize, TMBaseMap.TileSize);
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, TMBaseMap.TileSize, TMBaseMap.TileSize), color * 0.5f);
        }
    }
}
