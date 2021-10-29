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

    public interface IUserRepository
    {
        User POST_User(User User, ExecutionContext context);
        Task<User> GET_User(int UserId, ExecutionContext context);

        Task<List<User>> GET_All_Users(ExecutionContext context);
        User PUT_User(User updatedUser, int UserId, ExecutionContext context);
        void DELETE_User(User deletedUser, ExecutionContext context);
    }
    public class UserRepository : IUserRepository
    {

        public void DELETE_User(User deletedUser, ExecutionContext context)
        {
            CloudTable UserTable = Config.GetCloudStorageAccount(context, "UserTable");

            var dynamicTableEntity = UserEntityHelper.InitialiseEntity(deletedUser);

            var tableOperation = TableOperation.Delete(dynamicTableEntity);

            UserTable.ExecuteAsync(tableOperation);
        }

        public async Task<User> GET_User(int UserId, ExecutionContext context)
        {
            CloudTable UserTable = Config.GetCloudStorageAccount(context, "UserTable");

            TableQuery<User> getUserQuery = new TableQuery<User>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, UserId.ToString()));

            TableContinuationToken token = null;
            var result = await UserTable.ExecuteQuerySegmentedAsync(getUserQuery, token);

            User User = result.Results[0];

            return await Task.FromResult<User>(User);
        }

        public async Task<List<User>> GET_All_Users(ExecutionContext context)
        {
            CloudTable UserTable = Config.GetCloudStorageAccount(context, "UserTable");

            TableContinuationToken token = null;
            var entities = new List<User>();

            do {
                var queryResult = UserTable.ExecuteQuerySegmentedAsync(new TableQuery<User>(), token);
                entities.AddRange(queryResult.Result.Results);
                token = queryResult.Result.ContinuationToken;
            } while (token != null);

            return await Task.FromResult<List<User>>(entities);
        }

        public User POST_User(User User, ExecutionContext context)
        {
            CloudTable UserTable = Config.GetCloudStorageAccount(context, "UserTable");
            UserTable.CreateIfNotExistsAsync();

            var dynamicTableEntity = UserEntityHelper.InitialiseEntity(User);

            var tableOperation = TableOperation.Insert(dynamicTableEntity);
            UserTable.ExecuteAsync(tableOperation);

            return User;
        }

        public User PUT_User(User updatedUser, int UserId, ExecutionContext context)
        {
            CloudTable UserTable = Config.GetCloudStorageAccount(context, "UserTable");

            var dynamicTableEntity = UserEntityHelper.InitialiseEntity(updatedUser);

            var tableOperation = TableOperation.Merge(dynamicTableEntity);
            UserTable.ExecuteAsync(tableOperation);

            return updatedUser;
        }
    }

    public static class UserEntityHelper
    {
        public static DynamicTableEntity InitialiseEntity(User User)
        {
            var dynamicTableEntity = new DynamicTableEntity();
            dynamicTableEntity.RowKey = User.userId.ToString();
            dynamicTableEntity.PartitionKey = User.city;

            foreach (PropertyInfo prop in User.GetType().GetProperties())
            {
                dynamicTableEntity.Properties.Add(prop.Name, EntityProperty.CreateEntityPropertyFromObject(prop.GetValue(User)));
            }

            dynamicTableEntity.ETag = "*";

            return dynamicTableEntity;
        }
    }
}
