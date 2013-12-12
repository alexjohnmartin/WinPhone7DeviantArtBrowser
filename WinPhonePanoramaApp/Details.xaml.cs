using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace WinPhonePanoramaApp
{
    public partial class Details : PhoneApplicationPage
    {
        public Details()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("title"))
            {
                PageTitle.Text = NavigationContext.QueryString["title"];
            }

            if (NavigationContext.QueryString.ContainsKey("imageUrl"))
            {
                string imageUrl = NavigationContext.QueryString["imageUrl"];
                ImageView.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(imageUrl); 
            }
        }
    }
}