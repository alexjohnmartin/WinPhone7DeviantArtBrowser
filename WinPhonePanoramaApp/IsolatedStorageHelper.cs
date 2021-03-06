using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;

namespace WinPhonePanoramaApp
{
    public class IsolatedStorageHelper
    {
        public static BitmapImage GetImage(string imageName)
        {
            var bimg = new BitmapImage();
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = iso.OpenFile(@"Shared\ShellContent\" + imageName, FileMode.Open, FileAccess.Read))
                {
                    bimg.SetSource(stream);
                }
            }
            return bimg;
        }

        public static void DeleteImage(string imageName)
        {
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                iso.DeleteFile(@"Shared\ShellContent\" + imageName);
            }
        }

        public static IEnumerable<string> GetImageFilenames()
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (var filename in iso.GetFileNames(@"Shared\ShellContent\*"))
                {
                    if (filename.EndsWith(".jpg") || filename.EndsWith(".png"))
                        yield return filename;
                }
            }
        }

        public static bool DoesFileExist(string imageName)
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return iso.FileExists(@"Shared\ShellContent\" + imageName);
            }
        }

        public static void SaveToJpeg(Stream stream, string imageName)
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!iso.DirectoryExists(@"Shared\ShellContent"))
                {
                    iso.CreateDirectory(@"Shared\ShellContent");
                }
            }

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isostream = iso.CreateFile(@"Shared\ShellContent\" + imageName))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(stream);
                    WriteableBitmap wb = new WriteableBitmap(bitmap);
                    // Encode WriteableBitmap object to a JPEG stream. 
                    Extensions.SaveJpeg(wb, isostream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                    isostream.Close();

                    ShowToast("Image downloaded");
                }
            }
        }

        private static string GetFilenameFromUrl(string imageUrl)
        {
            if (!imageUrl.Contains("/")) return imageUrl;
            return imageUrl.Substring(imageUrl.LastIndexOf('/') + 1);
        }

        private static void ShowToast(string message)
        {
            ShellToast toast = new ShellToast();
            toast.Title = "Downloaded";
            toast.Content = message;
            toast.Show();
        }
    }
}