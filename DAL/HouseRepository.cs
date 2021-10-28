using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace DAL
{

    public interface IHouseRepository
    {
        Task<House> POST_HouseAsync(House house);
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

        public async Task<House> POST_HouseAsync(House house)
        {
            CloudTable houseTable = Config.GetCloudStorageAccount(new Microsoft.Azure.WebJobs.ExecutionContext(), "housetable");

            var dynamicTableEntity = new DynamicTableEntity();
            dynamicTableEntity.RowKey = "HouseRow";
            dynamicTableEntity.PartitionKey = "HousePartition";

            foreach (PropertyInfo prop in house.GetType().GetProperties())
            {
                dynamicTableEntity.Properties.Add(prop.Name, EntityProperty.CreateEntityPropertyFromObject(prop.GetValue(house)));
            }

            var tableOperation = TableOperation.Insert(dynamicTableEntity);
            await houseTable.ExecuteAsync(tableOperation);

            return house;
        }

        public House PUT_House(House updatedHouse, int houseId)
        {
            throw new NotImplementedException();
        }
    }
}
