using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public static class Helper
    {
        public static async Task<T> ReadBodyFromHttpResponseMessage<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Failed to read body as it doesn't have a success status code. \nStatusCode: '{response.StatusCode}', Content: '{await response.Content.ReadAsStringAsync()}'");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static TApiClient CreateApiClient<TApiClient>()
        {
            string endpointUrl = Constants.DefaultEnv;
            Console.WriteLine($"Running with url: {endpointUrl}");
            return RestService.For<TApiClient>(endpointUrl);
        }

    }
}
