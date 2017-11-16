using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Trady.Importer.Csv
{
    public class CsvImportConfiguration
    {
        public string Delimiter { get; set; } = ",";
        public string DateFormat { get; set; }
        public CultureInfo Culture { get; set; }
        public bool HasHeaderRecord { get; set; } = true;
    }
}
