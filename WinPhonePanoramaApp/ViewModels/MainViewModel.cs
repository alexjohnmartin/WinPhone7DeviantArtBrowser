using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace WinPhonePanoramaApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            MostPopularItems = new ObservableCollection<ItemViewModel>();
            LatestItems = new ObservableCollection<ItemViewModel>();
        }

        public ObservableCollection<ItemViewModel> MostPopularItems { get; private set; }
        public ObservableCollection<ItemViewModel> LatestItems { get; private set; }

        //private string _sampleProperty = "Sample Runtime Property Value";
        //public string SampleProperty
        //{
        //    get
        //    {
        //        return _sampleProperty;
        //    }
        //    set
        //    {
        //        if (value != _sampleProperty)
        //        {
        //            _sampleProperty = value;
        //            NotifyPropertyChanged("SampleProperty");
        //        }
        //    }
        //}

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the MostPopularItems collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime one", Author = "Maecenas praesent accumsan bibendum", ImageUrl = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime two", Author = "Dictumst eleifend facilisi faucibus", ImageUrl = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime three", Author = "Habitant inceptos interdum lobortis", ImageUrl = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime four", Author = "Nascetur pharetra placerat pulvinar", ImageUrl = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime five", Author = "Maecenas praesent accumsan bibendum", ImageUrl = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime six", Author = "Dictumst eleifend facilisi faucibus", ImageUrl = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime seven", Author = "Habitant inceptos interdum lobortis", ImageUrl = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime eight", Author = "Nascetur pharetra placerat pulvinar", ImageUrl = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime nine", Author = "Maecenas praesent accumsan bibendum", ImageUrl = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime ten", Author = "Dictumst eleifend facilisi faucibus", ImageUrl = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime eleven", Author = "Habitant inceptos interdum lobortis", ImageUrl = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime twelve", Author = "Nascetur pharetra placerat pulvinar", ImageUrl = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime thirteen", Author = "Maecenas praesent accumsan bibendum", ImageUrl = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime fourteen", Author = "Dictumst eleifend facilisi faucibus", ImageUrl = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime fifteen", Author = "Habitant inceptos interdum lobortis", ImageUrl = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //MostPopularItems.Add(new ItemViewModel() { Title = "runtime sixteen", Author = "Nascetur pharetra placerat pulvinar", ImageUrl = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });



            IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}