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
using TMFormat.Formats;

namespace TMEditorMap.Windows
{
    public partial class TeleportWindow : Window, INotifyPropertyChanged
    {
        #region Propiedades

        public static TeleportWindow Instance { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        int _posx , _posy, _posz;

        public int posX
        {
            get { return _posx; }
            set
            {
                _posx = value;
                OnPropertyChanged("posX");
            }
        }

        public int posY
        {
            get { return _posy; }
            set
            {
                _posy = value;
                OnPropertyChanged("posY");
            }
        }

        public int posZ
        {
            get { return _posz; }
            set
            {
                _posz = value;
                OnPropertyChanged("posZ");
            }
        }

        TMSprite _item;
        public TMSprite item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged("item");
            }
        }
        #endregion

        public TeleportWindow(TMSprite _item)
        {
            InitializeComponent();
            Instance = this;
            item= _item;
            posX = (int)item.Destine.X;
            posY = (int)item.Destine.Y;
            posZ = (int)item.Destine.Z;
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            Instance = this;
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
        }

        void onSave(object sender, RoutedEventArgs e)
        {
            
            item.Destine = new System.Numerics.Vector3(posX, posY, posZ);
            this.Close();
        }
    }
}
