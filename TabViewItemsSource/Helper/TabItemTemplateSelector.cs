namespace TabViewItemsSource
{
    // DataTemplateSelector for TabView
    public class TabItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TextTemplate { get; set; }
        public DataTemplate? ImageTemplate { get; set; }
        public DataTemplate? ComplexTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                TextTabItem => TextTemplate,
                ImageTabItem => ImageTemplate,
                ComplexTabItem => ComplexTemplate,
                _ => null
            };
        }
    }
}
