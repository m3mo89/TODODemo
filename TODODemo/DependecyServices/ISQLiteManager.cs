using System;
namespace TODODemo.DependecyServices
{
    /// <summary>
    /// SQL ite manager.
    /// </summary>
    public interface ISQLiteManager
    {
        string GetDatabasePath(string databaseName);
    }
}
