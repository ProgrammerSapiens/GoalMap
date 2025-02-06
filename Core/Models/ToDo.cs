namespace Core.Models
{
    #region Enums

    /// <summary>
    /// Defines the time block for a task.
    /// </summary>
    public enum TimeBlock { Day, Week, Month, Year }

    /// <summary>
    /// Represents the difficulty level of a task.
    /// </summary>
    public enum Difficulty { None = 0, Easy = 5, Medium = 10, Hard = 15, Nightmare = 20 }

    /// <summary>
    /// Specifies how often a task should repeat.
    /// </summary>
    public enum RepeatFrequency { None, Daily, Weekly, Monthly, Yearly }

    #endregion

    /// <summary>
    /// Represents a to-do task with properties such as description, difficulty, deadline, and completion status.
    /// </summary>
    public class ToDo
    {
        #region Fields

        private Guid toDoId;
        private string description;
        private TimeBlock timeBlock;
        private Difficulty difficulty;
        private DateTime? deadline;
        private DateTime toDoDate;
        private bool completionStatus;
        private Guid? parentToDoId;
        private RepeatFrequency repeatFrequency;
        private string toDoCategoryName;
        private Guid userId;
        private ToDoCategory? toDoCategory;
        private User? user;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the to-do task.
        /// </summary>
        public Guid ToDoId
        {
            get { return toDoId; }
            private set { toDoId = value; }
        }

        /// <summary>
        /// Gets or sets the description of the to-do task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the description is null or empty.</exception>
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The description of the todo cannot be empty or null");
                description = value;
            }
        }

        /// <summary>
        /// Gets the time block assigned to the to-do task.
        /// </summary>
        public TimeBlock TimeBlock
        {
            get { return timeBlock; }
            private set { timeBlock = value; }
        }

        /// <summary>
        /// Gets or sets the difficulty level of the to-do task.
        /// </summary>
        public Difficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        /// <summary>
        /// Gets or sets the deadline for the to-do task.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the deadline is in the past.</exception>
        public DateTime? Deadline
        {
            get { return deadline; }
            set
            {
                if (value.HasValue && value.Value < DateTime.UtcNow)
                    throw new ArgumentOutOfRangeException(nameof(value), "The deadline of the todo cannot be in the past");
                if (value.HasValue && value.Value <= ToDoDate)
                    throw new ArgumentException(nameof(value), "The deadline of the todo cannot be earlier than the todo date");
                deadline = value;
            }
        }

        /// <summary>
        /// Gets or sets the date the task is scheduled for.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the date is in the past.</exception>
        public DateTime ToDoDate
        {
            get { return toDoDate; }
            set
            {
                if (value < DateTime.Today)
                    throw new ArgumentOutOfRangeException(nameof(value), "The toDoDate of the todo cannot be in the past");
                toDoDate = value;
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
        /// Gets the unique identifier of the parent task, if applicable.
        /// </summary>
        public Guid? ParentToDoId
        {
            get { return parentToDoId; }
            private set { parentToDoId = value; }
        }

        /// <summary>
        /// Gets or sets the repeat frequency of the task.
        /// </summary>
        public RepeatFrequency RepeatFrequency
        {
            get { return repeatFrequency; }
            set { repeatFrequency = value; }
        }

        //TODO: Change category name to category id
        /// <summary>
        /// Gets or sets the name of the category assigned to the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the category name is empty.</exception>
        public string ToDoCategoryName
        {
            get { return toDoCategoryName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The category id cannot be empty");
                toDoCategoryName = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the user assigned to the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the user ID is empty.</exception>
        public Guid UserId
        {
            get { return userId; }
            private set
            {
                if (Guid.Empty == value)
                    throw new ArgumentException(nameof(value), "The user id cannot be empty");
                userId = value;
            }
        }

        /// <summary>
        /// Gets or sets the category assigned to the task.
        /// </summary>
        public virtual ToDoCategory? ToDoCategory
        {
            get { return toDoCategory; }
            set { toDoCategory = value; }
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
        /// Parameterless constructor required by Entity Framework.
        /// </summary>
        protected ToDo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class.
        /// </summary>
        /// <param name="description">Description of the task.</param>
        /// <param name="timeBlock">Time block for the task.</param>
        /// <param name="difficulty">Difficulty level of the task.</param>
        /// <param name="toDoDate">Scheduled date of the task.</param>
        /// <param name="toDoCategoryName">Category name of the task.</param>
        /// <param name="userId">Unique identifier of the user.</param>
        /// <param name="deadline">Deadline for the task (optional).</param>
        /// <param name="parentToDoId">Parent task ID (optional).</param>
        /// <param name="repeatFrequency">Repeat frequency of the task (optional).</param>
        public ToDo(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime toDoDate, string toDoCategoryName, Guid userId, DateTime? deadline = null, Guid parentToDoId = new Guid(), RepeatFrequency repeatFrequency = RepeatFrequency.None)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description cannot be empty or null");

            toDoId = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            ToDoDate = toDoDate;
            Deadline = deadline;
            CompletionStatus = false;
            this.parentToDoId = parentToDoId;
            RepeatFrequency = repeatFrequency;
            this.toDoCategoryName = toDoCategoryName;
            UserId = userId;
        }

        #endregion
    }
}
