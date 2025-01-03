using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    internal class TaskService : ITaskService
    {
        public Models.Task GetTaskById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Models.Task> GetTasksByDate(Guid userId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Models.Task> GetTasksByTimeBlock(Guid userId, TimeBlock timeBlock)
        {
            throw new NotImplementedException();
        }

        public List<Models.Task> GetTasksByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void AddTask(Models.Task task)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(Models.Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public void MoveIncompleteTasks(DateTime fromDate, DateTime toDate, Guid userId)
        {
            throw new NotImplementedException();
        }

        public void MoveRepeatedTasks(RepeatFrequency repeatFrequency, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
