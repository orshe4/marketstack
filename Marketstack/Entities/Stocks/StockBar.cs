using Newtonsoft.Json;
using System;

namespace Marketstack.Entities.Stocks
{
    public class StockBar
    {
        public float? Open { get; set; }
        public float? High { get; set; }
        public float? Low { get; set; }
        public float? Close { get; set; }
        public float? Volume { get; set; }

        [JsonProperty("adj_high")]
        public float? AdjHigh { get; set; }

        [JsonProperty("adj_low")]
        public float? AdjLow { get; set; }

        [JsonProperty("adj_close")]
        public float? adjClose { get; set; }

        [JsonProperty("adj_open")]
        public float? AdjOpen { get; set; }

        [JsonProperty("adj_volume")]
        public float? AdjVolume { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public DateTime Date { get; set; }
    }
}