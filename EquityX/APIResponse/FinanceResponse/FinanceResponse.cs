namespace EquityX.APIResponse.FinanceResponse
{
    // These clasess were generated using https://json2csharp.com/json-to-csharp
    public class Finance
    {
        public object error { get; set; }
        public List<Result> result { get; set; }
    }

    public class Quote
    {
        public string symbol { get; set; }
    }

    public class Result
    {
        public int count { get; set; }
        public long jobTimestamp { get; set; }
        public List<Quote> quotes { get; set; }
        public long startInterval { get; set; }
    }

    public class Root
    {
        public Finance finance { get; set; }
    }
}
