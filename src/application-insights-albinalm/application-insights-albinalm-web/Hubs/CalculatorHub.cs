using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace application_insights_albinalm_web.Hubs
{
    public class CalculatorHub : Hub
    {
        public async Task Calculate(string mathformula)
        {
            Debug.WriteLine("Calculating!");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://moln-tenta-albinalm-function-calculate.azurewebsites.net/api/Calculate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string result;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                string json = "{\"input\":" + "\"" + mathformula + "\"" + "}";
                Debug.WriteLine(json);
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
          
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = JsonConvert.DeserializeObject<string>(streamReader.ReadToEnd());

            }
            await Clients.Caller.SendAsync("OnCalculationCompleted", result);
        }
    }
}
