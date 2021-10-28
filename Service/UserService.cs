using System;
using System.Collections.Generic;
using Models;
using DAL;


namespace Service
{
    public interface IUserService
    {
        User POST_User(User User);
        IEnumerable<User> GET_AllUsers();
        User PUT_User(User updatedUser, int UserId);
        void DELETE_User(int UserId);
    }
    public class UserService : IUserService
    {
        public void DELETE_User(int UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GET_AllUsers()
        {
            throw new NotImplementedException();
        }

        public User POST_User(User User)
        {
            throw new NotImplementedException();
        }

        public User PUT_User(User updatedUser, int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
