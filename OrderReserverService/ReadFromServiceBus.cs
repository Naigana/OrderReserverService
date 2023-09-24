using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Configuration;

namespace OrderReserverService
{
    public class ReadFromServiceBus
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        [FunctionName("OrderItemsReserver")]
        public static async Task Run(
        [ServiceBusTrigger("paid-orders", "reserver-handler-func", Connection = "ServiceBusConnection")] string payload,
        string messageId,
        [Blob("orders-reservation/{messageId}.json", FileAccess.Write, Connection = "AzureWebJobsStorage")]
        Stream output,
        ILogger log)
        {
            try
            {
                //throw new Exception("Test Applogic");
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);               
                await output.WriteAsync(payloadBytes);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while uploading the file to Blob Storage.");

                var logicAppUrl = Environment.GetEnvironmentVariable("LogicAppConnection");

                var jsonBody = "{\"documentid\":" + $"\"{messageId}\""+"}";
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(logicAppUrl, content);
            }
        }
    }
}