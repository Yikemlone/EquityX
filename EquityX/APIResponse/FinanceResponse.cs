
/* Unmerged change from project 'EquityX (net7.0-android)'
Before:
using Newtonsoft.Json;
After:
using EquityX;
using EquityX.APIResponse;
using EquityX.APIResponse;
using EquityX.APIResponse.FinanceResponse;
using Newtonsoft.Json;
*/
using Newtonsoft.Json;

namespace EquityX.APIResponse
{
    // These clasess were generated using https://json2csharp.com/json-to-csharp
    public class Finance
    {
        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("result")]
        public List<FinanceResult> Result { get; set; }
    }

    public class Quote
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }

    public class FinanceResult
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("jobTimestamp")]
        public long JobTimestamp { get; set; }

        [JsonProperty("quotes")]
        public List<Quote> Quotes { get; set; }

        [JsonProperty("startInterval")]
        public long StartInterval { get; set; }
    }

    public class FinanceRoot
    {
        [JsonProperty("finance")]
        public Finance Finance { get; set; }
    }
}
