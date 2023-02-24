using System.Collections.Generic;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IUserRepository
    {
        ///<summary>
        ///get user by user email
        ///</summary>
        ///<param name="email"></param>
        User GetUserByEmail(string email);

        ///<summary>
        ///is user email already exist
        ///</summary>
        ///<param name="email"></param>
        bool IsEmailExist(string email);

        ///<summary>
        ///create new user in Db
        ///</summary>
        ///<param name="email"></param>
        void CreateUser(User user);

        ///<summary>
        ///get user by email
        ///</summary>
        List<User> GetAllUser();

    }
}