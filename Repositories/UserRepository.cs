using System.Collections.Generic;
using Contracts.Repositories;
using Entities.Models;
using System;
using System.Linq;
using Entities.DbContexts;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        ///<summary>
        ///get user by email
        ///</summary>
        ///<param name="email"></param>
        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(a => a.EmailAddress == email && a.IsActive).FirstOrDefault();
        }

        ///<summary>
        ///get all user
        ///</summary>
        public List<User> GetAllUser()
        {
            return _context.Users.Where(e => e.IsActive).ToList();
        }

        ///<summary>
        ///is user email already exist
        ///</summary>
        ///<param name="email"></param>
        public bool IsEmailExist(string email)
        {
            return _context.Users.Any(a => a.EmailAddress == email && a.IsActive);
        }

        ///<summary>
        ///create new user in Db
        ///</summary>
        ///<param name="email"></param>
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            Save();
        }

        ///<summary>
        ///save all changes
        ///</summary>
        private bool Save()
        {

            _context.OnBeforeSaving(Guid.NewGuid());
            return _context.SaveChanges() >= 0;
        }
    }
}