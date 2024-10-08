﻿using Newtonsoft.Json;

namespace EquityX.DTO
{
    public class QuoteResponse
    {
        [JsonProperty("result")]
        public List<QuoteResult> Result { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }

    public class QuoteResult
    {
        public string language { get; set; }
        public string region { get; set; }
        public string quoteType { get; set; }
        public bool triggerable { get; set; }
        public string quoteSourceName { get; set; }
        public string currency { get; set; }
        public string fullExchangeName { get; set; }
        public string longName { get; set; }
        public string financialCurrency { get; set; }
        public long averageDailyVolume3Month { get; set; }
        public long averageDailyVolume10Day { get; set; }
        public decimal fiftyTwoWeekLowChange { get; set; }
        public decimal fiftyTwoWeekLowChangePercent { get; set; }
        public string fiftyTwoWeekRange { get; set; }
        public decimal fiftyTwoWeekHighChange { get; set; }
        public decimal fiftyTwoWeekHighChangePercent { get; set; }
        public decimal fiftyTwoWeekLow { get; set; }
        public decimal fiftyTwoWeekHigh { get; set; }
        public long dividendDate { get; set; }
        public decimal bookValue { get; set; }
        public decimal fiftyDayAverage { get; set; }
        public decimal fiftyDayAverageChange { get; set; }
        public decimal fiftyDayAverageChangePercent { get; set; }
        public decimal twoHundredDayAverage { get; set; }
        public decimal twoHundredDayAverageChange { get; set; }
        public decimal twoHundredDayAverageChangePercent { get; set; }
        public object marketCap { get; set; }
        public decimal forwardPE { get; set; }
        public decimal priceToBook { get; set; }
        public long sourceInterval { get; set; }
        public string exchangeTimezoneName { get; set; }
        public string exchangeTimezoneShortName { get; set; }
        public long exchangeDataDelayedBy { get; set; }
        public string market { get; set; }
        public string shortName { get; set; }
        public decimal preMarketPrice { get; set; }
        public decimal regularMarketChangePercent { get; set; }
        public string regularMarketDayRange { get; set; }
        public decimal regularMarketPreviousClose { get; set; }
        public decimal bid { get; set; }
        public decimal ask { get; set; }
        public int bidSize { get; set; }
        public int askSize { get; set; }
        public string messageBoardId { get; set; }
        public string marketState { get; set; }
        public long earningsTimestamp { get; set; }
        public long earningsTimestampStart { get; set; }
        public long earningsTimestampEnd { get; set; }
        public decimal trailingAnnualDividendRate { get; set; }
        public decimal trailingPE { get; set; }
        public decimal trailingAnnualDividendYield { get; set; }
        public decimal epsTrailingTwelveMonths { get; set; }
        public decimal epsForward { get; set; }
        public decimal epsCurrentYear { get; set; }
        public decimal priceEpsCurrentYear { get; set; }
        public object sharesOutstanding { get; set; }
        public bool esgPopulated { get; set; }
        public bool tradeable { get; set; }
        public long gmtOffSetMilliseconds { get; set; }
        public int priceHint { get; set; }
        public decimal preMarketChange { get; set; }
        public decimal preMarketChangePercent { get; set; }
        public long preMarketTime { get; set; }
        public string exchange { get; set; }
        public decimal regularMarketPrice { get; set; }
        public long regularMarketTime { get; set; }
        public decimal regularMarketChange { get; set; }
        public decimal regularMarketOpen { get; set; }
        public decimal regularMarketDayHigh { get; set; }
        public decimal regularMarketDayLow { get; set; }
        public long regularMarketVolume { get; set; }
        public string symbol { get; set; }
    }

    public class QuoteDTO
    {
        [JsonProperty("quoteResponse")]
        public QuoteResponse QuoteResponse { get; set; }
    }
}
