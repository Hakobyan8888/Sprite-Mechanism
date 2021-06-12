using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App2
{
    public class LayerTempalateSelector : DataTemplateSelector
    {
        public DataTemplate LayerDataTemplate { get; set; }
        public DataTemplate GroupDataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (item)
            {
                case GroupUiObject _:
                    return GroupDataTemplate;
                case UiObject _:
                    return LayerDataTemplate;
            }
            return base.SelectTemplateCore(item);

        }
    }
}
