namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing tasks, including CRUD operations.
    /// </summary>
    internal interface ITaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Task> GetByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        List<Task> GetByDate(int userId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        void Add(Task task);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        void Update(Task task);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="userId"></param>
        void MoveIncompleteTasks(DateTime fromDate, DateTime toDate, int userId);
    }
}
