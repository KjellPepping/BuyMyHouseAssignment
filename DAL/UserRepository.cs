using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL
{
    public interface IUserRepository
    {
        User POST_User(User User);
        User GET_User(int UserId);
        IEnumerable<User> GET_AllUsers();
        User PUT_User(User updatedUser, int UserId);
        void DELETE_User(int UserId);
    }
    public class UserRepository : IUserRepository
    {
        public void DELETE_User(int UserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GET_AllUsers()
        {
            throw new NotImplementedException();
        }

        public User GET_User(int UserId)
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
