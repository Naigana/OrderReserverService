using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderDeliveryServiceFunction
{
    public static class CreateDeliveryItem
    {
        private const string _databaseId = "OrderDelivery";
        private const string _containerId = "Orders";

        [FunctionName("CreateDeliveryItem")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
            databaseName: _databaseId,
            containerName: _containerId,
            Connection = "CosmosStorage")] IAsyncCollector<object> taskItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var order = JsonConvert.DeserializeObject<OrderForDelivery>(requestBody);

           
            await taskItems.AddAsync(
                new
                {
                    id = Guid.NewGuid().ToString(),
                    order.OrderId,
                    order.ShippingAddress,
                    order.Items,
                    order.FinalPrice
                });

            return new OkObjectResult("Order created successfully");
        }
    }
}
