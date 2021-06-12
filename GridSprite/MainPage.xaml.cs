using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GridSprite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<UIElement> _images;
        public ObservableCollection<UIElement> Images
        {
            get => _images;
            set
            {
                _images = value;
                NotifyPropertyChanged("Images");
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                NotifyPropertyChanged("Width");
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                NotifyPropertyChanged("Height");
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Images = new ObservableCollection<UIElement>()
            {
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new TextBlock()
                {
                    Text = "Hello World asdf asldfk jasdlkfj laksdjf lkasjdfl kjasdlfkj ",
                    TextWrapping = TextWrapping.Wrap
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
                new Image()
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/picture.jpg")),
                    Stretch = Stretch.Uniform,
                },
            };
            Width = 100;
            Height = 100;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
