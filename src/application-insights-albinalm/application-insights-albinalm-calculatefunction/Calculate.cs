using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace application_insights_albinalm_calculatefunction
{
  
   
    public class Calculate
    {
        private readonly TelemetryClient telemetryClient;
        public Calculate(TelemetryConfiguration configuration)
        {
            telemetryClient = new TelemetryClient(configuration);
        }
        [FunctionName("Calculate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string input = req.Query["input"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            double result = 0;
            input = input ?? data?.input;
          

            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    result = Convert.ToDouble(new DataTable().Compute(input, null));
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    log.LogInformation("The operation failed with the following message: " + ex.Message);
                    telemetryClient.TrackException(ex);
                    return new BadRequestObjectResult(ex.Message);

                }
            }
            else
            {
                log.LogInformation("Process failed. Input data was incorrect");
               
                telemetryClient.TrackTrace("Input data was is incorrect format. Make sure you have an input and accountId parameter.");
                return new BadRequestObjectResult("Input data was is incorrect format. Make sure you have an input and accountId parameter.");
            }
        }
    }
}
