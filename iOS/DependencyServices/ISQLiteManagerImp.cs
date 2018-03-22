using System;
using TODODemo.DependecyServices;

namespace TODODemo.iOS.DependencyServices
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
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = System.IO.Path.Combine(documentPath, "..", "Library");
            string databasePath = System.IO.Path.Combine(libraryPath, databaseName);
            return databasePath;
        }
    }
}
