using System;
using System.Text;
using CosmosDB.ToDo.Store.Abstracts;
using CosmosDB.ToDo.Store.Configuration;
using CosmosDB.ToDo.Store.DbContext;
using CosmosDB.ToDo.Store.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosDB.ToDo.Store.Extensions
{
    public static class AspNetCoreStartupExtensions
    {
        /// <summary>
        ///     Add ToDo Store
        /// </summary>
        /// <param name="builder">The IIdentity Server Builder</param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddToDoItemStore(
            this IServiceCollection services, 
            Action<CosmosDbConfiguration> setupAction)
        {
            services.Configure(setupAction);
            services.AddTransient<ISimpleItemDbContext<Item>, DocumentDBRepository<Item>>();
            return services;
        }
    }
}
