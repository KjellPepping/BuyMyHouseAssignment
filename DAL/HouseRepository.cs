using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL
{

    public interface IHouseRepository
    {
        House POST_House(House house);
        House GET_House(int houseId);
        IEnumerable<House> GET_AllHouses();
        House PUT_House(House updatedHouse, int houseId);
        void DELETE_House(int houseId);
    }
    public class HouseRepository : IHouseRepository
    {
        public void DELETE_House(int houseId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<House> GET_AllHouses()
        {
            throw new NotImplementedException();
        }

        public House GET_House(int houseId)
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
}
