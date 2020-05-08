using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Util.UrlBuilder {
    public class MapQueryBuilder : IMapQueryBuilder {

        private static readonly string POINTER_STYLE = "pm2";
        private static readonly string POINTER_COLOR = "rd";
        private static readonly string POINTER_SIZE = "m";
        private static readonly Size SIZE = new Size(650, 450);
        private readonly string BASE_URI;

        private UriBuilder uri;

        public MapQueryBuilder(string baseUri) {
            uri = new UriBuilder(baseUri);
            uri.Query += "l=map";
            AppendParameter($"size={SIZE}");
            BASE_URI = uri.ToString();
        }

        public Uri Build() {
            Uri cur = uri.Uri;
            uri = new UriBuilder(BASE_URI);
            return cur;
        }

        public IMapQueryBuilder WithPointer(double coordN, double coordE, string content) {
            if (!CanAppendPointer()) {
                throw new Exception("Pointer cannot be appended");
            }
            
            if (uri.Query.IndexOf("&pt") == -1) {
                AppendParameter($"pt={coordN},{coordE},{POINTER_STYLE}{POINTER_COLOR}{POINTER_SIZE}{content}");
            } else {
                Append($"~{coordN},{coordE},{POINTER_STYLE}{POINTER_COLOR}{POINTER_SIZE}{content}");
            }
            return this;
        }

        public IMapQueryBuilder WithZoom(int zoom) {
            if (zoom < 0 || zoom > 23) {
                throw new Exception("Size should be within 0 and 23");
            }

            AppendParameter($"z={zoom}");
            return this;
        }

        private void AppendParameter(string content) {
            Append("&" + content);
        }

        private void Append(string content) {
            uri.Query = uri.Query.Substring(1) + content;
        }

        private bool CanAppendPointer() {
            int indexOfPtParam = uri.Query.IndexOf("&pt");
            if (indexOfPtParam == -1) {
                return true;
            } else {
                return indexOfPtParam == uri.Query.LastIndexOf("&pt");
            }
        }
    }
}
