using System.Collections.ObjectModel;

namespace App2.MultiSelectObjects
{
    public class MultiSelectUiObjectBase : UiObject
    {

        public MultiSelectUiObjectBase()
        {
            Items = new ObservableCollection<UiObject>();
        }

        public override void HandleRotation(UiObject item, UiObject parent, float value)
        {
            if (item.Items == null) return;

            if (!item.Equals(parent))
                Rotation += value;

            foreach (var i in item.Items)
            {
                i.HandleRotation(i, parent, value);
            }
        }

        public override void HandleOpacity(UiObject item, UiObject parent, int value)
        {
            if (item.Items == null) return;

            if (!item.Equals(parent))
                Opacity += value;

            foreach (var i in item.Items)
                i.HandleOpacity(i, parent, value);
        }
    }
}
