using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TODODemo.Data.Models;
using TODODemo.Data.Repositories;

namespace TODODemo.Data.Managers
{
    public class TodoItemManager
    {
        TodoItemRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TODODemo.Data.Managers.TodoItemManager"/> class.
        /// </summary>
        public TodoItemManager()
        {
            _repo = new TodoItemRepository();
        }

        /// <summary>
        /// Saves the or update in DB async.
        /// </summary>
        /// <returns>The or update in DB async.</returns>
        /// <param name="item">Item.</param>
        public async Task<bool> SaveOrUpdateInDBAsync(TodoItem item)
        {
            try
            {
                item.LastModified = DateTime.Now;
                await _repo.SaveOrUpdateAsync(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all pending task async.
        /// </summary>
        /// <returns>The all pending task async.</returns>
        public async Task<IList<TodoItem>> GetAllPendingTaskAsync()
        {
            IList<TodoItem> items = null;
            try
            {
                items = await _repo.GetAllPendingTaskAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return items;
        }

        /// <summary>
        /// Gets all completed task async.
        /// </summary>
        /// <returns>The all completed task async.</returns>
        public async Task<IList<TodoItem>> GetAllCompletedTaskAsync()
        {
            IList<TodoItem> items = null;
            try
            {
                items = await _repo.GetAllCompletedTaskAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return items;
        }

        public async Task<TodoItem> GetTaskById(int id)
        {

            TodoItem item = null;
            try
            {
                item = await _repo.GetTaskByIdAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return item;
        }
    }
}
