using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMEditorMap.Engine;
using TMFormat.Formats;

namespace TMEditorMap.Windows
{
    public partial class MapInfoWindow : Window, INotifyPropertyChanged
    {
        #region Propiedades

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _name, _autor, _version;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        public string autor
        {
            get { return _autor; }
            set
            {
                _autor = value;
                OnPropertyChanged("autor");
            }
        }

        public string version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("version");
            }
        }

        int _ancho, _alto;
        public int ancho
        {
            get { return _ancho; }
            set
            {
                _ancho = value;
                OnPropertyChanged("ancho");
            }
        }

        public int alto
        {
            get { return _alto; }
            set
            {
                _alto = value;
                OnPropertyChanged("alto");
            }
        }

        #endregion

        public MapInfoWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            name = MapManager.MapBase.mapInfo.Name;
            autor = MapManager.MapBase.mapInfo.Autor;
            version = MapManager.MapBase.mapInfo.Version;
            ancho = (int)MapManager.MapBase.mapInfo.Size.X;
            alto = (int)MapManager.MapBase.mapInfo.Size.Y;
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onMapInfoSave(object sender, RoutedEventArgs e)
        {
            var _result = MessageBox.Show(this, "¿Desea guardar los cambios?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (_result == MessageBoxResult.Yes)
            {
                MapManager.MapBase.mapInfo.Name = name;
                MapManager.MapBase.mapInfo.Autor = autor;
                MapManager.MapBase.mapInfo.Version = version;
                MapManager.MapBase.mapInfo.Size = new System.Numerics.Vector2(ancho, alto);
                this.Close();
            }
        }
    }
}
