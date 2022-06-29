using DAL.Abstractions.Interfaces;
using DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Dependencies
{
    /// <summary>
    /// Class that contains extension methods for IServiceCollection and registers the dependencies containing in data access layer project
    /// </summary>
    public static class RegisterDependencies
    {
        /// <summary>
        /// Extension method which registers all dependencies containing in data access layer project
        /// </summary>
        /// <param name="services">Instance of class that implements IServiceCollection interface to add dependencies</param>
        public static void ConfigureDALServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
