using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpriteArchitectureTypeBased
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Test test { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            test = new Test();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Slider.SetValue(Slider.ValueProperty, Slider.Value + 2);
        }
    }

    public class Test : BindableBase
    {
        private int _opacity;
        public int Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }
    }


    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEmpty { get; set; }

        public virtual void Merge(BindableBase source)
        {
        }

        public bool SetProperty<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyChanges()
        {
            // Notify all properties
            NotifyPropertyChanged("");
        }
    }


    public class SpriteViewModelBase
    {
        private int opacity;
        public SettingsModel Settings { get; set; }


        public SpriteViewModelBase()
        {
            Settings = new SettingsModel();
        }

        public Layer Layer;

        public List<SpriteViewModelBase> Sprites { get; set; }

        public int Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                Settings.SetOpacity(this, opacity);
            }
        }

    }

    public class ImageSpriteViewModel
    {

    }


    public class SettingsModel
    {
        public void SetOpacity(SpriteViewModelBase sprite, int opacity)
        {
            if (sprite.Layer != null)
                sprite.Layer.Opacity = opacity;
            if (sprite.Sprites != null)
            {
                foreach (var s in sprite.Sprites)
                {
                    s.Opacity = opacity;
                }
            }
        }
    }


    public class Layer
    {
        public int Opacity { get; set; }
    }

}
