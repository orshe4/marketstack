using Marketstack.Entities.Exchanges;
using Marketstack.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Marketstack.Entities.Stocks;
using System.Linq;
using Throttling;
using System.Threading.Tasks;
using Marketstack.Entities.Enums;

namespace Marketstack.Services
{
    public class MarketstackService : IMarketstackService
    {
        private readonly MarketstackOptions _options;        
        private readonly HttpClient _httpClient;
        private readonly Throttled _throttled;
        private readonly ILogger<MarketstackService> _logger;
        private readonly string _apiUrl;
        public MarketstackService(IOptions<MarketstackOptions> options,
                                  HttpClient httpClient,
                                  ILogger<MarketstackService> logger)
        {
            _options = options.Value;
            if(_options.MaxRequestsPerSecond >= 10)
            {
                _throttled = new Throttled(_options.MaxRequestsPerSecond / 10, 100);
            }
            else
            {
                _throttled = new Throttled(_options.MaxRequestsPerSecond, 1000);
            }
            
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _apiUrl = options.Value.Https ? "https://api.marketstack.com/v1" : "http://api.marketstack.com/v1";
            _logger = logger;                        
        }

        public MarketstackService(IOptions<MarketstackOptions> options, ILogger<MarketstackService> logger) 
            : this(options, new HttpClient(), logger)
        {
        }
        
        public Task<List<Exchange>> GetExchanges()
        {            
            return _httpClient.GetAsync<Exchange>($"{_apiUrl}/exchanges", _options.ApiToken, _throttled);
        }

        public Task<List<Stock>> GetExchangeStocks(string exchangeMic)
        {
            return _httpClient.GetAsync<Stock>($"{_apiUrl}/tickers?exchange={exchangeMic}", _options.ApiToken, _throttled);                                
        }

        public Task<List<StockBar>> GetStockEodBars(string stockSymbol, DateTime fromDate, DateTime toDate)
        {
            string dateFromStr = fromDate.ToString("yyyy-MM-dd");
            string dateToStr = toDate.ToString("yyyy-MM-dd");
            return _httpClient.GetAsync<StockBar>($"{_apiUrl}/eod?symbols={stockSymbol}&date_from={dateFromStr}&date_to={dateToStr}", _options.ApiToken, _throttled);
        }
        public Task<List<StockBar>> GetStockIntraDayBars(string stockSymbol, DateTime fromDate, DateTime toDate, Interval interval = Interval._1hour)
        {
            string dateFromStr = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
            string dateToStr = toDate.ToString("yyyy-MM-dd HH:mm:ss");
            return _httpClient.GetAsync<StockBar>($"{_apiUrl}/intraday?symbols={stockSymbol}&interval={interval.GetDescription()}&date_from={dateFromStr}&date_to={dateToStr}", _options.ApiToken, _throttled);
        }
    }
}
