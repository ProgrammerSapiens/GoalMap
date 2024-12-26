namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for managing tasks, including CRUD operations and task movement.
    /// </summary>
    internal interface ITaskService
    {
        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <returns></returns>
        Task GetTaskById(int id);

        /// <summary>
        /// Retrieves a list of tasks associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of tasks assigned to the specified user.</returns>
        List<Task> GetTasksByUser(int userId);

        /// <summary>
        /// Retrieves a list of tasks for a specific user and date.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="date">The date for which tasks are to be retrieved.</param>
        /// <returns>A list of tasks scheduled for the specified user on the given date.</returns>
        List<Task> GetTasksByDate(int userId, DateTime date);

        /// <summary>
        /// Adds a new task to the system.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        void AddTask(Task task);

        /// <summary>
        /// Updates the details of an existing task.
        /// </summary>
        /// <param name="task">The task with updated information.</param>
        void UpdateTask(Task task);

        /// <summary>
        /// Deletes a task from the system by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task to be deleted.</param>
        void DeleteTask(int id);

        /// <summary>
        /// Moves incomplete tasks from one date to another for a specified user.
        /// </summary>
        /// <param name="fromDate">The date from which tasks will be moved.</param>
        /// <param name="toDate">The date to which tasks will be moved.</param>
        /// <param name="userId">The unique identifier of the user whose tasks are being moved.</param>
        void MoveIncompleteTasks(DateTime fromDate, DateTime toDate, int userId);
    }
}
