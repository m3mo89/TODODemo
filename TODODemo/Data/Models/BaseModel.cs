using System;
using SQLite;

namespace TODODemo.Data.Models
{
    public class BaseModel
    {
        /// <summary>
        /// Id for local database SQLite
        /// </summary>
        /// <value>The model identifier.</value>
        [PrimaryKey, AutoIncrement]
        public int ModelId { get; set; }
    }
}
