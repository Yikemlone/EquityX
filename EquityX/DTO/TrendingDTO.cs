using Newtonsoft.Json;

namespace EquityX.DTO
{
    // These clasess were generated using https://json2csharp.com/json-to-csharp
    public class Finance
    {
        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("result")]
        public List<FinanceResult> Result { get; set; }
    }

    public class TrendingQuote
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
        public List<TrendingQuote> Quotes { get; set; }

        [JsonProperty("startInterval")]
        public long StartInterval { get; set; }
    }

    public class TrendingDTO
    {
        [JsonProperty("finance")]
        public Finance Finance { get; set; }
    }
}
