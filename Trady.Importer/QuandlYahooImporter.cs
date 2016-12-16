using Quandl.NET;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Importer
{
    public class QuandlYahooImporter : QuandlImporterBase
    {
        public QuandlYahooImporter(string apiKey) : base(apiKey)
        {
        }

        protected override string DatabaseCode => "YAHOO";
    }
}
