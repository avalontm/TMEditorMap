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
    public partial class SignWindow : Window, INotifyPropertyChanged
    {
        #region Propiedades

        public static SignWindow Instance { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public SignWindow(TMSprite _item)
        {
            InitializeComponent();
            Instance = this;
            item = _item;
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
            this.Close();
        }
    }
}
