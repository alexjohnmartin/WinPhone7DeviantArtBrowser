﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;

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
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel MostPopularItems
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
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