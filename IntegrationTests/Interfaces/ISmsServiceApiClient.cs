using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public interface ISmsServiceApiClient
    {
        [Post("/sms")]
        [Headers("Content-Type: application/json")]
        Task<HttpResponseMessage> GetSMSPartsAsync([Body] string inputString);
    }
}
