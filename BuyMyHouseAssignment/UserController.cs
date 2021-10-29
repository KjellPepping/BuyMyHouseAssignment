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

namespace BuyMyUserAssignment
{
    public class UserController
    {
        IUserService UserService { get; }

        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [FunctionName("POST_User")]
        public async Task<IActionResult> POST_User([HttpTrigger(AuthorizationLevel.Function, "post", Route = "User")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("/User POST has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string response;

            if (UserService.VALIDATE_User(requestBody))
            {
                User newUser = JsonConvert.DeserializeObject<User>(requestBody);
                UserService.POST_User(newUser, context);
                response = "The supplied User has been added";
            }
            else
                response = "There has been an error adding the User. Please check the supplied values.";

            return new OkObjectResult(response);
        }

        [FunctionName("GET_User")]
        public async Task<IActionResult> GET_User([HttpTrigger(AuthorizationLevel.Function, "get", Route = "User/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/User GET has been requested.");

            User retrievedUser = UserService.GET_User_ID(id, context);

            return new OkObjectResult(retrievedUser.ToString());
        }

        [FunctionName("DELETE_User")]
        public async Task<IActionResult> DELETE_User([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "User/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/User DELETE has been requested.");

            User toBeDeletedUser = UserService.GET_User_ID(id, context);

            if (toBeDeletedUser != null)
                UserService.DELETE_User(toBeDeletedUser, context);

            string responseMessage = "Thanks for supplying a valid User model" + id;

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("PUT_User")]
        public async Task<IActionResult> PUT_User([HttpTrigger(AuthorizationLevel.Function, "put", Route = "User/{id:int}")] HttpRequest req, ILogger log, int id, ExecutionContext context)
        {
            log.LogInformation("/User DELETE has been requested.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (UserService.VALIDATE_User(requestBody))
            {
                User updatedUser = JsonConvert.DeserializeObject<User>(requestBody);
                UserService.PUT_User(updatedUser, id, context);
            }

            string responseMessage = "Thanks for supplying a valid User model" + id;

            return new OkObjectResult(responseMessage);
        }
    }


}
