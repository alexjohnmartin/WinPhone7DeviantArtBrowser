using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RateMyApp.Resources;

namespace WinPhonePanoramaApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            Loaded += MainPage_Loaded;

            //BuildApplicationBar();

            //FeedbackOverlay.VisibilityChanged += FeedbackOverlay_VisibilityChanged;
        }

        //public void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
        //{
        //    ApplicationBar.IsVisible = (FeedbackOverlay.Visibility != Visibility.Visible);
        //}

        //private void BuildApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar
        //    ApplicationBar = new ApplicationBar {Mode = ApplicationBarMode.Minimized};

        //    // Create reset menu item
        //    var appBarMenuItem = new ApplicationBarMenuItem("reset");
        //    //var appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    appBarMenuItem.Click += new EventHandler(Reset_Click);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        //private void Reset_Click(object sender, EventArgs e)
        //{
        //    FeedbackOverlay.Reset();
        //}


        // Load data for the ViewModel MostPopularItems
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            else
            {
                App.ViewModel.UpdateDownloads();
            }
        }

        private void StackPanel_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                var stackPanel = (StackPanel)sender;
                var itemText = stackPanel.Tag.ToString();
                string testTitle = itemText.Substring(itemText.IndexOf("|") + 1);
                string imageUrl = itemText.Substring(0, itemText.IndexOf("|"));

                //string testTitle = "test image title";
                NavigationService.Navigate(new Uri("/Details.xaml?title=" + testTitle + "&imageUrl=" + imageUrl, UriKind.Relative));
            }
            catch (Exception)
            {
                //log error

                //redirect to 'about' page?
            }
        }
    }
}