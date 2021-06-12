using System.Collections.ObjectModel;
using Windows.UI.ViewManagement;

namespace App2.MultiSelectObjects
{
    public class MultiSelectionUiObject : MultiSelectUiObjectBase
    {
        public MultiSelectionUiObject() : base()
        {
        }
        public override ObservableCollection<string> CommandSource => new ObservableCollection<string> { "Group", "Duplicate", "Delete" };

        public override void HandleObjectMove(UiObject item, UiObject parent, float leftDelta, float topDelta)
        {
            if (Items == null) return;

            foreach (var i in Items)
                i.HandleObjectMove(i, i, leftDelta, topDelta);
        }

        public override void HandleRotation(UiObject item, UiObject parent, float value)
        {
            if (Items == null) return;

            if (!item.Equals(parent))
                Rotation += value;

            foreach (var i in Items)
            {
                if (i is GroupUiObject)
                {
                    i.RotateObjects(i.Rotation, 0);
                    i.Rotation += value;
                }
                else
                    i.HandleRotation(i, parent, value);
            }
        }

        public override void HandleOpacity(UiObject item, UiObject parent, int value)
        {
            if (Items == null) return;

            if (!item.Equals(parent))
                Opacity += value;

            foreach (var i in Items)
            {
                i.HandleOpacity(i, i, value);
            }
        }

        public override void HandleRightScale(UiObject item, UiObject parent, float scale, float deltaWidth)
        {
            if (Items == null) return;

            foreach (var i in Items)
            {
                if (i is GroupUiObject)
                {
                    i.RotateObjects(0, 0);
                    i.HandleRightScale(i, i, scale, deltaWidth);
                    i.RotateObjects(i.Rotation, 0);
                }
                else
                    i.HandleRightScale(i, i, scale, deltaWidth);
            }
        }

        public override void HandleLeftScale(UiObject item, UiObject parent, float scale, float deltaWidth, float previousCanvasLeft)
        {
            if (Items == null) return;

            foreach (var i in Items)
            {
                if (i is GroupUiObject)
                {
                    i.RotateObjects(0, 0);
                    i.HandleLeftScale(i, i, scale, deltaWidth, previousCanvasLeft);
                    i.RotateObjects(i.Rotation, 0);
                }
                else
                    i.HandleLeftScale(i, i, scale, deltaWidth, previousCanvasLeft);
            }
        }

        public override void HandleTopScale(UiObject item, UiObject parent, float scale, float deltaHeight, float previousCanvasTop)
        {
            if (Items == null) return;

            foreach (var i in Items)
            {
                if (i is GroupUiObject)
                {
                    i.RotateObjects(0, 0);
                    i.HandleTopScale(i, i, scale, deltaHeight, previousCanvasTop);
                    i.RotateObjects(i.Rotation, 0);
                }
                else
                    i.HandleTopScale(i, i, scale, deltaHeight, previousCanvasTop);
            }
        }

        public override void HandleBottomScale(UiObject item, UiObject parent, float scale, float deltaHeight)
        {
            if (Items == null) return;

            foreach (var i in Items)
            {
                if (i is GroupUiObject)
                {
                    i.RotateObjects(0, 0);
                    i.HandleBottomScale(i, i, scale, deltaHeight);
                    i.RotateObjects(i.Rotation, 0);
                }
                else
                    i.HandleBottomScale(i, i, scale, deltaHeight);
            }
        }

    }
}
