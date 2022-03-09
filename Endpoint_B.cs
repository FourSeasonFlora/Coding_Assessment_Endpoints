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
            var evenResults = new EvenResult();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, int[]>>(requestBody);

            evenResults.sortedNumbers = data["numbers"];
            evenResults.evenSum = data["numbers"].Where(num => num % 2 == 0).Sum();
            evenResults.evenAvg = data["numbers"].Sum() / data["numbers"].Count();

            return new OkObjectResult(JsonSerializer.Serialize(evenResults));
        }
    }
}
