using System;
using TODODemo.Data.Models;

namespace TODODemo.DependecyServices
{
    public interface IShareFile
    {
        void ShareLocalFile(TodoItem item);
    }
}
