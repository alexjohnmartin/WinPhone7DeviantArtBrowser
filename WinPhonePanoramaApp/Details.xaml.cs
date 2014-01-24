using System;
using System.Net;
using System.Windows;
using System.Windows.Media;
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
                }
                else
                {
                    ImageView.Source = IsolatedStorageHelper.GetImage(_imageUrl);
                }
                    
                UpdateBarMenuItem();
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
                UpdateBarMenuItem();
            }
            else
            {
                ShowToast("Image download error: " + e.Error.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            IsolatedStorageHelper.DeleteImage(GetFilenameFromUrl(_imageUrl));
            UpdateBarMenuItem();
        }

        private void UpdateBarMenuItem()
        {
            var item = (ApplicationBarMenuItem)ApplicationBar.MenuItems[0];
            item.Click -= Download_Click;
            item.Click -= Delete_Click; 
            if (!IsolatedStorageHelper.DoesFileExist(GetFilenameFromUrl(_imageUrl)))
            {
                item.Text = "download";
                item.Click += Download_Click;
            }
            else
            {
                item.Text = "delete";
                item.Click += Delete_Click;
            }
        }

        private static string GetFilenameFromUrl(string imageUrl)
        {
            if (!imageUrl.Contains("/")) return imageUrl;
            return imageUrl.Substring(imageUrl.LastIndexOf('/') + 1); 
        }

        private static void ShowToast(string message)
        {
            new ShellToast {Title = "Downloaded", Content = message}.Show();
        }
    }
}