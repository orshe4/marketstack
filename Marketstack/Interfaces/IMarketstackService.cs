using Marketstack.Entities.Exchanges;
using Marketstack.Entities.Stocks;
using System;
using System.Collections.Generic;

namespace Marketstack.Interfaces
{
    public interface IMarketstackService
    {
        IAsyncEnumerable<Exchange> GetExchanges();
        IAsyncEnumerable<Stock> GetExchangeStocks(string exchangeMic);
        IAsyncEnumerable<StockBar> GetStockEodBars(string stockSymbol, DateTime fromDate, DateTime toDate);
        IAsyncEnumerable<StockBar> GetStockIntraDayBars(string stockSymbol, DateTime fromDate, DateTime toDate);
    }
}
