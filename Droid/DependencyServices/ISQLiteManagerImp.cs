using System;
using TODODemo.DependecyServices;

namespace TODODemo.Droid.DependencyServices
{
    public class ISQLiteManagerImp : ISQLiteManager
    {
        /// <summary>
        /// Gets the database path.
        /// </summary>
        /// <returns>The database path.</returns>
        /// <param name="databaseName">Database name.</param>
        public string GetDatabasePath(string databaseName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string databasePath = System.IO.Path.Combine(path, databaseName);
            return databasePath;
        }
    }
}
