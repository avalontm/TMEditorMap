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


        int _mouseX;
        public int MouseX
        {
            get { return _mouseX; }
            set
            {
                _mouseX = value;
                OnPropertyChanged("MouseX");
            }
        }

        int _mouseY;
        public int MouseY
        {
            get { return _mouseY; }
            set
            {
                _mouseY = value;
                OnPropertyChanged("MouseY");
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

            MouseX = MouseState.X;
            MouseY = MouseState.Y;

            /*
            if (KeyboardState.GetPressedKeys().Length > 0)
            {
                Debug.WriteLine($"[KeyboardState] {KeyboardState.GetPressedKeys()[0].ToString()}");
            }
            */

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

            _spriteBatch.End();
        }
    }
}
