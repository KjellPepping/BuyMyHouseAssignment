using System;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Service
{
    public interface IHouseService
    {
        House VALIDATE_House(string houseBody);
        House POST_House(House house);
        IEnumerable<House> GET_AllHouses();
        House PUT_House(House updatedHouse, int houseId);
        void DELETE_House(int houseId);
    }
    public class HouseService : IHouseService
    {
        public House VALIDATE_House(string houseBody)
        {
            throw new NotImplementedException();
        }
        public void DELETE_House(int houseId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<House> GET_AllHouses()
        {
            throw new NotImplementedException();
        }

        public House POST_House(House house)
        {
            throw new NotImplementedException();
        }

        public House PUT_House(House updatedHouse, int houseId)
        {
            throw new NotImplementedException();
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
