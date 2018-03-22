using System;
using SQLite;

namespace TODODemo.Data.Models
{
    /// <summary>
    /// Todo item.
    /// </summary>
    public class TodoItem : BaseModel
    {
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
        public StatusType Status { get; set; }
        public string Image { get; set; }
    }
}
