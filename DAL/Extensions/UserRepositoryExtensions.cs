using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Extensions
{
    /// <summary>
    /// Extension class for UserRepository to implement searching
    /// </summary>
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> SearcByUserName(this IQueryable<User> users, string searchUserName)
        {
            if (string.IsNullOrWhiteSpace(searchUserName))
            {
                return users;
            }

            var lowerCaseTerm = searchUserName.Trim().ToLower();
            return users.Where(u => u.UserName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<User> SearchByEmail(this IQueryable<User> users, string searchEmail)
        {
            if (string.IsNullOrWhiteSpace(searchEmail))
            {
                return users;
            }

            var lowerCaseTerm = searchEmail.Trim().ToLower();
            return users.Where(u => u.Email.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<User> FilterByBanStatus(this IQueryable<User> users, bool isBanned)
        {
            if (!isBanned)
            {
                return users;
            }

            return users.Where(u => u.Ban != null && u.Ban.Expires > DateTime.Now);
        }
    }
}
