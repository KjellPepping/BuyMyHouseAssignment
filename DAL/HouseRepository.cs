using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace DAL
{

    public interface IHouseRepository
    {
        House POST_House(House house,ExecutionContext context);
        Task<House> GET_House(int houseId, ExecutionContext context);
        House PUT_House(House updatedHouse, int houseId, ExecutionContext context);
        void DELETE_House(House deletedHouse, ExecutionContext context);
    }
    public class HouseRepository : IHouseRepository
    {
     
        public void DELETE_House(House deletedHouse, ExecutionContext context)
        {
            CloudTable houseTable = Config.GetCloudStorageAccount(context, "HouseTable");

            var dynamicTableEntity = HouseEntityHelper.InitialiseEntity(deletedHouse);

            var tableOperation = TableOperation.Delete(dynamicTableEntity);

            houseTable.ExecuteAsync(tableOperation);
        }

        public async Task<House> GET_House(int houseId, ExecutionContext context)
        {
            CloudTable houseTable = Config.GetCloudStorageAccount(context, "HouseTable");

            TableQuery<House> getHouseQuery = new TableQuery<House>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, houseId.ToString()));

            TableContinuationToken token = null;
            var result = await houseTable.ExecuteQuerySegmentedAsync(getHouseQuery, token);

            House house = result.Results[0];

            return await Task.FromResult<House>(house);
        }

        public House POST_House(House house, ExecutionContext context)
        {
            CloudTable houseTable = Config.GetCloudStorageAccount(context, "HouseTable");
            houseTable.CreateIfNotExistsAsync();

            var dynamicTableEntity = HouseEntityHelper.InitialiseEntity(house);

            var tableOperation = TableOperation.Insert(dynamicTableEntity);
            houseTable.ExecuteAsync(tableOperation);

            HouseImageRepository.POST_Image(house.imageUrl, house.postalCode + ".png", context, "houseBlob");

            return house;
        }

        public House PUT_House(House updatedHouse, int houseId, ExecutionContext context)
        {
            CloudTable houseTable = Config.GetCloudStorageAccount(context, "HouseTable");

            var dynamicTableEntity = HouseEntityHelper.InitialiseEntity(updatedHouse);

            var tableOperation = TableOperation.Merge(dynamicTableEntity);
            houseTable.ExecuteAsync(tableOperation);

            return updatedHouse;
        }
    }

    public static class HouseImageRepository
    {
        public static void POST_Image(string url, string fileName, ExecutionContext context, string blobName)
        {
            CloudBlobContainer blobContainer = Config.GetCloudBlobAccount(context, blobName);
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);
            blockBlob.UploadFromFileAsync(url);
        }
    }

    public static class HouseEntityHelper
    {
        public static DynamicTableEntity InitialiseEntity(House house)
        {
            var dynamicTableEntity = new DynamicTableEntity();
            dynamicTableEntity.RowKey = house.id.ToString();
            dynamicTableEntity.PartitionKey = house.city;

            foreach (PropertyInfo prop in house.GetType().GetProperties())
            {
                dynamicTableEntity.Properties.Add(prop.Name, EntityProperty.CreateEntityPropertyFromObject(prop.GetValue(house)));
            }

            dynamicTableEntity.ETag = "*";

            return dynamicTableEntity;
        }
    }
}
