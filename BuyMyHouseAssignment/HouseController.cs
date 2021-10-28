using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;
using Service;
using DAL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Reflection;

namespace BuyMyHouseAssignment
{
    public class HouseController
    {
        private readonly IHouseService HouseService;

        public HouseController(IHouseService HouseService)
        {
            this.HouseService = HouseService;
        }
       
        [FunctionName("POST_House")]
        public async Task<IActionResult> POST_House([HttpTrigger(AuthorizationLevel.Function, "post", Route = "house")] HttpRequest req, ILogger log,ExecutionContext context)
        {
            log.LogInformation("/house GET has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            

            if (HouseService.VALIDATE_House(requestBody))
            {
                House newHouse = JsonConvert.DeserializeObject<House>(requestBody);
                HouseService.POST_House(newHouse);
            }

            return new OkObjectResult("POST House");
        }

        [FunctionName("GET_House")]
        public async Task<IActionResult> GET_House([HttpTrigger(AuthorizationLevel.Function, "get", Route = "house")] HttpRequest req, ILogger log)
        {
            log.LogInformation("/house GET has been requested.");

            //Get all Houses

            string responseMessage = "Thanks for supplying a valid House model";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("DELETE_House")]
        public async Task<IActionResult> DELETE_House([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "house/{id:int}")] HttpRequest req, ILogger log, int id)
        {
            log.LogInformation("/house DELETE has been requested.");

            //Delete selected house

            string responseMessage = "Thanks for supplying a valid House model" +id;

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("PUT_House")]
        public async Task<IActionResult> PUT_House([HttpTrigger(AuthorizationLevel.Function, "put", Route = "house/{id:int}")] HttpRequest req, ILogger log, int id)
        {
            log.LogInformation("/house DELETE has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (!houseSerializer.validateHouse(requestBody))
                return new BadRequestObjectResult("Invalid house object");

            //Update House

            string responseMessage = "Thanks for supplying a valid House model" + id;

            return new OkObjectResult(responseMessage);
        }
    }


}
