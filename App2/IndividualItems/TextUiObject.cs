using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace App2.IndividualItems
{
    public class TextUiObject : IndividualElementsBase
    {
        public TextUiObject() : base()
        {
            Type = "Text";
        }
        public override ObservableCollection<string> CommandSource => new ObservableCollection<string> { "Duplicate", "Delete" };

        public override UiObject Clone(UiObject uiObject)
        {
            var serialized = JsonConvert.SerializeObject(this);
            var obj = JsonConvert.DeserializeObject<TextUiObject>(serialized);

            obj.Items.Clear();

            foreach (var item in uiObject.Items)
                obj.Items.Add(Clone(item));

            return obj;
        }

    }
}
