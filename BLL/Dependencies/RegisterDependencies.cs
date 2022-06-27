using BLL.Abstractions.cs.Interfaces;
using BLL.Email;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dependencies
{
    /// <summary>
    /// Class that contains extension methods for IServiceCollection and registers the dependencies containing in business layer project
    /// </summary>
    public static class RegisterDependencies
    {
        /// <summary>
        /// Extension method which registers all dependencies containing in business layer project
        /// </summary>
        /// <param name="services">Instance of IServiceCollection interface to add dependencies</param>
        public static void ConfigureBLLServices(this IServiceCollection services)
        {
            services.AddScoped<TextMaterialService>();
            services.AddScoped<TextMaterialCategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<RoleService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<CommentService>();
            services.AddScoped<UserService>();
            services.AddScoped<SavedTextMaterialsService>();
            services.AddScoped<LikedTextMaterialService>();
        }
    }
}
