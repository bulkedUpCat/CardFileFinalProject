using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFileTests.BusinessTests.ServicesTests
{
    public class RoleServiceTests
    {
        [Test]
        public async Task RoleService_GetRolesAsync_ReturnsAllValues()
        {
            // Arrange
            var expected = GetRoles;
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>();


        }

        public List<string> GetRoles =>
            new List<string>()
            {
                "Manager",
                "Admin"
            };
    }
}
