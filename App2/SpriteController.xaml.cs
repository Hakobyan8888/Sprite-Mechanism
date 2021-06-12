using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpriteController : Page
    {
        public Model ObjectSpriteModel
        {
            get { return (Model)GetValue(ObjectSpriteModelProperty); }
            set { SetValue(ObjectSpriteModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ObjectSpriteModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ObjectSpriteModelProperty =
            DependencyProperty.Register("ObjectSpriteModel", typeof(Model), typeof(SpriteController), new PropertyMetadata(0));

        public SpriteController()
        {
            this.InitializeComponent();
        }
    }
}
