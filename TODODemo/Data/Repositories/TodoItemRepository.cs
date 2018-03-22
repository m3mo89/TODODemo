using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TODODemo.Data.Models;

namespace TODODemo.Data.Repositories
{
    /// <summary>
    /// Todo item repository.
    /// </summary>
    public class TodoItemRepository : GenericRepository<TodoItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TODODemo.Data.Repositories.TodoItemRepository"/> class.
        /// </summary>
        public TodoItemRepository()
        {
        }

        /// <summary>
        /// Saves the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="itemToInsert">Item to insert.</param>
        public override Task SaveAsync(TodoItem itemToInsert)
        {
            if (itemToInsert.LastModified == DateTime.MinValue)
                itemToInsert.LastModified = new DateTime();
            
            return base.SaveAsync(itemToInsert);
        }

        /// <summary>
        /// Gets all pending task async.
        /// </summary>
        /// <returns>The all pending task async.</returns>
        /// <param name="token">Token.</param>
        public async Task<IList<TodoItem>> GetAllPendingTaskAsync(CancellationToken? token = null)
        {
            if (token.HasValue)
            {
                token.Value.ThrowIfCancellationRequested();
            }

            IList<TodoItem> items = await ConnectionAsync.Table<TodoItem>().
                                                         Where(x=>x.Status == StatusType.Pending).
                                                         OrderByDescending(x => x.LastModified).
                                                         ToListAsync().ConfigureAwait(false);
            return items;
        }

        /// <summary>
        /// Gets all completed task async.
        /// </summary>
        /// <returns>The all completed task async.</returns>
        /// <param name="token">Token.</param>
        public async Task<IList<TodoItem>> GetAllCompletedTaskAsync(CancellationToken? token = null)
        {
            if (token.HasValue)
            {
                token.Value.ThrowIfCancellationRequested();
            }

            IList<TodoItem> items = await ConnectionAsync.Table<TodoItem>().
                                                         Where(x => x.Status == StatusType.Completed).
                                                         OrderByDescending(x => x.LastModified).
                                                         ToListAsync().ConfigureAwait(false);
            return items;
        }

    }
}
