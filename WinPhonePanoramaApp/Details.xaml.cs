using System;
using System.Windows;
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
        }
    }
}