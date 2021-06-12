using App2.IndividualItems;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace App2
{
    public class GroupUiObject : UiObject
    {
        public float CanvasRight { get; set; }
        public float CanvasBottom { get; set; }

        public GroupUiObject() : base()
        {
            Type = "Group";
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InitializeUpdate(this);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    InitializeUpdate(this);
                    break;
                default:
                    break;
            }
        }

        public override void InitializeUpdate(UiObject uiObject)
        {
            CanvasTop = int.MaxValue;
            CanvasLeft = int.MaxValue;
            CanvasRight = int.MinValue;
            CanvasBottom = int.MinValue;
            if (uiObject.Items == null) return;

            Update(uiObject);
        }


        public override void Update(UiObject uiObject)
        {
            foreach (var item in uiObject.Items)
            {
                if (item is IndividualElementsBase)
                {
                    CanvasTop = Math.Min(CanvasTop, item.CanvasTop);
                    CanvasLeft = Math.Min(CanvasLeft, item.CanvasLeft);
                    CanvasRight = Math.Max(item.CanvasLeft + item.Width, CanvasRight);
                    CanvasBottom = Math.Max(item.CanvasTop + item.Height, CanvasBottom);
                    Width = CanvasRight - CanvasLeft;
                    Height = CanvasBottom - CanvasTop;
                    item.MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
                }
                else
                    Update(item);
            }
            UpdateOnlyParent = true;
            Rotation = 0;
            UpdateOnlyParent = false;
        }

        public override ObservableCollection<string> CommandSource => new ObservableCollection<string> { "UnGroup", "Duplicate", "Delete" };

        public override void HandleObjectMove(UiObject item, UiObject parent, float leftDelta, float topDelta)
        {
            if (item.Items == null) return;

            if (item.Equals(parent))
            {
                CanvasTop += topDelta;
                CanvasLeft += leftDelta;
            }

            foreach (var i in item.Items)
            {
                RotateObjects(0, 0);
                i.HandleObjectMove(i, item, leftDelta, topDelta);
                RotateObjects(Rotation, 0);
            }
        }

        public override void HandleRotation(UiObject item, UiObject parent, float value)
        {
            if (item.Items == null) return;

            if (!item.Equals(parent))
                Rotation += value;

            foreach (var i in item.Items)
            {
                i.HandleRotation(i, parent, value);
                i.CanvasLeft = (float)(i.PositionAfterRotation.X - i.Width / 2);
                i.CanvasTop = (float)(i.PositionAfterRotation.Y - i.Height / 2);
            }
        }

        public override void HandleOpacity(UiObject item, UiObject parent, int value)
        {
            if (item.Items == null) return;

            if (!item.Equals(parent))
                Opacity += value;

            foreach (var i in item.Items)
            {
                i.HandleOpacity(i, parent, value);
            }
        }

        public override void HandleRightScale(UiObject item, UiObject parent, float scale, float deltaWidth)
        {
            if (item.Items == null) return;

            if (item.Equals(parent))
            {
                var w = Width;
                CanvasTop -= (float)(0.5 * deltaWidth * Math.Sin(Radian));
                CanvasLeft += (float)(deltaWidth * 0.5 * (1 - Math.Cos(Radian)));
                Width -= (float)deltaWidth;
                scale = w / Width;
            }

            foreach (var i in item.Items)
            {
                if (i is IndividualElementsBase)
                {
                    var cLeft = i.CanvasLeft - CanvasLeft;
                    var delta = cLeft - cLeft / scale;

                    i.Width /= scale;
                    i.CanvasLeft -= (float)(delta - 0.5 * deltaWidth * (1 - Math.Cos(Radian)) / scale);
                    i.CanvasTop -= (float)(0.5 * deltaWidth * Math.Sin(Radian));
                    i.MiddlePoint = new Point(i.Width / 2 + i.CanvasLeft, i.Height / 2 + i.CanvasTop);
                }
                else
                    HandleRightScale(i, item, scale, deltaWidth);
            }
        }

        public override void HandleLeftScale(UiObject item, UiObject parent, float scale, float deltaWidth, float previousCanvasLeft)
        {
            if (item.Items == null) return;

            if (item.Equals(parent))
            {
                var w = Width;
                previousCanvasLeft = CanvasLeft;
                CanvasTop += (float)(deltaWidth * Math.Sin(Radian) - 0.5 * deltaWidth * Math.Sin(Radian));
                CanvasLeft += (float)(deltaWidth * Math.Cos(Radian) + 0.5 * deltaWidth * (1 - Math.Cos(Radian)));
                Width -= (float)deltaWidth;
                scale = w / Width;
            }

            foreach (var i in item.Items)
            {
                if (i is IndividualElementsBase)
                {
                    var sin = Math.Sin(Radian);

                    i.Width /= scale;
                    i.CanvasLeft = (float)(CanvasLeft + (i.CanvasLeft - previousCanvasLeft) / scale);
                    i.CanvasTop += (float)(0.5 * deltaWidth * sin);
                    i.MiddlePoint = new Point(i.Width / 2 + i.CanvasLeft, i.Height / 2 + i.CanvasTop);
                }
                else
                    HandleLeftScale(i, item, scale, deltaWidth, previousCanvasLeft);
            }
        }

        public override void HandleTopScale(UiObject item, UiObject parent, float scale, float deltaHeight, float previousCanvasTop)
        {
            if (item.Items == null) return;

            if (item.Equals(parent))
            {
                previousCanvasTop = CanvasTop;
                var h = Height;
                CanvasTop += (float)(deltaHeight * Math.Cos(Radian) + 0.5 * deltaHeight * (1 - Math.Cos(Radian)));
                CanvasLeft += (float)(deltaHeight * Math.Sin(-Radian) - (0.5 * deltaHeight * Math.Sin(-Radian)));
                Height -= (float)deltaHeight;
                scale = h / Height;
            }

            foreach (var i in item.Items)
            {
                if (i is IndividualElementsBase)
                {
                    var sin = Math.Sin(Radian);

                    i.Height /= scale;
                    i.CanvasTop = CanvasTop + (i.CanvasTop - previousCanvasTop) / scale;
                    i.CanvasLeft += (float)(0.5 * deltaHeight * -sin);
                    i.MiddlePoint = new Point(i.Width / 2 + i.CanvasLeft, i.Height / 2 + i.CanvasTop);
                }
                else
                    HandleRightScale(i, item, scale, deltaHeight);
            }
        }

        public override void HandleBottomScale(UiObject item, UiObject parent, float scale, float deltaHeight)
        {
            if (item.Items == null) return;

            if (item.Equals(parent))
            {
                var h = Height;
                CanvasTop += (float)(0.5 * deltaHeight * (1 - Math.Cos(-Radian)));
                CanvasLeft -= (float)(0.5 * deltaHeight * Math.Sin(-Radian));
                Height -= (float)deltaHeight;
                scale = h / Height;
            }

            foreach (var i in item.Items)
            {
                if (i is IndividualElementsBase)
                {
                    i.Height /= scale;
                    var sin = (float)Math.Sin(-Radian);
                    var cos = (float)Math.Cos(-Radian);
                    var cTop = i.CanvasTop - CanvasTop;
                    var delta = cTop - cTop / scale;

                    i.CanvasTop -= (float)(delta - 0.5 * deltaHeight * (1 - cos) / scale);
                    i.CanvasLeft -= (float)0.5 * deltaHeight * sin;
                    i.MiddlePoint = new Point(i.Width / 2 + i.CanvasLeft, i.Height / 2 + i.CanvasTop);
                }
                else
                    HandleRightScale(i, item, scale, deltaHeight);
            }
        }

        public override UiObject Clone(UiObject uiObject)
        {
            var serialized = JsonConvert.SerializeObject(this);
            var obj = JsonConvert.DeserializeObject<GroupUiObject>(serialized);

            obj.Items.Clear();

            foreach (var item in uiObject.Items)
                obj.Items.Add(item.Clone(item));

            return obj;

        }


        public override void RotateObjects(float rotation, float rotationDelta)
        {
            var items = Items;

            var centerX = Width / 2 + CanvasLeft;
            var centerY = Height / 2 + CanvasTop;
            var rt = new RotateTransform
            {
                CenterX = centerX,
                CenterY = centerY,
                Angle = rotation
            };

            foreach (var item in items)
            {
                item.PositionAfterRotation = rt.TransformPoint(item.MiddlePoint);
            }

            HandleRotation(this, this, -rotationDelta);
        }

    }
}
