using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.UserDataAccounts.SystemAccess;
using Windows.Foundation;

namespace App2.IndividualItems
{
    public class IndividualElementsBase : UiObject
    {
        public IndividualElementsBase() : base()
        {
            Items = new ObservableCollection<UiObject>();
            MiddlePoint = new Point(Width / 2 + CanvasLeft, Height / 2 + CanvasTop);
        }

        public override void HandleObjectMove(UiObject item, UiObject parent, float leftDelta, float topDelta)
        {
            item.CanvasTop += topDelta;
            item.CanvasLeft += leftDelta;
            item.MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
        }

        public override void HandleLeftScale(UiObject item, UiObject parent, float scale, float deltaWidth, float previousCanvasLeft)
        {
            CanvasTop += (float)(deltaWidth * Math.Sin(Radian) - 0.5 * deltaWidth * Math.Sin(Radian));
            CanvasLeft += (float)(deltaWidth * Math.Cos(Radian) + 0.5 * deltaWidth * (1 - Math.Cos(Radian)));
            Width -= (float)deltaWidth;
            MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
        }

        public override void HandleRightScale(UiObject item, UiObject parent, float scale, float deltaWidth)
        {
            CanvasTop -= (float)(0.5 * deltaWidth * Math.Sin(Radian));
            CanvasLeft += (float)(deltaWidth * 0.5 * (1 - Math.Cos(Radian)));
            Width -= (float)deltaWidth;
            MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
        }

        public override void HandleBottomScale(UiObject item, UiObject parent, float scale, float deltaHeight)
        {
            CanvasTop += (float)(0.5 * deltaHeight * (1 - Math.Cos(-this.Radian)));
            CanvasLeft -= (float)(0.5 * deltaHeight * Math.Sin(-this.Radian));
            Height -= (float)deltaHeight;
            MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
        }

        public override void HandleTopScale(UiObject item, UiObject parent, float scale, float deltaHeight, float previousCanvasTop)
        {
            CanvasTop += (float)(deltaHeight * Math.Cos(Radian) + 0.5 * deltaHeight * (1 - Math.Cos(Radian)));
            CanvasLeft += (float)(deltaHeight * Math.Sin(-Radian) - (0.5 * deltaHeight * Math.Sin(-Radian)));
            Height -= (float)deltaHeight;
            MiddlePoint = new Point(item.Width / 2 + item.CanvasLeft, item.Height / 2 + item.CanvasTop);
        }

        public override void HandleRotation(UiObject item, UiObject parent, float value)
        {
            if (item != null && !item.Equals(parent))
            {
                item.Rotation += value;
            }
        }

        public override void HandleOpacity(UiObject item, UiObject parent, int value)
        {
            if (item != null && !item.Equals(parent))
                Opacity += value;
        }
    }
}
