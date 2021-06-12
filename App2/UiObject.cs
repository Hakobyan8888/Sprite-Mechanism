using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace App2
{
    public class UiObject : BindableBase
    {
        public float Radian => (float)(Math.PI / 180 * Rotation);

        public Point PositionAfterRotation { get; set; }

        private string _source;
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        private bool _isSelectedClone;
        /// <summary>
        /// for treeview (it must not have selected mode)
        /// </summary>
        public bool IsSelectedClone
        {
            get => _isSelectedClone;
            set => SetProperty(ref _isSelectedClone, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                BorderThickness = _isSelected ? new Thickness(10) : new Thickness(0);
            }
        }

        private bool _isTransformEnabled;
        public bool IsTransformEnabled
        {
            get => _isTransformEnabled;
            set => SetProperty(ref _isTransformEnabled, value);
        }

        private Thickness _borderThickness;

        public Thickness BorderThickness
        {
            get => _borderThickness;
            set => SetProperty(ref _borderThickness, value);
        }

        public UiObject()
        {
            Items = new ObservableCollection<UiObject>();
            Width = 500;
            Height = 500;
        }

        public string Type { get; set; }

        private ObservableCollection<UiObject> _items;
        public ObservableCollection<UiObject> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private int _opacity;
        private int _lastOpacity;

        public int Opacity
        {
            get => _opacity;
            set
            {
                SetProperty(ref _opacity, value);
                HandleOpacity(this, this, _opacity - _lastOpacity);
                _lastOpacity = _opacity;
            }
        }
        private float _rotation;
        private float _lastRotation;
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (value > 360)
                    value -= 360;
                else if (value < 0)
                    value += 360;
                RotateObjects(value, 0);
                SetProperty(ref _rotation, value);
                if (!UpdateOnlyParent)
                    HandleRotation(this, this, _rotation - _lastRotation);
                _lastRotation = _rotation;
            }
        }

        public bool UpdateOnlyParent { get; set; }

        public int FontSize { get; set; }
        public int Blend { get; set; }

        private float _canvasLeft;
        public float CanvasLeft
        {
            get => _canvasLeft;
            set => SetProperty(ref _canvasLeft, value);
        }

        private float _canvasTop;
        public float CanvasTop
        {
            get => _canvasTop;
            set => SetProperty(ref _canvasTop, value);
        }

        private float _width;
        public float Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        private float _height;

        public float Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public Point MiddlePoint { get; set; }

        public virtual ObservableCollection<string> CommandSource { get; }
        public float MinSpriteSize => 20;

        public virtual void InitializeUpdate(UiObject uiObject)
        {

        }

        public virtual void Update(UiObject uiObject)
        {

        }

        public virtual void HandleObjectMove(UiObject item, UiObject parent, float leftDelta, float topDelta)
        {

        }

        public virtual void HandleRotation(UiObject item, UiObject parent, float value)
        {
        }

        public virtual void HandleOpacity(UiObject item, UiObject parent, int value)
        {

        }

        public virtual void HandleRightScale(UiObject item, UiObject parent, float scale, float deltaWidth)
        {

        }

        public virtual void HandleLeftScale(UiObject item, UiObject parent, float scale, float deltaWidth, float previousCanvasLeft)
        {

        }

        public virtual void HandleTopScale(UiObject item, UiObject parent, float scale, float deltaHeight, float previousCanvasTop)
        {

        }

        public virtual void HandleBottomScale(UiObject item, UiObject parent, float scale, float deltaHeight)
        {

        }

        public virtual UiObject Clone(UiObject uiObject)
        {
            var serialized = JsonConvert.SerializeObject(uiObject);
            return JsonConvert.DeserializeObject<UiObject>(serialized);
        }

        public virtual void RotateObjects(float rotation, float rotationDelta)
        {

        }
    }
}
