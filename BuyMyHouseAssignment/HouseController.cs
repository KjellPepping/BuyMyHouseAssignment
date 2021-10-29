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
using Newtonsoft.Json;

namespace BuyMyHouseAssignment
{
    public class HouseController
    {
        IHouseService HouseService { get;}

        public HouseController(IHouseService HouseService)
        {
            this.HouseService = HouseService;
        }
       
        [FunctionName("POST_House")]
        public async Task<IActionResult> POST_House([HttpTrigger(AuthorizationLevel.Function, "post", Route = "house")] HttpRequest req, ILogger log,ExecutionContext context)
        {
            log.LogInformation("/house POST has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string response;

            if (HouseService.VALIDATE_House(requestBody))
            {
                House newHouse = JsonConvert.DeserializeObject<House>(requestBody);
                HouseService.POST_House(newHouse, context);
                response = "The supplied House has been added";
            }
            else
                response = "There has been an error adding the House. Please check the supplied values.";

            return new OkObjectResult(response);
        }

        [FunctionName("GET_House")]
        public async Task<IActionResult> GET_House([HttpTrigger(AuthorizationLevel.Function, "get", Route = "house/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/house GET has been requested.");

            House retrievedHouse = HouseService.GET_House_ID(id,context);

            return new OkObjectResult(retrievedHouse.ToString());
        }

        [FunctionName("DELETE_House")]
        public async Task<IActionResult> DELETE_House([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "house/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/house DELETE has been requested.");

            House toBeDeletedHouse = HouseService.GET_House_ID(id,context);

            if (toBeDeletedHouse != null)
                HouseService.DELETE_House(toBeDeletedHouse,context);

            string responseMessage = "Thanks for supplying a valid House model" +id;

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("PUT_House")]
        public async Task<IActionResult> PUT_House([HttpTrigger(AuthorizationLevel.Function, "put", Route = "house/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/house DELETE has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (HouseService.VALIDATE_House(requestBody))
            {
                House updatedHouse = JsonConvert.DeserializeObject<House>(requestBody);
                HouseService.PUT_House(updatedHouse, id,context);
            }

            string responseMessage = "Thanks for supplying a valid House model" + id;

            return new OkObjectResult(responseMessage);
        }
    }


}
