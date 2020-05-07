using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Util.UrlBuilder {
    public interface IMapQueryBuilder {
        Uri Build();

        IMapQueryBuilder WithPointer(double coordN, double coordE, string content);

        // size 0-23
        IMapQueryBuilder WithZoom(int size);

    }
}
