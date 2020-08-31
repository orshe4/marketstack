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

namespace Marketstack.Services
{
    public class MarketstackService : IMarketstackService
    {
        private readonly MarketstackOptions _options;        
        private readonly HttpClient _httpClient;
        private readonly Throttled _throttled;
        private readonly ILogger<MarketstackService> _logger;

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
            _logger = logger;                        
        }

        public MarketstackService(IOptions<MarketstackOptions> options, ILogger<MarketstackService> logger) 
            : this(options, new HttpClient(), logger)
        {
        }
        
        public IAsyncEnumerable<Exchange> GetExchanges()
        {            
            return _httpClient.GetAsync<Exchange>("http://api.marketstack.com/v1/exchanges", _options.ApiToken, _throttled);
        }

        public IAsyncEnumerable<Stock> GetExchangeStocks(string exchangeMic)
        {
            return _httpClient.GetAsync<Stock>($"http://api.marketstack.com/v1/tickers?exchange={exchangeMic}", _options.ApiToken, _throttled);                                
        }

        public IAsyncEnumerable<StockBar> GetStockEodBars(string stockSymbol, DateTime fromDate, DateTime toDate)
        {
            string dateFromStr = fromDate.ToString("yyyy-MM-dd");
            string dateToStr = toDate.ToString("yyyy-MM-dd");
            return _httpClient.GetAsync<StockBar>($"http://api.marketstack.com/v1/eod?symbols={stockSymbol}&date_from={dateFromStr}&date_to={dateToStr}", _options.ApiToken, _throttled);
        }
    }
}
