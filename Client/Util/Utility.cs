using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client.Util {
    public static class Utility {

        public static BitmapImage NoPhotoImg;

        static Utility() {
            NoPhotoImg = GetNoPhoto();
        }

        public static object GetDefault(Type type) {
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            } else if (type.Equals(typeof(DateTime))) {
                return default(DateTime);
            } else {
                return null;
            }
        }

        public static BitmapImage LoadImage(byte[] imageData) {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData)) {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private static BitmapImage GetNoPhoto() {
            var myResourceDictionary = new ResourceDictionary();
    myResourceDictionary.Source =
            new Uri("/Client;component/Resources/Dictionary.xaml",
                UriKind.RelativeOrAbsolute);
            return myResourceDictionary["SamplePersonImage"] as BitmapImage;
        }
    }
}
