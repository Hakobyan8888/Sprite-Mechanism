using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

namespace CommandBestPracticeUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new ViewModel();
            DataContext = ViewModel;
        }
        public ViewModel ViewModel { get; set; }
    }

    public class ViewModel : BindableBase
    {
        public StandardUICommand AddItemCommand { get; set; }
        public StandardUICommand DeleteItemCommand { get; set; }
        public ObservableCollection<Model> Models { get; set; }
        private int CurrentIndex = 0;
        public ViewModel()
        {
            AddItemCommand = new StandardUICommand();
            AddItemCommand.ExecuteRequested += AddItemCommand_ExecuteRequested;
            DeleteItemCommand = new StandardUICommand();
            DeleteItemCommand.ExecuteRequested += DeleteItemCommand_ExecuteRequested;
            Models = new ObservableCollection<Model>();
            for (CurrentIndex = 0; CurrentIndex < 15; CurrentIndex++)
            {
                Models.Add(new Model($"Model {CurrentIndex + 1}"));
            }
        }

        private void DeleteItemCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            if (Models.Count > 0)
                Models.Remove(Models.Last());
        }

        private void AddItemCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            Models.Add(new Model($"Model {++CurrentIndex}"));
        }


    }

    public class Model : BindableBase
    {
        public Model(string name)
        {
            ModelName = name;
        }
        private string _modelName;

        public string ModelName
        {
            get => _modelName;
            set => _modelName = value;
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

}
