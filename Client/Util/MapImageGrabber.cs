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
        // zoom = -1 - auto
        public async Task<BitmapImage> GetImage(int zoom, int[] numbers, params ViolationDto[] violations) {
            if (numbers.Length != violations.Length) {
                throw new Exception("Array lengths do not match");
            }

            List<ViolationDto> violationsList = violations.ToList();
            IMapQueryBuilder localBuilder = mapUriBuilder;
            for (int i = 0; i < violationsList.Count; i++) {
                ViolationDto violation = violationsList[i];
                if (violation.latitude == null || violation.longitude == null) {
                    throw new Exception("Both coordinates must be not null");
                }

                if (numbers[i] < 1 || numbers[i] > 99) {
                    throw new Exception("Number must be bigger than 1 and less than 99");
                }

                localBuilder = localBuilder
                    .WithPointer(violation.longitude.Value, violation.latitude.Value, numbers[i].ToString());
            }
            if (zoom != -1) {
                localBuilder = localBuilder.WithZoom(zoom);
            }

            Uri query = localBuilder.Build();
            HttpResponseMessage response = (await httpClient.GetAsync(query));
            byte[] img = null;
            if (response.IsSuccessStatusCode) {
                 img = await response.Content.ReadAsByteArrayAsync();
                return Utility.LoadImage(img);
            } else {
                return null;
            }
        }
    }
}
