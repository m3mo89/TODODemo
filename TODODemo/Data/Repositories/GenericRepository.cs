using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using TODODemo.Data.Managers;
using TODODemo.Data.Models;

namespace TODODemo.Data.Repositories
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    public abstract class GenericRepository<T> where T : BaseModel, new()
    {
        protected SQLiteAsyncConnection ConnectionAsync { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TODODemo.Data.Repositories.GenericRepository`1"/> class.
        /// </summary>
        public GenericRepository()
        {
            //Get connection
            ConnectionAsync = ConnectionManager.Instance.GetAsyncConnection();

            //Create the table according with the specific implementation
            ConnectionAsync.CreateTableAsync<T>();
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>The table.</returns>
        public virtual async Task CreateTable()
        {
            await ConnectionAsync.CreateTableAsync<T>();
        }

        /// <summary>
        /// Saves the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="itemToInsert">Item to insert.</param>
        public virtual async Task SaveAsync(T itemToInsert)
        {
            if (itemToInsert == null)
                throw new NullReferenceException("SaveAsync must have a value to insert SQLite");

            await ConnectionAsync.InsertAsync(itemToInsert).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="itemToUpdate">Item to update.</param>
        public virtual async Task UpdateAsync(T itemToUpdate)
        {
            await ConnectionAsync.UpdateAsync(itemToUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the or update async.
        /// </summary>
        /// <returns>The or update async.</returns>
        /// <param name="itemToInsert">Item to insert.</param>
        public virtual async Task<T> SaveOrUpdateAsync(T itemToInsert)
        {
            if (itemToInsert.ModelId != 0)
                await UpdateAsync(itemToInsert);
            else
                await SaveAsync(itemToInsert);

            return itemToInsert;
        }

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="itemToUpdate">Item to update.</param>
        public virtual async Task DeleteAsync(T itemToUpdate)
        {
            await ConnectionAsync.DeleteAsync(itemToUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all async.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="token">Token.</param>
        public async Task<IList<T>> GetAllAsync(CancellationToken? token = null)
        {
            if (token.HasValue)
            {
                token.Value.ThrowIfCancellationRequested();
            }

            IList<T> items = await ConnectionAsync.Table<T>().ToListAsync().ConfigureAwait(false);
            return items;
        }

        /// <summary>
        /// Finds the by expression.
        /// </summary>
        /// <returns>The by expression.</returns>
        /// <param name="wherePredicate">Where predicate.</param>
        public async Task<T> FindByExpression(Expression<Func<T, bool>> wherePredicate)
        {
            return await ConnectionAsync.FindAsync<T>(wherePredicate).ConfigureAwait(false);
        }
    }
}
