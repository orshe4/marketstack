# marketstack API

[marketstack](https://marketstack.com) is a JSON-based REST API for real-time, intraday and historical stock market data, supporting 125,000+ stock ticker symbols from 72+ worldwide stock exchanges. The API was built upon powerful apilayer cloud infrastructure, handling any volume with ease - from a few API requests per day all the way to millions of calls per minute.

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

## Supported Stock Market Data

* **125.000+ Stock Tickers**
* **72+ Stock Exchanges**
* **50+ Countries**

## In-depth Documentation

An in-depth API documentation, including interactive code examples and extensive descriptions can be found at [marketstack.com/documentation](https://marketstack.com/documentation)

## Usage

**Initialize MarketstackService:**
```c#
    var options = Options.Create(new MarketstackOptions() { ApiToken = "apiToken"});
    var marketstackService = new MarketstackService(options, NullLogger<MarketstackService>.Instance);    
```

**GetExchanges:**
```c#
    var exchanges = await marketstackService.GetExchanges().ToListAsync();
```

**GetExchangeStocks:**
```c#
    var nasdaqMic = "XNAS";
    var stocks = await marketstackService.GetExchangeStocks(nasdaqMic)
                .Take(400)
                .ToListAsync();
```    
    
**GetStockEodBars:**
```c#
    var appleSymbol = "AAPL";
    var fromDate = DateTime.Now.AddDays(-200);
    var toDate = DateTime.Now;
    var bars = await marketstackService.GetStockEodBars(appleSymbol, fromDate, toDate)
        .ToListAsync();       
```

## Tests

Before running the tests, the ApiToken should be set using: 

```
setx ASPNETCORE_MarketstackApiToken {Your_Api_Token} /M
```

## Legal

All usage of the marketstack website, API, and services is subject to the [marketstack Terms & Conditions](https://marketstack.com/terms) and all annexed legal documents and agreements.
