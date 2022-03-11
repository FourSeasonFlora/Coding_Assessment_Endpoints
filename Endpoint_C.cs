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
            // Endpoint_C is called from our console app, and passed in the RequestMessage as our HttpRequest req

            // read the RequestMessage from the Body of the HttpRequest and read to the end to grab all the data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // make new client for sending requests to endpoints
            var client = new HttpClient();

            // make new RequestMessage to send to endpoint with our client
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:7071/api/Endpoint_A");// add the URI for Endpoint_A
            request.Content = new StringContent(requestBody); // send the requestBody, which is still a Dictionary that is searialized as a JSON string

            // await the response from Endpoint_A
            var response = await client.PostAsync(request.RequestUri, request.Content);

            // read our response, which is a Dictionary with a sorted Array, but still in JSON string format
            var dictWithSortedArray = response.Content.ReadAsStringAsync().Result;

            // make new RequestMessage to send to endpoint with our client
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:7071/api/Endpoint_B");// add the URI for Endpoint_B
            request.Content = new StringContent(dictWithSortedArray); // send Dictionary which is still in JSON format to Endpoint_B

            // await the response from Endpoint_B
            response = await client.PostAsync(request.RequestUri, request.Content);

            // return response content back to Console App for printing to screen
            return new OkObjectResult(response.Content.ReadAsStringAsync().Result);
        }
    }
}
