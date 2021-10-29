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

    public interface IOfferRepository
    {
        Offer POST_Offer(Offer Offer, ExecutionContext context);
        Task<Offer> GET_Offer(int OfferId, ExecutionContext context);
        Offer PUT_Offer(Offer updatedOffer, int OfferId, ExecutionContext context);
        void DELETE_Offer(Offer deletedOffer, ExecutionContext context);
    }
    public class OfferRepository : IOfferRepository
    {

        public void DELETE_Offer(Offer deletedOffer, ExecutionContext context)
        {
            CloudTable OfferTable = Config.GetCloudStorageAccount(context, "OfferTable");

            var dynamicTableEntity = OfferEntityHelper.InitialiseEntity(deletedOffer);

            var tableOperation = TableOperation.Delete(dynamicTableEntity);

            OfferTable.ExecuteAsync(tableOperation);
        }

        public async Task<Offer> GET_Offer(int OfferId, ExecutionContext context)
        {
            CloudTable OfferTable = Config.GetCloudStorageAccount(context, "OfferTable");

            TableQuery<Offer> getOfferQuery = new TableQuery<Offer>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, OfferId.ToString()));

            TableContinuationToken token = null;
            var result = await OfferTable.ExecuteQuerySegmentedAsync(getOfferQuery, token);

            Offer Offer = result.Results[0];

            return await Task.FromResult<Offer>(Offer);
        }

        public Offer POST_Offer(Offer Offer, ExecutionContext context)
        {
            CloudTable OfferTable = Config.GetCloudStorageAccount(context, "OfferTable");
            OfferTable.CreateIfNotExistsAsync();

            var dynamicTableEntity = OfferEntityHelper.InitialiseEntity(Offer);

            var tableOperation = TableOperation.Insert(dynamicTableEntity);
            OfferTable.ExecuteAsync(tableOperation);

            return Offer;
        }

        public Offer PUT_Offer(Offer updatedOffer, int OfferId, ExecutionContext context)
        {
            CloudTable OfferTable = Config.GetCloudStorageAccount(context, "OfferTable");

            var dynamicTableEntity = OfferEntityHelper.InitialiseEntity(updatedOffer);

            var tableOperation = TableOperation.Merge(dynamicTableEntity);
            OfferTable.ExecuteAsync(tableOperation);

            return updatedOffer;
        }
    }

    public static class OfferEntityHelper
    {
        public static DynamicTableEntity InitialiseEntity(Offer Offer)
        {
            var dynamicTableEntity = new DynamicTableEntity();
            dynamicTableEntity.RowKey = Offer.offerId.ToString();
            dynamicTableEntity.PartitionKey = Offer.user.city;

            foreach (PropertyInfo prop in Offer.GetType().GetProperties())
            {
                dynamicTableEntity.Properties.Add(prop.Name, EntityProperty.CreateEntityPropertyFromObject(prop.GetValue(Offer)));
            }

            dynamicTableEntity.ETag = "*";

            return dynamicTableEntity;
        }
    }
}
