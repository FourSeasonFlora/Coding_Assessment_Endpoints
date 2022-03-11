using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Coding_Assessment_Endpoints
{
    public static class Endpoint_C
    {
        [FunctionName("Endpoint_C")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var client = new HttpClient();

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:7071/api/Endpoint_A");
            request.Content = new StringContent(requestBody); 
            
            var response = await client.PostAsync(request.RequestUri, request.Content);

            
            var dictWithSortedArray = response.Content.ReadAsStringAsync().Result;

            
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:7071/api/Endpoint_B");
            request.Content = new StringContent(dictWithSortedArray); 

            
            response = await client.PostAsync(request.RequestUri, request.Content);

            
            return new OkObjectResult(response.Content.ReadAsStringAsync().Result);
        }
    }
}
