﻿using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB.ToDo.Store.Interfaces
{
    public interface ISimpleItemDbContext<T> : IDisposable where T: class
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<Document> CreateItemAsync(T item);

        Task<Document> UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
