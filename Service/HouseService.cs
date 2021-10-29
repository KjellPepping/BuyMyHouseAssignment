using System;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using DAL;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace Service
{
    public interface IHouseService
    {
        bool VALIDATE_House(string houseBody);
        House POST_House(House house, ExecutionContext context);
        House GET_House_ID(int houseId, ExecutionContext context);
        House PUT_House(House updatedHouse, int houseId, ExecutionContext context);
        void DELETE_House(House deletedHouse, ExecutionContext context);
    }
    public class HouseService : IHouseService
    {
        private IHouseRepository HouseRepository { get; }

        public HouseService(IHouseRepository HouseRepository)
        {
            this.HouseRepository = HouseRepository;
        }

        public bool VALIDATE_House(string houseBody)
        {
            return houseSerializer.validateHouse(houseBody);
        }

        public void DELETE_House(House deletedHouse, ExecutionContext context)
        {
            HouseRepository.DELETE_House(deletedHouse, context);
        }

        public House POST_House(House house,ExecutionContext context)
        {
            HouseRepository.POST_House(house,context);
            return house;
        }

        public House PUT_House(House updatedHouse, int houseId, ExecutionContext context)
        {
            House returnedHouse = HouseRepository.PUT_House(updatedHouse, houseId, context);
            return returnedHouse;
        }

        public House GET_House_ID(int houseId, ExecutionContext context)
        {
            Task<House> retrievedHouse = HouseRepository.GET_House(houseId, context);
            House house = retrievedHouse.Result;
            return house;
        }
    }
    public class houseSerializer
    {
        public static bool validateHouse(string requestBody)
        {
            if (requestBody == null || requestBody.ToString() == "")
                return false;

            JSchema schema = JSchema.Parse(
            @"{
                'type':'object',
                'properties':
                {
                   'id': {'type':'integer'},
                   'price': {'type':'number'},
                   'imageUrl': {'type':'string'},
                   'streetName': {'type':'string'},
                   'streetNumber': {'type': 'integer'},
                   'postalCode': {'type':'string'}
                },
                'required': ['id','price','imageUrl','streetName','streetNumber','postalCode']
            }");

            JObject house = JObject.Parse(requestBody);
            return house.IsValid(schema);
        }
    }
}
