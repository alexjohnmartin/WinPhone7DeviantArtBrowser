using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WinPhonePanoramaApp
{
    public partial class Details : PhoneApplicationPage
    {
        private string _imageUrl = string.Empty; 

        public Details()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            BuildApplicationBar(); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("title"))
            {
                PageTitle.Text = NavigationContext.QueryString["title"];
            }

            if (NavigationContext.QueryString.ContainsKey("imageUrl"))
            {
                _imageUrl = NavigationContext.QueryString["imageUrl"];
                if (_imageUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    ImageView.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(_imageUrl);
                    var item = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
                    item.Text = IsolatedStorageHelper.DoesFileExist(GetFilenameFromUrl(_imageUrl)) ? "re-download" : "download";
                }
                else
                {
                    ImageView.Source = IsolatedStorageHelper.GetImageFromIsolatedStorage(_imageUrl);
                    ApplicationBar.IsVisible = false; 
                }
            }
        }

        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar
            ApplicationBar = new ApplicationBar { Mode = ApplicationBarMode.Minimized };

            // Create reset menu item
            var appBarMenuItem = new ApplicationBarMenuItem("download");
            appBarMenuItem.Click += Download_Click;
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void Download_Click(object sender, EventArgs e)
        {
            var uri = new Uri(_imageUrl);
            var wc = new WebClient();
            //wc.Headers["Accept"] = "text/html, application/xhtml+xml, */*";
            //wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows Phone OS 7.5; Trident/3.1; IEMobile/7.0; Nokia; Lumia 710)";
            //wc.Headers["Host"] = uri.Host;
            //wc.Headers["Accept-Encoding"] = "gzip, deflate";
            //wc.Headers["UA_CPU"] = "ARM";
            //wc.Headers["Referer"] = "";
            wc.OpenReadCompleted += wc_OpenReadCompleted;
            wc.OpenReadAsync(uri);
        }

        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                IsolatedStorageHelper.SaveToJpeg(e.Result, GetFilenameFromUrl(_imageUrl));
                var item = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
                item.Text = "re-download";
            }
            else
            {
                ShowToast("Image download error: " + e.Error.Message);
            }
        }

        private string GetFilenameFromUrl(string imageUrl)
        {
            if (!imageUrl.Contains("/")) return imageUrl;
            return imageUrl.Substring(imageUrl.LastIndexOf('/') + 1); 
        }

        private void ShowToast(string message)
        {
            ShellToast toast = new ShellToast();
            toast.Title = "Downloaded";
            toast.Content = message;
            toast.Show();
        }
    }
}