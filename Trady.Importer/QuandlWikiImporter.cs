using Quandl.NET;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer.Helper;

namespace Trady.Importer
{
    public class QuandlWikiImporter : QuandlImporterBase
    {
        public QuandlWikiImporter(string apiKey) : base(apiKey)
        {
        }

        protected override string DatabaseCode => "WIKI";
    }
}
