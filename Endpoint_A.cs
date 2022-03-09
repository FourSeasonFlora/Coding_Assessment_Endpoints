using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace Coding_Assessment_Endpoints
{
    public static class Endpoint_A
    {
        [FunctionName("Endpoint_A")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, int[]>>(requestBody);

            var sortedArray = data["numbers"].OrderByDescending(num => num).ToArray();
            data["numbers"] = sortedArray;

            return new OkObjectResult(JsonSerializer.Serialize(data));
        }
    }
}
