using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.Features;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerence_CleanArch.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper registration
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // FluentValidation registration
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Service registrations
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
