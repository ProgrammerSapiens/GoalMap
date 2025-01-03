using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class TaskCategoryService : ITaskCategoryService
    {
        public TaskCategory GetTaskCategoryById(Guid taskCategoryId)
        {
            throw new NotImplementedException();
        }

        public List<TaskCategory> GetCategoriesByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void AddCategory(TaskCategory taskCategory)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(TaskCategory taskCategory)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(Guid taskCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
