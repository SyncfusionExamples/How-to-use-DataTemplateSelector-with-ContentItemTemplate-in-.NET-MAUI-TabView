namespace TabViewSample
{
    // ViewModel
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
                        Description = "Alex is a software developer whose primary role is to design, implement, and maintain software applications. As a software engineer, Alex uses technical skills and coding expertise to create effective and robust solutions that meet the needs of users and businesses."
                    }
                }
            };
        }
    }
}
