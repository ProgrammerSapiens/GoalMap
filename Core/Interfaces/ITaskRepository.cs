namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing tasks, including CRUD operations.
    /// </summary>
    internal interface ITaskRepository
    {
        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <returns>The task with the specified identifier.</returns>
        Task GetById(int id);

        /// <summary>
        /// Retrieves all tasks associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of tasks associated with the user.</returns>
        List<Task> GetByUserId(int userId);

        /// <summary>
        /// Retrieves all tasks for a specific user on a given date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which tasks are retrieved.</param>
        /// <returns>A list of tasks for the specified user on the given date.</returns>
        List<Task> GetByDate(int userId, DateTime date);

        /// <summary>
        /// Adds a new task to the repository.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        void Add(Task task);

        /// <summary>
        /// Updates an existing task in the repository.
        /// </summary>
        /// <param name="task">The task to be updated.</param>
        void Update(Task task);

        /// <summary>
        /// Deletes a task from the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Moves all incomplete tasks for a specific user from one date to another.
        /// </summary>
        /// <param name="fromDate">The date from which incomplete tasks are moved.</param>
        /// <param name="toDate">The date to which incomplete tasks are moved.</param>
        /// <param name="userId">The unique identifier of the user whose tasks are moved.</param>
        void MoveIncompleteTasks(DateTime fromDate, DateTime toDate, int userId);
    }
}
