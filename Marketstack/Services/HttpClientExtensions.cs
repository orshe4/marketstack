using Marketstack.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Throttling;

namespace Marketstack.Services
{
    internal static class HttpClientExtensions
    {
        public static async IAsyncEnumerable<T> GetAsync<T>(this HttpClient httpClient, string url, string apiToken, Throttled throttled)
        {
            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["access_key"] = apiToken;
            builder.Query = query.ToString();
            var pageResponse = await throttled.Run( () => httpClient.GetNextPageResponse<T>(builder, null));

            while (pageResponse != null)
            {
                foreach (var item in pageResponse.Data)
                {
                    yield return item;
                }

                pageResponse = await throttled.Run(() => httpClient.GetNextPageResponse(builder, pageResponse));
            }
        }

        private static async Task<PageResponse<T>> GetNextPageResponse<T>(this HttpClient httpClient, UriBuilder builder, PageResponse<T> lastPageResponse)
        {
            if (lastPageResponse != null)
            {
                if (lastPageResponse.IsLastResponse)
                {
                    return null;
                }

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["offset"] = lastPageResponse.NextOffset.ToString();
                builder.Query = query.ToString();
            }


            using Stream s = await httpClient.GetStreamAsync(builder.Uri);
            using StreamReader sr = new StreamReader(s);
            using JsonReader reader = new JsonTextReader(sr);
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<PageResponse<T>>(reader);

        }
    }
}
