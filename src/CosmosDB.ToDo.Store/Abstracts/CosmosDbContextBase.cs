﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CosmosDB.ToDo.Store.Configuration;
using CosmosDB.ToDo.Store.Extensions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace CosmosDB.ToDo.Store.Abstracts
{
    /// <summary>
    ///     Base class for Context that uses CosmosDb.
    /// </summary>
    public abstract class CosmosDbContextBase : IDisposable
    {
        /// <summary>
        ///     CosmosDb Configuration Data
        /// </summary>
        protected readonly CosmosDbConfiguration Configuration;

        /// <summary>
        ///     Logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        ///     Protected Constructor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="connectionPolicy"></param>
        /// <param name="logger"></param>
        protected CosmosDbContextBase(
            IOptions<CosmosDbConfiguration> settings,
            ConnectionPolicy connectionPolicy = null,
            ILogger logger = null)
        {
            Guard.ForNullOrDefault(settings.Value, nameof(settings));
            Guard.ForNullOrDefault(settings.Value.EndPointUrl, nameof(settings.Value.EndPointUrl));
            Guard.ForNullOrDefault(settings.Value.PrimaryKey, nameof(settings.Value.PrimaryKey));
            Guard.ForNullOrDefault(settings.Value.DatabaseName, nameof(settings.Value.EndPointUrl));
            Logger = logger;
            Configuration = settings.Value;

            var serviceEndPoint = new Uri(settings.Value.EndPointUrl);
            DocumentClient = new DocumentClient(serviceEndPoint, settings.Value.PrimaryKey,
                connectionPolicy ?? ConnectionPolicy.Default);

            EnsureDatabaseCreated(Configuration.DatabaseName).Wait();
        }

        /// <summary>
        ///     CosmosDb Document Client.
        /// </summary>
        protected DocumentClient DocumentClient { get; }

        /// <summary>
        ///     Instance of CosmosDb Database.
        /// </summary>
        protected Database Database { get; private set; }

        /// <summary>
        ///     URL for CosmosDb Instance.
        /// </summary>
        protected Uri DatabaseUri { get; private set; }

        /// <summary>
        ///     Dispose of object resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the Reserve Units (RU) per second for a given collection from configuration or the default value (1000).
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        protected int GetRUsFor(CollectionName collection)
        {
            return Configuration.Collections.FirstOrDefault(x => x.CollectionName.Equals(collection))?.ReserveUnits
                   ?? Constants.DefaultReserveUnits;
        }

        private async Task EnsureDatabaseCreated(string databaseName)
        {
            var dbNameToUse = Configuration.DatabaseName.GetValueOrDefault(databaseName);

            DatabaseUri = UriFactory.CreateDatabaseUri(dbNameToUse);
            Logger?.LogDebug($"Database URI: {DatabaseUri}");
            Database = new Database { Id = databaseName };
            Logger?.LogDebug($"Database: {Database}");

            Logger.LogDebug($"Ensuring `{Database.Id}` exists...");
            var result = DocumentClient.CreateDatabaseIfNotExistsAsync(Database).Result;
            Logger.LogDebug($"{Database.Id} Creation Results: {result.StatusCode}");
            if (result.StatusCode.EqualsOne(HttpStatusCode.Created, HttpStatusCode.OK))
                Database = result.Resource;

        }

        /// <summary>
        ///     Dispose of object resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO : Dispose of any resources
            }
        }

        /// <summary>
        ///     Deconstructor
        /// </summary>
        ~CosmosDbContextBase()
        {
            Dispose(false);
        }
    }
}
