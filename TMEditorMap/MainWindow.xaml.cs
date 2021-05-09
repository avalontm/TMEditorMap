using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TMEditorMap.Helpers;
using TMEditorMap.Models;
using TMFormat;
using TMFormat.Formats;

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
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onNew(object sender, RoutedEventArgs e)
        {

        }

        void onOpen(object sender, RoutedEventArgs e)
        {

        }

        void onSave(object sender, RoutedEventArgs e)
        {

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

            List<TMItem> items = TMItem.Load(Path.Combine(root, "data", "items.dat"));

            gridItems.barItems.Minimum = 0;
            gridItems.barItems.Maximum = items.Count;

            gridItems.Visibility = Visibility.Visible;
            await Task.Delay(1);

            int index = 0;

            foreach (var item in items)
            {
                ImageSource _image = null;

                if (item.Textures.Count > 0)
                {
                    _image = item.Textures[0].Texture1.ToImage();
                }

                sprites.Add(new TMSprite() { id = item.Id, name = item.Name, image = _image });
                index++;
                gridItems.barItems.Value = index;
                await Task.Delay(1);
            }

            lstSprites.ItemsSource = sprites;
            gridItems.Visibility = Visibility.Hidden;
            await Task.Delay(1);

        }

        void onSelectSpriteChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        void onGroupSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
