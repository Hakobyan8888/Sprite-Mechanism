using App2.IndividualItems;
using App2.MultiSelectObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public Model Model { get; set; }

        private bool CtrlPressed => Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
        private bool ShiftPressed => Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

        public MainPage()
        {
            this.InitializeComponent();
            MultiSelectionHub.Instance = new MultiSelectionHub();
            Model = new Model();
            Model.SpritesSource.Add(new GroupUiObject
            {
                Items = new ObservableCollection<UiObject>
                {
                    new ImageUiObject(){ Source = "ms-appx:///Assets/download.jpg" },
                    new ImageUiObject() { Source = "ms-appx:///Assets/image2.jfif", CanvasLeft=550 }
        }
            });
        }

        private void TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var e = args.InvokedItem;
            if (e is UiObject obj)
            {
                if (CtrlPressed)
                    Model.AddSpriteToSelection(obj);
                else if (ShiftPressed)
                    Model.AddSpriteToGroupSelection(obj);
                else
                    Model.SelectSprite(obj);
            }
            Model.CurrentObject.InitializeUpdate(Model.CurrentObject);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "Duplicate":
                    Model.Duplicate();
                    break;
                case "Group":
                    Model.Group();
                    break;
                case "UnGroup":
                    Model.UnGroup();
                    break;
                case "Delete":
                    Model.Delete(Model.CurrentObject);
                    break;
                case "Hide":
                    Model.IsGroupMultiSelection = !Model.IsGroupMultiSelection;
                    break;
            }
        }
    }
    public class Model : BindableBase
    {
        public Model()
        {
            SpritesSource = new ObservableCollection<UiObject>();
            SpritesSource.CollectionChanged += SpritesSource_CollectionChanged;
        }

        private void SpritesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (item is UiObject uiObject)
                        {
                            CurrentObject = uiObject;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<UiObject> SpritesSource { get; set; }

        private UiObject _currentObject;

        public UiObject CurrentObject
        {
            get => _currentObject;
            set
            {
                SetProperty(ref _currentObject, value);

                if (_currentObject is GroupMultiSelectUiObject)
                    IsGroupMultiSelection = true;
                else
                    IsGroupMultiSelection = false;
            }
        }

        private bool _isGroupMultiSelection;
        public bool IsGroupMultiSelection
        {
            get => _isGroupMultiSelection;
            set => SetProperty(ref _isGroupMultiSelection, value);
        }

        public void AddSpriteToSelection(UiObject uiObject)
        {
            DeselectAllObjects();
            if (CurrentObject is MultiSelectUiObjectBase)
            {
                if (IsItemSelected(uiObject, CurrentObject))
                {
                    CurrentObject.Items.Remove(uiObject);
                    uiObject.IsSelected = false;
                }
                else
                {
                    CurrentObject.Items.Add(uiObject);
                    uiObject.IsSelected = true;
                    SelectObjects(CurrentObject, CurrentObject);
                }
            }
            else
            {
                if (IsItemSelected(uiObject, CurrentObject))
                {
                    SelectObjects(CurrentObject, CurrentObject);
                    return;
                }
                var newObject = new MultiSelectionUiObject();
                newObject.Items.Add(CurrentObject);
                newObject.Items.Add(uiObject);
                CurrentObject = newObject;
                SelectObjects(CurrentObject, CurrentObject);
            }
            MultiSelectionHub.Instance.CurrentMultiSelection = CurrentObject;
        }

        public void AddSpriteToGroupSelection(UiObject uiObject)
        {
            DeselectAllObjects();
            if (CurrentObject is MultiSelectUiObjectBase)
            {
                if (IsItemSelected(uiObject, CurrentObject))
                {
                    CurrentObject.Items.Remove(uiObject);
                    uiObject.IsSelected = false;
                }
                else
                {
                    CurrentObject.Items.Add(uiObject);
                    uiObject.IsSelected = true;
                    SelectObjects(CurrentObject, CurrentObject);
                }
            }
            else
            {
                if (IsItemSelected(uiObject, CurrentObject))
                {
                    SelectObjects(CurrentObject, CurrentObject);
                    return;
                }
                var newObject = new GroupMultiSelectUiObject();
                //var elementsOfCurrent = GetAllIndividualElements(CurrentObject);
                //var elementsOfNewObject = GetAllIndividualElements(uiObject);
                newObject.Items.Add(CurrentObject);
                newObject.Items.Add(uiObject);
                DeselectObjects(newObject);
                CurrentObject = newObject;
                SelectObjects(CurrentObject, CurrentObject);
            }
            MultiSelectionHub.Instance.CurrentMultiSelection = null;
        }

        public void SelectSprite(UiObject uiObject)
        {
            DeselectObjects(CurrentObject);
            CurrentObject = uiObject;
            SelectObjects(CurrentObject, CurrentObject);
            MultiSelectionHub.Instance.CurrentMultiSelection = null;
        }

        public void DeselectObjects(UiObject uiObject)
        {
            uiObject.IsTransformEnabled = false;
            uiObject.IsSelectedClone = false;
            uiObject.IsSelected = false;
            uiObject.UpdateOnlyParent = true;
            if (uiObject.Items == null) return;
            foreach (var item in uiObject.Items)
            {
                item.IsSelectedClone = false;
                item.IsSelected = false;
                item.UpdateOnlyParent = true;
                DeselectObjects(item);
            }
        }

        public void DeselectAllObjects()
        {
            foreach (var obj in SpritesSource)
                DeselectObjects(obj);
        }

        public void SelectObjects(UiObject uiObject, UiObject parent)
        {
            if (uiObject.Equals(parent) || parent is MultiSelectionUiObject)
                uiObject.IsTransformEnabled = true;
            uiObject.IsSelected = true;
            uiObject.UpdateOnlyParent = false;
            if (uiObject.Items == null) return;
            foreach (var item in uiObject.Items)
            {
                item.UpdateOnlyParent = false;
                item.IsSelected = true;
                SelectObjects(item, uiObject);
            }
        }

        #region Duplicate
        internal void Duplicate()
        {
            DeselectObjects(CurrentObject);
            var index = SpritesSource.IndexOf(CurrentObject);
            if (index == -1)
            {
                if (CurrentObject is MultiSelectUiObjectBase)
                {
                    foreach (var item in CurrentObject.Items)
                    {
                        index = SpritesSource.IndexOf(item);
                        if (index == -1)
                        {
                            DuplicateInParent(item);
                        }
                        else
                        {
                            DuplicateItem(item, index);
                        }
                    }
                }
                else
                {
                    DuplicateInParent(CurrentObject);
                }
            }
            else
            {
                DuplicateItem(CurrentObject, index);
            }
            SelectSprite(CurrentObject);
        }

        private void DuplicateItem(UiObject obj, int index)
        {
            var newObject = obj.Clone(obj);
            DeselectObjects(CurrentObject);
            SpritesSource.Insert(index, newObject);
            CurrentObject = newObject;
        }

        private void DuplicateInParent(UiObject item)
        {
            foreach (var baseItem in SpritesSource)
            {
                var parent = GetParentOfNode(baseItem, item, new UiObject());
                if (parent != null)
                {
                    var index = parent.Items.IndexOf(item);
                    parent.Items.Insert(index, item.Clone(item));
                    return;
                }
            }
        }
        #endregion

        public bool IsItemSelected(UiObject newObject, UiObject obj)
        {
            if (newObject.Equals(obj))
                return true;
            foreach (var item in obj.Items)
            {
                var isSelected = IsItemSelected(newObject, item);
                if (isSelected)
                    return isSelected;
            }

            return false;
        }

        internal void Group()
        {
            DeselectObjects(CurrentObject);
            var group = new GroupUiObject();
            var items = new ObservableCollection<UiObject>();
            items.AddRange(GetAllIndividualElements(CurrentObject).Reverse().ToList());
            var minIndex = int.MaxValue;

            foreach (var item in CurrentObject.Items)
            {
                foreach (var sprite in SpritesSource)
                {
                    var parent = GetParentOfNode(sprite, item, item);
                    if (parent != null)
                        minIndex = Math.Min(minIndex, SpritesSource.IndexOf(parent));
                }
            }
            int i = items.Count - 1;

            while (i > -1)
            {
                Delete(items[i]);
                i--;
            }

            group.Items.AddRange(items);
            SpritesSource.Insert(minIndex, group);
            SelectSprite(group);
        }

        internal void UnGroup()
        {
            DeselectObjects(CurrentObject);
            var individualElements = GetAllIndividualElements(CurrentObject).Reverse().ToList();
            var index = SpritesSource.IndexOf(CurrentObject);
            SpritesSource.Remove(CurrentObject);
            foreach (var item in individualElements)
            {
                SpritesSource.Insert(index, item);
            }
            SelectObjects(CurrentObject, CurrentObject);
        }

        internal void Delete(UiObject uiObject)
        {
            DeselectObjects(uiObject);
            var index = SpritesSource.IndexOf(uiObject);
            if (index == -1)
            {
                if (uiObject is MultiSelectUiObjectBase)
                {
                    foreach (var item in uiObject.Items)
                    {
                        index = SpritesSource.IndexOf(item);
                        if (index == -1)
                        {
                            DeleteInParent(item);
                        }
                        else
                        {
                            SpritesSource.Remove(item);
                        }
                    }
                }
                else
                {
                    DeleteInParent(uiObject);
                }
            }
            else
            {
                SpritesSource.Remove(uiObject);
            }
            SelectObjects(uiObject, uiObject);
        }

        private void DeleteInParent(UiObject item)
        {
            foreach (var baseItem in SpritesSource)
            {
                var parent = GetParentOfNode(baseItem, item, new UiObject());
                if (parent != null)
                {
                    parent.Items.Remove(item);

                    if (parent.Items.Count == 1)
                    {
                        var index = SpritesSource.IndexOf(parent);
                        var obj = parent.Items.FirstOrDefault();
                        SpritesSource.Remove(parent);
                        SpritesSource.Insert(index, obj);
                    }
                    return;
                }
            }


        }

        public UiObject GetParentOfNode(UiObject node, UiObject val, UiObject parent)
        {
            var obj = new UiObject();
            if (node == null) return null;

            if (node.Equals(val))
            {
                return parent;
            }
            else
            {
                if (node.Items.Count == 0)
                    return null;
                foreach (var item in node.Items)
                {
                    obj = GetParentOfNode(item, val, node);
                    if (obj != null) return obj;
                }
            }
            return obj;
        }

        private ObservableCollection<UiObject> GetAllIndividualElements(UiObject uiObject)
        {
            var uiObjects = new ObservableCollection<UiObject>();
            if (uiObject is IndividualElementsBase)
                uiObjects.Add(uiObject);
            foreach (var item in uiObject.Items)
            {
                uiObjects.AddRange(GetAllIndividualElements(item));
            }

            return uiObjects;
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

    public static class Ext
    {
        public static void AddRange<T>(this IList<T> coll, IList<T> addColl)
        {
            if (addColl != null)
                foreach (var item in addColl) coll.Add(item);
        }
    }

}
