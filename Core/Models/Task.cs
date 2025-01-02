namespace Core.Models
{
    #region Enums

    /// <summary>
    /// Enumeration for defining the task's time block.
    /// </summary>
    public enum TimeBlock { Day, Week, Month, Year }

    /// <summary>
    /// Enumeration for defining the task's difficulty level.
    /// </summary>
    public enum Difficulty { Easy, Middle, Hard, Nightmare }

    /// <summary>
    /// Enumeration for defining the task's repeat frequency.
    /// </summary>
    public enum RepeatFrequency { None, Daily, Weekly, Monthly, Yearly }

    #endregion

    /// <summary>
    /// Represents the task's model.
    /// </summary>
    public class Task
    {
        #region Fields

        private Guid id;
        private string description;
        private TimeBlock timeBlock;
        private Difficulty difficulty;
        private DateTime? deadline;
        private DateTime taskDate;
        private bool completionStatus;
        private Guid? parentTaskId;
        private RepeatFrequency repeatFrequency;
        private Guid taskCategoryId;
        private Guid userId;
        private TaskCategory? taskCategory;
        private User? user;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the task.
        /// </summary>
        public Guid Id => id;

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the description is null or empty.</exception>
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The description of the task cannot be empty or null");
                description = value;
            }
        }

        /// <summary>
        /// Gets the time block of the task.
        /// </summary>
        public TimeBlock TimeBlock => timeBlock;

        /// <summary>
        /// Gets or sets the difficulty level of the task.
        /// </summary>
        public Difficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        /// <summary>
        /// Gets or sets the deadline for the task.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the deadline is in the past.</exception>
        public DateTime? Deadline
        {
            get { return deadline; }
            set
            {
                if (value.HasValue && value.Value < DateTime.Now)
                    throw new ArgumentOutOfRangeException(nameof(value), "The deadline of the task cannot be in the past");
                if (value.HasValue && value.Value <= TaskDate)
                    throw new ArgumentException(nameof(value), "The deadline of the task cannot be earlier than the task date");
                deadline = value;
            }
        }

        /// <summary>
        /// Gets or sets the date associated with the task.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the date is in the past.</exception>
        public DateTime TaskDate
        {
            get { return taskDate; }
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentOutOfRangeException(nameof(value), "The taskDate of the task cannot be in the past");
                taskDate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task is completed.
        /// </summary>
        public bool CompletionStatus
        {
            get { return completionStatus; }
            set { completionStatus = value; }
        }

        /// <summary>
        /// Gets the ID of the parent task, if applicable.
        /// </summary>
        public Guid? ParentTaskId => parentTaskId;

        /// <summary>
        /// Gets or sets the repeat frequency of the task.
        /// </summary>
        public RepeatFrequency RepeatFrequency
        {
            get { return repeatFrequency; }
            set { repeatFrequency = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the task category assigned to the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the category ID is empty</exception>
        public Guid TaskCategoryId
        {
            get { return taskCategoryId; }
            set
            {
                if (Guid.Empty == value)
                    throw new ArgumentException(nameof(value), "The category id cannot be empty");
                taskCategoryId = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the user assigned to the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the user ID is empty</exception>
        public Guid UserId
        {
            get { return userId; }
            set
            {
                if (Guid.Empty == value)
                    throw new ArgumentException(nameof(value), "The user id cannot be empty");
                userId = value;
            }
        }

        /// <summary>
        /// Gets or sets the task category assigned to the task.
        /// </summary>
        public virtual TaskCategory? TaskCategory
        {
            get { return taskCategory; }
            set { taskCategory = value; }
        }

        /// <summary>
        /// Gets or sets the user assigned to the task.
        /// </summary>
        public virtual User? User
        {
            get { return user; }
            set { user = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializeds a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="description">The task's description.</param>
        /// <param name="timeBlock">The time block for the task.</param>
        /// <param name="difficulty">The difficulty level of the task.</param>
        /// <param name="taskDate">The creation date of the task.</param>
        /// <param name="taskCategoryId">The unique identifier of the task's category</param>
        /// <param name="userId">The unique identifier of the user.</param>
        public Task(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime taskDate, Guid taskCategoryId, Guid userId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the task cannot be empty or null");

            id = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            TaskDate = taskDate;
            Deadline = null;
            CompletionStatus = false;
            parentTaskId = null;
            RepeatFrequency = RepeatFrequency.None;
            TaskCategoryId = taskCategoryId;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with a dealine.
        /// </summary>
        /// <param name="description">The task's description.</param>
        /// <param name="timeBlock">The time block for the task.</param>
        /// <param name="difficulty">The difficulty level of the task.</param>
        /// <param name="taskDate">The creating date of the task.</param>
        /// <param name="taskCategoryId">The unique identifier of the task's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the task.</param>
        public Task(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime taskDate, Guid taskCategoryId, Guid userId, DateTime deadline)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the task cannot be empty or null");

            id = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            TaskDate = taskDate;
            Deadline = deadline;
            CompletionStatus = false;
            parentTaskId = null;
            RepeatFrequency = RepeatFrequency.None;
            TaskCategoryId = taskCategoryId;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with a deadline and parent task ID.
        /// </summary>
        /// <param name="description">The task's description.</param>
        /// <param name="timeBlock">The time block for the task.</param>
        /// <param name="difficulty">The difficulty level of the task.</param>
        /// <param name="taskDate">The creating date of the task.</param>
        /// <param name="taskCategoryId">The unique identifier of the task's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the task.</param>
        /// <param name="parentTaskId">The unique identifier of the parent task.</param>
        public Task(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime taskDate, Guid taskCategoryId, Guid userId, DateTime deadline, Guid parentTaskId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the task cannot be empty or null");

            id = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            TaskDate = taskDate;
            Deadline = deadline;
            CompletionStatus = false;
            this.parentTaskId = parentTaskId;
            RepeatFrequency = RepeatFrequency.None;
            TaskCategoryId = taskCategoryId;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with a deadline, parent task ID, and repeat frequency.
        /// </summary>
        /// <param name="description">The task's description.</param>
        /// <param name="timeBlock">The time block for the task.</param>
        /// <param name="difficulty">The difficulty level of the task.</param>
        /// <param name="taskDate">The creating date of the task.</param>
        /// <param name="taskCategoryId">The unique identifier of the task's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the task.</param>
        /// <param name="parentTaskId">The unique identifier of the parent task.</param>
        /// <param name="repeatFrequency">The repeat frequency of the task.</param>
        public Task(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime taskDate, Guid taskCategoryId, Guid userId, DateTime deadline, Guid parentTaskId, RepeatFrequency repeatFrequency)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the task cannot be empty or null");

            id = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            TaskDate = taskDate;
            Deadline = deadline;
            CompletionStatus = false;
            this.parentTaskId = parentTaskId;
            RepeatFrequency = repeatFrequency;
            TaskCategoryId = taskCategoryId;
            UserId = userId;
        }

        #endregion
    }
}
