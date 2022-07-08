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
        /// <summary>
        /// Extension method to search users by user name
        /// </summary>
        /// <param name="users">Users to search</param>
        /// <param name="searchUserName">Search user name to search users by</param>
        /// <returns>Users with user names that contain search string</returns>
        public static IQueryable<User> SearcByUserName(this IQueryable<User> users, string searchUserName)
        {
            if (string.IsNullOrWhiteSpace(searchUserName))
            {
                return users;
            }

            var lowerCaseTerm = searchUserName.Trim().ToLower();
            return users.Where(u => u.UserName.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Extension method to search users by email
        /// </summary>
        /// <param name="users">Users to search</param>
        /// <param name="searchEmail">Search email to search users by</param>
        /// <returns>Users with emails that contain search string</returns>
        public static IQueryable<User> SearchByEmail(this IQueryable<User> users, string searchEmail)
        {
            if (string.IsNullOrWhiteSpace(searchEmail))
            {
                return users;
            }

            var lowerCaseTerm = searchEmail.Trim().ToLower();
            return users.Where(u => u.Email.ToLower().Contains(lowerCaseTerm));
        }

        /// <summary>
        /// Extension method to filter users by ban status
        /// </summary>
        /// <param name="users">Users to filter</param>
        /// <param name="isBanned">Filter criteria</param>
        /// <returns>Banned users with non-expired ban</returns>
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
