using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;

namespace WinPhonePanoramaApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            DailyDeviationItems = new ObservableCollection<ItemViewModel>();
            MostPopularItems = new ObservableCollection<ItemViewModel>();
            LatestItems = new ObservableCollection<ItemViewModel>();
        }

        public ObservableCollection<ItemViewModel> DailyDeviationItems { get; private set; }
        public ObservableCollection<ItemViewModel> MostPopularItems { get; private set; }
        public ObservableCollection<ItemViewModel> LatestItems { get; private set; }

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
            GetDailyDeviations("http://backend.deviantart.com/rss.xml?q=special%3Add");
            GetMostPopular("http://backend.deviantart.com/rss.xml?type=deviation&q=boost%3Apopular");
            GetLatest("http://backend.deviantart.com/rss.xml?type=deviation&q=sort%3Atime"); 

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

        private void GetDailyDeviations(string url)
        {
            var targetUri = new System.Uri(url);
            var request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(DailyDeviationCallback, request);
        }

        private void GetMostPopular(string url)
        {
            var targetUri = new System.Uri(url);
            var request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(MostPopularCallback, request);
        }

        private void GetLatest(string url)
        {
            var targetUri = new System.Uri(url);
            var request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            request.BeginGetResponse(LatestCallback, request);
        }

        private void DailyDeviationCallback(IAsyncResult callbackResult)
        {
            var myRequest = (HttpWebRequest)callbackResult.AsyncState;
            var myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (var httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
                Deployment.Current.Dispatcher.BeginInvoke(() => ParseRssDataIntoCollection(results, DailyDeviationItems));
            }
            myResponse.Close();
        }

        private void MostPopularCallback(IAsyncResult callbackResult)
        {
            var myRequest = (HttpWebRequest)callbackResult.AsyncState;
            var myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (var httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
                Deployment.Current.Dispatcher.BeginInvoke(() => ParseRssDataIntoCollection(results, MostPopularItems));
            }
            myResponse.Close();
        }

        private void LatestCallback(IAsyncResult callbackResult)
        {
            var myRequest = (HttpWebRequest)callbackResult.AsyncState;
            var myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (var httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
                Deployment.Current.Dispatcher.BeginInvoke(() => ParseRssDataIntoCollection(results, LatestItems));
            }
            myResponse.Close();
        }

        private void ParseRssDataIntoCollection(string results, ObservableCollection<ItemViewModel> items)
        {
            int resultCount = 0; 
            while (results.Contains("<item>") && resultCount < 100)
            {
                results = results.Substring(results.IndexOf("<item>") + 6); 

                //is a 'mature' image
                var wholeItem = results.Substring(0, results.IndexOf("</item>"));
                if (!wholeItem.ToLower().Contains("ismature"))
                {
                    //<title>Dubstep Girl</title>
                    var startIndex = results.IndexOf("<title>") + 7;
                    var title = results.Substring(startIndex, results.IndexOf("</title>") - startIndex);
                    //<media:credit role="author" scheme="urn:ebu">MrPyrOs</media:credit>
                    startIndex = results.IndexOf("<media:credit role=\"author\" scheme=\"urn:ebu\">") + 45;
                    var author = results.Substring(startIndex, results.IndexOf("</media:credit>") - startIndex);
                    //<media:thumbnail url="http://th06.deviantart.net/fs71/150/f/2013/344/e/b/dubstep_girl_by_mrpyros-d6xfbik.png" height="60" width="150"/>
                    var thumbnail = results.Substring(results.IndexOf("<media:thumbnail url=\"") + 22);
                    thumbnail = thumbnail.Substring(0, thumbnail.IndexOf("\""));
                    //<media:content url="http://fc02.deviantart.net/fs70/i/2011/074/4/5/poinson_frogs_by_greenestreet-d3bp5bu.jpg" height="484" width="900" medium="image"/>
                    var fullImageUrl = results.Substring(results.IndexOf("<media:content url=\"") + 20);
                    fullImageUrl = fullImageUrl.Substring(0, fullImageUrl.IndexOf("\""));

                    items.Add(new ItemViewModel
                                  {
                                      Title = title,
                                      Author = author,
                                      ImageUrl = thumbnail,
                                      FullDetails = fullImageUrl + "|" + title
                                  });
                }

                results = results.Substring(results.IndexOf("</item>") + 7);
                resultCount++;
            }

            //mostPopularItems.Add(new ItemViewModel() { Title = "runtime sixteen", Author = "Nascetur pharetra placerat pulvinar", ImageUrl = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
        }
    }
}