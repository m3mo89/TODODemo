using System;
using SQLite;
using TODODemo.DependecyServices;

namespace TODODemo.Data.Managers
{
    public class ConnectionManager
    {
        const string DatabaseName = "todo_propelics_db";
        //Singleton
        static readonly Lazy<ConnectionManager> _instance = new Lazy<ConnectionManager>(() => new ConnectionManager());
        public static ConnectionManager Instance { get { return _instance.Value; } }
        ISQLiteManager _manager;
        //SQlite Async
        SQLiteAsyncConnection _sqliteAsyncConnection;

        private ConnectionManager() { }

        /// <summary>
        /// Gets the async connection.
        /// </summary>
        /// <returns>The async connection.</returns>
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            if (_manager == null || _sqliteAsyncConnection == null)
            {
                throw new Exception("ConnectionManager.Instance.Initialize could not load check Android Main Activity or AppDelegate IOS");
            }

            return _sqliteAsyncConnection;
        }

        /// <summary>
        /// This method should be execute
        /// before using the connection in the
        /// MainActivity Android, AppDelegate IOS
        /// </summary>
        /// <param name="manager">specific implementation for creating the connection</param>
        public void Initialize(ISQLiteManager manager)
        {
            _manager = manager;

            //Create just one connection
            if (_sqliteAsyncConnection == null)
            {
                _sqliteAsyncConnection = new SQLiteAsyncConnection(_manager.GetDatabasePath(DatabaseName));
            }
        }
    }
}
