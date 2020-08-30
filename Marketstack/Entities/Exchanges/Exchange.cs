using Newtonsoft.Json;

namespace Marketstack.Entities.Exchanges
{
    public class Exchange
    {
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Mic { get; set; }
        public string Country { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string Website { get; set; }
        public Currency Currency { get; set; }
    }
}