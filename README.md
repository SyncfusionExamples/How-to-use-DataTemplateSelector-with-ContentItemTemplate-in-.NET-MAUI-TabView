This article provides a comprehensive guide on utilizing a `DataTemplateSelector` with the [ContentItemTemplate](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.TabView.SfTabView.html#Syncfusion_Maui_TabView_SfTabView_ContentItemTemplate) property in the [.NET MAUI TabView](https://www.syncfusion.com/maui-controls/maui-tab-view) control.

By following the steps below, you can create a flexible tab view that supports varying content for each tab based on its associated data model.

**Step 1:** Create a CustomTabView

- Extend the TabView control to add a `ContentItemTemplateSelector` property.
- Handle the [SelectionChanged](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.TabView.SfTabView.html?tabs=tabid-1#Syncfusion_Maui_TabView_SfTabView_SelectionChanged) and `Loaded` events to dynamically set the content template.

```
public class CustomTabView : SfTabView
{
    // Dependency property for ItemTemplateSelector
    public static readonly BindableProperty ContentItemTemplateSelectorProperty =
        BindableProperty.Create(
            nameof(ContentItemTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(CustomTabView),
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
        var tabView = bindable as CustomTabView;
        if (tabView != null)
        {
            tabView.SetInitialTemplate();
        }
    }

    // Constructor
    public CustomTabView()
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
```

**Step 2:** Create a DataTemplateSelector

- Implement a class that inherits from `DataTemplateSelector`.
- Define templates and logic to select the appropriate one based on the data type.

```
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
            _ => TextTemplate
        };
    }
}
```

**Step 3:** Define Data Models

- Create data models to represent the different types of content that need to be displayed in the TabView.

```
public abstract class BaseTabItem
{
    public string? Title { get; set; }
}

public class TextTabItem:BaseTabItem
{
    public string? Content { get; set; }
}

//Implement other data models.
```

**Step 5:** ViewModel

- Create a view model to provide the items for the tabs and bind it to your view.

```
public class TabViewModel 
{
    public List<BaseTabItem> TabItems { get; set; }

    public TabViewModel()
    {
        TabItems = new List<BaseTabItem>
        {
            new TextTabItem
            {
                Title = "Employee",
                Content = "Alex"
            },
            new ImageTabItem
            {
                Title = "Profile Picture",
                ImageSource = "user.png"
            },
            new ComplexTabItem
            {
                Title = "Description",
                DetailedContent = new {
                    Text = "Employee Details: ",
                    Description = "Alex is a software developer ..."
                }
            }
        };
    }
}
```

**Step 4:** XAML

- Define templates for each data model type in the ResourceDictionary.
- Assign the TabItemTemplateSelector and templates in XAML.
- Bind the CustomTabView to the ItemsSource and set the ContentItemTemplateSelector.

```
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TabViewItemsSource.MainPage"
             xmlns:local="clr-namespace:TabViewItemsSource"
             xmlns:tabView="clr-namespace:Syncfusion.Maui.TabView;assembly=Syncfusion.Maui.TabView">
    
    <ContentPage.BindingContext>
        <local:TabViewModel/>
    </ContentPage.BindingContext>
    
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:TabItemTemplateSelector x:Key="TabItemSelector">
                <local:TabItemTemplateSelector.TextTemplate>
                    <DataTemplate>
                        <Label Text="{Binding Content}"/>
                    </DataTemplate>
                </local:TabItemTemplateSelector.TextTemplate>

                <local:TabItemTemplateSelector.ImageTemplate>
                    <DataTemplate>
                        <Image Source="{Binding ImageSource}" />
                    </DataTemplate>
                </local:TabItemTemplateSelector.ImageTemplate>

                <local:TabItemTemplateSelector.ComplexTemplate>
                    <DataTemplate>
                        <StackLayout >
                            <Label Text="{Binding DetailedContent.Text}" />
                            <Label Text="{Binding DetailedContent.Description}" />
                        </StackLayout>
                    </DataTemplate>
                </local:TabItemTemplateSelector.ComplexTemplate>
            </local:TabItemTemplateSelector>
            <DataTemplate x:Key="headerItemTemplate">
                <StackLayout Margin="10">
                    <Label Text="{Binding Title}" VerticalOptions="Center"/>
                </StackLayout>
            </DataTemplate>
        </ResourceDictionary>
    </ContentPage.Resources>

    <local:CustomTabView ItemsSource="{Binding TabItems}"
        HeaderItemTemplate="{StaticResource headerItemTemplate}"
        ContentItemTemplateSelector="{StaticResource TabItemSelector}">
    </local:CustomTabView>
</ContentPage>
```

**Output**

![TabViewItemsSource.gif](https://support.syncfusion.com/kb/agent/attachment/article/18738/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjM0NTE0Iiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.QmyNTiwaVaoxcQlQK4vwaXG7FyceJF5ZBuicrry2ghY)

**Requirements to run the demo**
 
To run the demo, refer to [System Requirements for .NET MAUI](https://help.syncfusion.com/maui/system-requirements)
 
**Troubleshooting:**

**Path too long exception** 

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.