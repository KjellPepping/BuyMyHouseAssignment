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
    public interface IUserService
    {
        bool VALIDATE_User(string UserBody);
        User POST_User(User User, ExecutionContext context);
        User GET_User_ID(int UserId, ExecutionContext context);
        List<User> GET_User_All(ExecutionContext context);
        User PUT_User(User updatedUser, int UserId, ExecutionContext context);
        void DELETE_User(User deletedUser, ExecutionContext context);
    }
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; }

        public UserService(IUserRepository UserRepository)
        {
            this.UserRepository = UserRepository;
        }

        public bool VALIDATE_User(string UserBody)
        {
            return UserSerializer.validateUser(UserBody);
        }

        public void DELETE_User(User deletedUser, ExecutionContext context)
        {
            UserRepository.DELETE_User(deletedUser, context);
        }

        public User POST_User(User User, ExecutionContext context)
        {
            UserRepository.POST_User(User, context);
            return User;
        }

        public User PUT_User(User updatedUser, int UserId, ExecutionContext context)
        {
            User returnedUser = UserRepository.PUT_User(updatedUser, UserId, context);
            return returnedUser;
        }

        public User GET_User_ID(int UserId, ExecutionContext context)
        {
            Task<User> retrievedUser = UserRepository.GET_User(UserId, context);
            User User = retrievedUser.Result;
            return User;
        }

        public List<User> GET_User_All(ExecutionContext context)
        {
            Task<List<User>> retrievedUsers = UserRepository.GET_All_Users(context);
            List<User> Users = retrievedUsers.Result;
            return Users;
        }
    }
    public class UserSerializer
    {
        public static bool validateUser(string requestBody)
        {
            if (requestBody == null || requestBody.ToString() == "")
                return false;

            JSchema schema = JSchema.Parse(
            @"{
                'type':'object',
                'properties':
                {
                   'userId': {'type':'integer'},
                   'name': {'type':'string'},
                   'email': {'type':'string'},
                   'house': {'type':'object'},
                   'income': {'type': 'number'},
                   'city': {'type':'string'}
                },
                'required': ['userId','name','email','income','city']
            }");

            JObject User = JObject.Parse(requestBody);
            return User.IsValid(schema);
        }
    }
}
