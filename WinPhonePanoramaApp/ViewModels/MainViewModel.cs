using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WinPhonePanoramaApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            DailyDeviationItems = new ObservableCollection<ItemViewModel>();
            MostPopularItems = new ObservableCollection<ItemViewModel>();
            DownloadedItems = new ObservableCollection<ItemViewModel>();
            LatestItems = new ObservableCollection<ItemViewModel>();
        }

        public ObservableCollection<ItemViewModel> DailyDeviationItems { get; private set; }
        public ObservableCollection<ItemViewModel> MostPopularItems { get; private set; }
        public ObservableCollection<ItemViewModel> DownloadedItems { get; private set; }
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
            GetDownloaded(); 

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

        private void GetDownloaded()
        {
            DownloadedItems.Clear();
            foreach(var filename in IsolatedStorageHelper.GetImageFilenames())
            {
                DownloadedItems.Add(new ItemViewModel
                    {
                        Title = ExtractTitleFromFilename(filename),
                        Author = ExtractAuthorFromFilename(filename),
                        ImageUrl = IsolatedStorageHelper.GetImage(filename),
                        FullDetails = filename + "|" + ExtractTitleFromFilename(filename)
                    });
            }
        }

        private string ExtractAuthorFromFilename(string filename)
        {
            if (!filename.Contains("_by_")) return "unknown"; 

            var author = filename.Substring(filename.IndexOf("_by_") + 4);
            if (author.Contains("-"))
                author = author.Substring(0, author.IndexOf("-"));
            else
                author = author.Substring(0, author.IndexOf("."));

            return author.Substring(0, 1).ToUpper() + author.Substring(1); 
        }

        private string ExtractTitleFromFilename(string filename)
        {
            if (!filename.Contains("_by_")) return "unknown"; 

            var title = filename.Substring(0, filename.IndexOf("_by_")).Replace('_', ' ');
            return title.Substring(0, 1).ToUpper() + title.Substring(1); 
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
                if (!wholeItem.ToLower().Contains("ismature") &&
                    wholeItem.Contains("<title>") &&
                    wholeItem.Contains("<media:credit role=\"author\" scheme=\"urn:ebu\">") &&
                    wholeItem.Contains("<media:content url=\""))
                {
                    //<title>Dubstep Girl</title>
                    var startIndex = results.IndexOf("<title>") + 7;
                    var title = results.Substring(startIndex, results.IndexOf("</title>") - startIndex);
                    //<media:credit role="author" scheme="urn:ebu">MrPyrOs</media:credit>
                    startIndex = results.IndexOf("<media:credit role=\"author\" scheme=\"urn:ebu\">") + 45;
                    var author = results.Substring(startIndex, results.IndexOf("</media:credit>") - startIndex);
                    //<media:content url="http://fc02.deviantart.net/fs70/i/2011/074/4/5/poinson_frogs_by_greenestreet-d3bp5bu.jpg" height="484" width="900" medium="image"/>
                    var fullImageUrl = results.Substring(results.IndexOf("<media:content url=\"") + 20);
                    fullImageUrl = fullImageUrl.Substring(0, fullImageUrl.IndexOf("\""));
                    
                    if (!fullImageUrl.ToLower().EndsWith(".jpg") &&
                        !fullImageUrl.ToLower().EndsWith(".png")) continue;

                    string thumbnail; 
                    if (wholeItem.Contains("<media:thumbnail url=\""))
                    {
                        //<media:thumbnail url="http://th06.deviantart.net/fs71/150/f/2013/344/e/b/dubstep_girl_by_mrpyros-d6xfbik.png" height="60" width="150"/>
                        thumbnail = results.Substring(results.IndexOf("<media:thumbnail url=\"") + 22);
                        thumbnail = thumbnail.Substring(0, thumbnail.IndexOf("\""));
                    }
                    else
                    {
                        thumbnail = fullImageUrl; 
                    }

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

        public void UpdateDownloads()
        {
            GetDownloaded(); 
        }
    }
}