using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TMEditorMap.Engine;
using TMEditorMap.Helpers;
using TMEditorMap.Models;
using TMFormat;
using TMFormat.Enums;
using TMFormat.Formats;
using TMFormat.Helpers;

namespace TMEditorMap
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Propiedades

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        static string root = System.IO.Directory.GetCurrentDirectory();

        ObservableCollection<TMSprite> _sprites;

        public ObservableCollection<TMSprite> sprites
        {
            get { return _sprites; }
            set
            {
                _sprites = value;
                OnPropertyChanged("sprites");
            }
        }

        ObservableCollection<GroupSprites> _groups;

        public ObservableCollection<GroupSprites> groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged("groups");
            }
        }

        

        int _groupIndex;
        public int GroupIndex
        {
            get { return _groupIndex; }
            set
            {
                _groupIndex = value;
                OnPropertyChanged("GroupIndex");
            }
        }

        string _fileMap;

        public string FileMap
        {
            get { return _fileMap; }
            set
            {
                _fileMap = value;
                OnPropertyChanged("FileMap");
            }
        }

        TMSprite _itemSelect;

        public TMSprite ItemSelect
        {
            get { return _itemSelect; }
            set
            {
                _itemSelect = value;
                OnPropertyChanged("ItemSelect");
            }
        }

        Point _mouse = new Point();

        public Point Mouse
        {
            get { return _mouse; }
            set
            {
                _mouse = value;
                OnPropertyChanged("Mouse");
            }
        }

        bool _isLoaded;

        public bool isLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                OnPropertyChanged("isLoaded");
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            sprites = new ObservableCollection<TMSprite>();
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            TMInstance.Init(false, true);
            onLoadItems();
            isLoaded = true;
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MapManager.Camera != null)
            {
                MapManager.Camera.Update();
            }
        }

        void onNew(object sender, RoutedEventArgs e)
        {

        }

        void onOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TMap files (*.tmap)|*.tmap;*.abomap";

            if (openFileDialog.ShowDialog() == true)
            {
                FileMap = openFileDialog.FileName;

                if (!File.Exists(FileMap))
                {
                    return;
                }

                MapManager.MapBase = new TMBaseMap(MapManager.Items);

                bool isMapLoaded = MapManager.MapBase.Load(FileMap);

                if (!isMapLoaded)
                {
                    MessageBox.Show(this, "No se pudo cargar el archivo.\nFormato desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                onLoadMap();
            }
        }

        void onSave(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileMap))
            {
                onSaveMap();
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TMap files (*.tmap)|*.tmap";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileMap = saveFileDialog.FileName;

                if (!File.Exists(FileMap))
                {
                    return;
                }

                onSaveMap();
            }
        }

        void onSaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TMap files (*.tmap)|*.tmap";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileMap = saveFileDialog.FileName;

                if (!File.Exists(FileMap))
                {
                    return;
                }

                onSaveMap();
            }
        }

        void onSaveMap()
        {
            bool result = MapManager.MapBase.Save(FileMap);

            if (result)
            {
                MessageBox.Show(this, "Se ha guardado el archivo.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "No se pudo guardar el archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void onExit(object sender, RoutedEventArgs e)
        {

        }

        async void onLoadItems()
        {
            string dataDir = Path.Combine(root, "data");

            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            MapManager.Items = TMItem.Load(Path.Combine(root, "data", "items.dat")).ToSprites();

            gridItems.barItems.Minimum = 0;
            gridItems.barItems.Maximum = MapManager.Items.Count;

            gridItems.Visibility = Visibility.Visible;
            await Task.Delay(1);

            int index = 0;

            groups = new ObservableCollection<GroupSprites>();

            for (int i = 0; i < Enum.GetNames(typeof(TypeItem)).Length; i++)
            {
                groups.Add(new GroupSprites());
            }

            foreach (var item in MapManager.Items)
            {
                if (item.Textures.Count > 0)
                {
                    ImageSource _image = item.Textures[0].Texture1.ToImage();
                    item.Image = _image;
                }

                foreach (var text in item.Textures)
                {
                    item.Sprites.Add(new TMSpriteTexture() { Sprite1 = text.Texture1.ToTexture2D(), Sprite2 = text.Texture2.ToTexture2D(), Sprite3 = text.Texture3.ToTexture2D(), Sprite4 = text.Texture4.ToTexture2D() });
                }


                switch ((TypeItem)item.Type)
                {
                    case TypeItem.Tile:
                        groups[0].Items.Add(item);
                        break;

                    case TypeItem.Border:
                        groups[1].Items.Add(item);
                        break;

                    case TypeItem.Field:
                        groups[2].Items.Add(item);
                        break;

                    case TypeItem.Item:
                        groups[3].Items.Add(item);
                        break;

                    case TypeItem.Tree:
                        groups[4].Items.Add(item);
                        break;

                    case TypeItem.Wall:
                        groups[5].Items.Add(item);
                        break;

                    case TypeItem.Stair:
                        groups[6].Items.Add(item);
                        break;

                    case TypeItem.Door:
                        groups[7].Items.Add(item);
                        break;
                }

                
                index++;
                gridItems.barItems.Value = index;
                await Task.Delay(1);
            }

            if (GroupIndex >= 0)
            {
                sprites = new ObservableCollection<TMSprite>(groups[GroupIndex].Items);
                lstSprites.ItemsSource = sprites;
            }
           
            gridItems.Visibility = Visibility.Hidden;
            await Task.Delay(1);

        }

        void onSelectSpriteChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (lstSprites.SelectedIndex >= 0)
                {
                    ItemSelect = sprites[lstSprites.SelectedIndex] as TMSprite;
                    MapCore.Instance.ItemSelect = ItemSelect;
                }
            }
        }

        void onGroupSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (GroupIndex >= 0)
                {
                    sprites = new ObservableCollection<TMSprite>(groups[GroupIndex].Items);
                    lstSprites.ItemsSource = sprites;
                    
                }
            }
        }

        void onLoadMap()
        {
            Title = $"{MapManager.MapBase.mapInfo.Name} - [{FileMap}]";
            onLoadScrolls();
        }

        void onLoadScrolls()
        {
            hScroll.Minimum = 0;
            hScroll.Value = 0;
            hScroll.Maximum = MapManager.MapBase.mapInfo.Size.X;

            vScroll.Minimum = 0;
            vScroll.Value = 0;
            vScroll.Maximum = MapManager.MapBase.mapInfo.Size.Y;
        }

        void onScrollHorizontalChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MapManager.Camera.ToMove((int)hScroll.Value, (int)vScroll.Value);
        }

        void onScrollVerticalChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MapManager.Camera.ToMove((int)hScroll.Value, (int)vScroll.Value);
        }

        void onMouseMove(object sender, MouseEventArgs e)
        {
            Mouse = new Point(MapCore.Instance.GlobalPos.X, MapCore.Instance.GlobalPos.Y);
        }

    }
}
