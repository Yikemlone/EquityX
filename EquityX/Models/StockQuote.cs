using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquityX.Models
{
    public class StockQuote
    {
        public string symbol { get; set; }
    }

    public class StockResult
    {
        public int count { get; set; }
        public List<StockQuote> quotes { get; set; }
        public long jobTimestamp { get; set; }
        public int startInterval { get; set; }
    }

    public class StockFinance
    {
        public List<StockResult> result { get; set; }
        public object error { get; set; }
    }

    public class StockRoot
    {
        public StockFinance finance { get; set; }
    }
}
