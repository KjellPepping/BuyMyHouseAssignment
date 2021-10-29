using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace Service
{
    public interface IOfferService
    {
        Offer POST_Offer(Offer offer, ExecutionContext context);

        Offer GET_Offer(User performingUser, ExecutionContext context);

        void SEND_Offer(ExecutionContext context);

        void CALCULATE_Budget(ExecutionContext context);

        List<House> ASSIGN_Houses(Offer offer,ExecutionContext context);
    }
    public class OfferService : IOfferService
    {
        IUserService UserService { get; }
        IOfferRepository OfferRepository { get; }
        IHouseRepository HouseRepositoy { get; }
        public OfferService(IUserService UserService, IOfferRepository OfferRepository, IHouseRepository HouseRepository)
        {
            this.UserService = UserService;
            this.OfferRepository = OfferRepository;
            this.HouseRepositoy = HouseRepositoy;
            
        }

        public List<House> ASSIGN_Houses(Offer offer,ExecutionContext context)
        {
            //Incoming offer can be used to assign houses to user's offer
            return new List<House>();
        }

        public void CALCULATE_Budget(ExecutionContext context)
        {
            List<User> currentUsers = UserService.GET_User_All(context);
            foreach(User user in currentUsers)
            {
                Offer currentOffer = GET_Offer(user,context);
                if(currentOffer == null)
                {
                    Offer newOffer = new Offer();
                    //Not an actual calculation of budget, just for example
                    newOffer.budgetRange = (user.income - 100, user.income + 100);
                    newOffer.offerId = int.Parse(user.userId);
                    newOffer.user = user;
                    newOffer.offerHouses = ASSIGN_Houses(newOffer, context);
                    OfferRepository.POST_Offer(newOffer, context);
                }
            }
        }

        public Offer GET_Offer(User performingUser, ExecutionContext context)
        {
            Task<Offer> retrievedOffer = OfferRepository.GET_Offer(int.Parse(performingUser.userId), context);
            Offer offer = retrievedOffer.Result;
            return offer;
        }

        public Offer POST_Offer(Offer offer, ExecutionContext context)
        {
            OfferRepository.POST_Offer(offer, context);
            return offer;
        }

        public void SEND_Offer(ExecutionContext context)
        {
            List<User> currentUsers = UserService.GET_User_All(context);
            foreach (User user in currentUsers)
            {
                Offer currentOffer = GET_Offer(user, context);
                if (currentOffer == null)
                {
                    string mail = user.email;
                    //CurrentOffer data can be emailed to user's mail adress
                }
            }

        }
    }
}
