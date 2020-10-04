using Marketstack.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Throttling;

namespace Marketstack.Services
{
    internal static class HttpClientExtensions
    {
        public static async Task<List<T>> GetAsync<T>(this HttpClient httpClient, string url, string apiToken, Throttled throttled, int limit = PageResponse.MaxLimit)
        {
            if(limit > PageResponse.MaxLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), $"Maximum allowed limit value is {PageResponse.MaxLimit}");
            }

            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["access_key"] = apiToken;
            builder.Query = query.ToString();
            var firstResponse = await throttled.Run(() => httpClient.GetPageResponse<T>(builder, limit));
            var offsets = firstResponse.AllRequestOffsets();            
            var tasks = offsets.Select(async (offset) => await throttled.Run(() => httpClient.GetPageResponse<T>(new UriBuilder(builder.Uri), limit, offset)));
            var pages = await Task.WhenAll(tasks);
            var data = pages.SelectMany(page => page.Data).ToList();
            data.AddRange(firstResponse.Data);
            return data;
        }

        private static async Task<PageResponse<T>> GetPageResponse<T>(this HttpClient httpClient, UriBuilder builder, int limit, int offset = 0)
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["offset"] = offset.ToString();
            query["limit"] = limit.ToString();
            builder.Query = query.ToString();

            using Stream s = await httpClient.GetStreamAsync(builder.Uri);
            using StreamReader sr = new StreamReader(s);
            using JsonReader reader = new JsonTextReader(sr);
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<PageResponse<T>>(reader);
        }
    }
}
