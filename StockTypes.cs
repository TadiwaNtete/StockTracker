using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Names
{
    public class StockTypes
    {
        [Name("Symbol")]
        public string Symbol { get; set; }
        [Name("Name")]
        public string Name { get; set; }
    }
}
