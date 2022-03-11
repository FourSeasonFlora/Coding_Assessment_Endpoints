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
            // Endpoint_A is called from our Endpoint_C, and passed in the RequestMessage as our HttpRequest req

            // read the RequestMessage from the Body of the HttpRequest and read to the end to grab all the data
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // desearialize requestBody from a JSON string back into a Dictionary
            var data = JsonSerializer.Deserialize<Dictionary<string, int[]>>(requestBody);

            // sort the array in the Dictionary, and set the value of the Dictionary to the newly sorted array
            data["numbers"] = data["numbers"].OrderByDescending(num => num).ToArray();

            // searialize the Dictionary back into a JSON string and send back Endpoint_C as a response
            return new OkObjectResult(JsonSerializer.Serialize(data));
        }
    }
}
