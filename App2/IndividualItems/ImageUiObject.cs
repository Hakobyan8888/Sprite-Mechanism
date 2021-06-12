using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.IndividualItems
{
    public class ImageUiObject : IndividualElementsBase
    {
        public ImageUiObject() : base()
        {
            Type = "Image";
        }

        public override ObservableCollection<string> CommandSource => new ObservableCollection<string> { "Duplicate", "Delete" };

        public override UiObject Clone(UiObject uiObject)
        {
            var serialized = JsonConvert.SerializeObject(this);
            var obj = JsonConvert.DeserializeObject<ImageUiObject>(serialized);

            obj.Items.Clear();

            foreach (var item in uiObject.Items)
                obj.Items.Add(Clone(item));

            return obj;
        }
    }
}
