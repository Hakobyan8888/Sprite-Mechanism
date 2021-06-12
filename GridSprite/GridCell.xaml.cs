using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace GridSprite
{
    public sealed partial class GridCell : UserControl, INotifyPropertyChanged
    {
        public GridCell()
        {
            this.InitializeComponent();
        }
        private Geometry _geometry;

        public event PropertyChangedEventHandler PropertyChanged;

        public Geometry Geometry
        {
            get => _geometry;
            set
            {
                _geometry = value;
                NotifyPropertyChanged("Geometry");
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
