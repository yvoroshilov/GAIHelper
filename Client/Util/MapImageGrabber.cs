using Client.MainService;
using Client.Util.UrlBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.Util {
    public class MapImageGrabber {

        private static readonly string BASE_URL = "https://static-maps.yandex.ru/1.x/";
        private readonly IMapQueryBuilder mapUriBuilder;

        private readonly HttpClient httpClient = new HttpClient();

        public MapImageGrabber() {
            mapUriBuilder = new MapQueryBuilder(BASE_URL);

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BASE_URL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("image/jpg"));
        }

        public async Task<BitmapImage> GetImage(ViolationDto violation, int zoom = 16) {
            if (violation.locationN == null || violation.locationE == null) {
                throw new Exception("Both coordinates must be not null");
            }

            Uri query = mapUriBuilder
                .WithPointer(violation.locationN.Value, violation.locationE.Value, "9")
                .WithZoom(zoom)
                .Build();
            HttpResponseMessage response = (await httpClient.GetAsync(query));
            byte[] img = null;
            if (response.IsSuccessStatusCode) {
                 img = response.Content.ReadAsByteArrayAsync().Result;
            }
            return LoadImage(img);
        }

        private BitmapImage LoadImage(byte[] imageData) {
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
    }
}
