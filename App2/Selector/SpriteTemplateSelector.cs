using App2.IndividualItems;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App2.Selector
{
    public class SpriteTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate GroupTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (item)
            {
                case GroupUiObject _:
                    return GroupTemplate;
                case ImageUiObject _:
                    return ImageTemplate;
                case TextUiObject _:
                    return TextTemplate;
            }
            return base.SelectTemplateCore(item);

        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case GroupUiObject _:
                    return GroupTemplate;
                case ImageUiObject _:
                    return ImageTemplate;
                case TextUiObject _:
                    return TextTemplate;
            }
            return base.SelectTemplateCore(item);
        }
    }
}
