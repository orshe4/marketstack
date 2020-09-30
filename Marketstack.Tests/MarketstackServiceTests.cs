using Marketstack.Interfaces;
using Marketstack.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Marketstack.Tests
{
    public class MarketstackServiceTests
    {
        private readonly IMarketstackService _marketstackService;
        private const string apiKeyVariable = "ASPNETCORE_MarketstackApiToken";
        public MarketstackServiceTests()
        {
            var apiKey = Environment.GetEnvironmentVariable(apiKeyVariable, EnvironmentVariableTarget.Machine);
            var options = Options.Create(new MarketstackOptions() { ApiToken = apiKey, MaxRequestsPerSecond = 3, Https = true });
            _marketstackService = new MarketstackService(options, NullLogger<MarketstackService>.Instance);
        }

        [Fact]
        public async Task GetExchanges_ReturnsExchanges()
        {
            var exchanges = await _marketstackService.GetExchanges();
            Assert.NotEmpty(exchanges);
        }

        [Fact]
        public async Task GetExchangeStocks_ReturnsStocks()
        {
            var nasdaqMic = "XNAS";
            var stocks = await _marketstackService.GetExchangeStocks(nasdaqMic);                
            Assert.True(stocks.Count > 1000);
        }

        [Fact]
        public async Task GetStockEodBars_ReturnsBars()
        {
            var appleSymbol = "AAPL";
            var fromDate = new DateTime(2017, 1, 1);
            var toDate = DateTime.Now;
            var bars = await _marketstackService.GetStockEodBars(appleSymbol, fromDate, toDate);                
            Assert.NotEmpty(bars);
            var distinctDates = bars.Select(b => b.Date).Distinct().ToList();            
            Assert.Equal(distinctDates.Count, bars.Count);
            Assert.True(bars.Count > 100, "Not enough bars");
        }

        [Fact]
        public async Task GetStockEodBars_Parallel_ReturnsBars()
        {
            // 10 stocks
            List<string> symbols = new List<string>() { "AAPL", "MSFT", "GOOG", "VOD", "NVDA", "NFLX", "PEP", "NOW", "VEEV", "MOH" };
            var fromDate = new DateTime(2017, 9, 1);
            var toDate = DateTime.Now;
            var tasks = symbols.Select(async (symbol) => await _marketstackService.GetStockEodBars(symbol, fromDate, toDate));
            var stocksBars = await Task.WhenAll(tasks);

            Assert.True(symbols.Count == stocksBars.Length);
        }

        [Fact]
        public async Task GetStockIntraydayBars_ReturnsBars()
        {
            var appleSymbol = "AAPL";
            var fromDate = DateTime.Now.AddDays(-5);
            var toDate = DateTime.Now;
            var bars = await _marketstackService.GetStockIntraDayBars(appleSymbol, fromDate, toDate);                
            Assert.NotEmpty(bars);
            Assert.True(bars.Count > 10);
        }
    }
}
