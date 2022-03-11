using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace Coding_Assessment_Endpoints
{
    public static class Endpoint_B
    {
        [FunctionName("Endpoint_B")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Endpoint_B is called from our Endpoint_C, and passed in the RequestMessage as our HttpRequest req

            // make new EvenResult object from our custom built class
            var evenResults = new EvenResult();

            // read the RequestMessage from the Body of the HttpRequest and read to the end to grab all the data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // desearialize requestBody from a JSON string back into a Dictionary
            var dictWithSortedArray = JsonSerializer.Deserialize<Dictionary<string, int[]>>(requestBody);

            // grab sorted array from our Dictionary and assign it to the sortedNumbers property of our class
            evenResults.sortedNumbers = dictWithSortedArray["numbers"];

            // sum the even numbers of our sorted array and assign the value to the evenSum property of our class
            evenResults.evenSum = dictWithSortedArray["numbers"].Where(num => num % 2 == 0).Sum();

            // get the average of the sorted array and assign the value to our evenAvg property of our class
            evenResults.evenAvg = dictWithSortedArray["numbers"].Sum() / dictWithSortedArray["numbers"].Count();

            // searialize our evenResults object into a JSON string, and send back as a response
            return new OkObjectResult(JsonSerializer.Serialize(evenResults));
        }
    }
}
