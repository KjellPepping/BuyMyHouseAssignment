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
    public class BatchController
    {
        IOfferService OfferService { get; }

        public BatchController(IOfferService OfferService)
        {
            this.OfferService = OfferService;
        }

        [FunctionName("CRON_Save_Offers")]

        //Cron schedule for 23:00 every day
        public static void SaveOffers([TimerTrigger("0 23 * * *")]TimerInfo myTimer, ILogger log,ExecutionContext context)
        {
            OfferService.CALCULATE_Budget(context);
        }


        [FunctionName("CRON_Send_Mail")]

        //Cron schedule for 09:00 every day
        public static void MailOffers([TimerTrigger("0 9 * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            OfferService.SEND_Offer(context);
        }
    }
}
