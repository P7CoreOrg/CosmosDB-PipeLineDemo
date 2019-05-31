using System;
using System.Collections.Generic;
using CosmosDB.ToDo.Store.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Demystifier;
using Xunit.DependencyInjection.Logging;
using CosmosDB.ToDo.Store.Extensions;

[assembly: TestFramework("XUnitTest_ToDo_Store.Startup", "XUnitTest_ToDo_Store")]
// Set the orderer
[assembly: TestCollectionOrderer("XUnitHelpers.TestCaseOrdering.PriorityOrderer", "XUnitHelpers")]

// Need to turn off test parallelization so we can validate the run order
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace XUnitTest_ToDo_Store
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDependency, DependencyClass>();
            services.AddSingleton<IAsyncExceptionFilter, DemystifyExceptionFilter>();
         
            services.AddToDoItemStore(options =>
            {
                options.EndPointUrl = "https://localhost:8081";
                options.PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                options.DatabaseName = "ToDoDatabase";
                options.Collections = new List<Collection>()
                {
                    new Collection()
                    {
                        CollectionName = CollectionName.ToDo,
                        ReserveUnits = 1000

                    }
                };

            });

        }
        protected override void Configure(IServiceProvider provider)
        {
            provider.GetRequiredService<ILoggerFactory>()
               .AddProvider(new XunitTestOutputLoggerProvider(provider.GetRequiredService<ITestOutputHelperAccessor>()));


        }
    }
}
