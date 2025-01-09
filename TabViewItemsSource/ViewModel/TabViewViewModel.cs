namespace TabViewItemsSource
{
    // ViewModel
    public class TabViewViewModel 
    {
        public List<BaseTabItem> TabItems { get; set; }

        public TabViewViewModel()
        {
            TabItems = new List<BaseTabItem>
            {
                new TextTabItem
                {
                    Title = "Text Tab",
                    Content = "This is a text tab content"
                },
                new ImageTabItem
                {
                    Title = "Image Tab",
                    ImageSource = "dotnet_bot.png"
                },
                new ComplexTabItem
                {
                    Title = "Complex Tab",
                    DetailedContent = new {
                        Name = "Complex Content",
                        Description = "Detailed information"
                    }
                }
            };
        }
    }
}
