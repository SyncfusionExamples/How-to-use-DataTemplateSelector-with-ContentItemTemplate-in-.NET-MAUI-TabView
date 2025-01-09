using Syncfusion.Maui.TabView;

namespace TabViewItemsSource
{
    public class CustomSfTabView : SfTabView
    {
        // Dependency property for ItemTemplateSelector
        public static readonly BindableProperty ContentItemTemplateSelectorProperty =
            BindableProperty.Create(
                nameof(ContentItemTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(CustomSfTabView),
                null,
                propertyChanged: OnContentItemTemplateSelectorChanged);

        // Property wrapper
        public DataTemplateSelector ContentItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContentItemTemplateSelectorProperty);
            set => SetValue(ContentItemTemplateSelectorProperty, value);
        }

        // Handle changes to the template selector
        private static void OnContentItemTemplateSelectorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as CustomSfTabView;
            if (tabView != null)
            {
                tabView.SetInitialTemplate();
            }
        }

        // Constructor
        public CustomSfTabView()
        {
            // Wire up selection changed event
            SelectionChanged += OnSelectionChanged;
            Loaded += OnLoaded;
        }

        // Loaded event handler
        private void OnLoaded(object? sender, EventArgs e)
        {
            SetInitialTemplate();
        }

        // Selection changed handler
        private void OnSelectionChanged(object? sender, TabSelectionChangedEventArgs e)
        {
            if (e.NewIndex != -1 && ItemsSource != null && ContentItemTemplateSelector != null)
            {
                var selectedItem = ItemsSource.Cast<object>().ElementAt((Index)e.NewIndex);
                HandleTabSelection(selectedItem);
            }
        }

        // Method to set initial template
        private void SetInitialTemplate()
        {
            // Ensure both ItemsSource and ContentItemTemplateSelector are available
            if (ItemsSource != null && ContentItemTemplateSelector != null)
            {
                // Try to get the first item
                var items = ItemsSource.Cast<object>().ToList();
                if (items.Any())
                {
                    var firstItem = items.First();

                    // Select template for the first item
                    var selectedTemplate = ContentItemTemplateSelector.SelectTemplate(firstItem, this);

                    // Directly set the ContentItemTemplate
                    ContentItemTemplate = selectedTemplate;

                    // Optional: Log for debugging
                    System.Diagnostics.Debug.WriteLine($"Initial template set for item type: {firstItem?.GetType()}");
                }
            }
        }
        // Dynamic template selection method
        private void HandleTabSelection(object selectedItem)
        {
            // Directly use the ContentItemTemplateSelector if available
            if (ContentItemTemplateSelector != null)
            {
                ContentItemTemplate = ContentItemTemplateSelector.SelectTemplate(selectedItem, this);
            }
        }
    }
}
