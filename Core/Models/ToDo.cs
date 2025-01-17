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
    public enum Difficulty { Easy = 5, Medium = 10, Hard = 15, Nightmare = 20 }

    /// <summary>
    /// Enumeration for defining the task's repeat frequency.
    /// </summary>
    public enum RepeatFrequency { None, Daily, Weekly, Monthly, Yearly }

    #endregion

    /// <summary>
    /// Represents the todo's model.
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
        /// Gets the unique identifier of the todo.
        /// </summary>
        public Guid ToDoId
        {
            get { return toDoId; }
            private set { toDoId = value; }
        }

        /// <summary>
        /// Gets or sets the description of the todo.
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
        /// Gets the time block of the todo.
        /// </summary>
        public TimeBlock TimeBlock
        {
            get { return timeBlock; }
            private set { timeBlock = value; }
        }

        /// <summary>
        /// Gets or sets the difficulty level of the todo.
        /// </summary>
        public Difficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        /// <summary>
        /// Gets or sets the deadline for the todo.
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
        /// Gets or sets the date associated with the todo.
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
        /// Gets or sets a value indicating whether the todo is completed.
        /// </summary>
        public bool CompletionStatus
        {
            get { return completionStatus; }
            set { completionStatus = value; }
        }

        /// <summary>
        /// Gets the ID of the parent todo, if applicable.
        /// </summary>
        public Guid? ParentToDoId
        {
            get { return parentToDoId; }
            private set { parentToDoId = value; }
        }

        /// <summary>
        /// Gets or sets the repeat frequency of the todo.
        /// </summary>
        public RepeatFrequency RepeatFrequency
        {
            get { return repeatFrequency; }
            set { repeatFrequency = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the todo category assigned to the todo.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the category ID is empty</exception>
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
        /// Gets or sets the ID of the user assigned to the todo.
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
        /// Gets or sets the todo category assigned to the todo.
        /// </summary>
        public virtual ToDoCategory? ToDoCategory
        {
            get { return toDoCategory; }
            set { toDoCategory = value; }
        }

        /// <summary>
        /// Gets or sets the user assigned to the todo.
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
        /// Initializeds a new instance of the <see cref="ToDo"/> class.
        /// </summary>
        /// <param name="description">The todo's description.</param>
        /// <param name="timeBlock">The time block for the todo.</param>
        /// <param name="difficulty">The difficulty level of the todo.</param>
        /// <param name="toDoDate">The creation date of the todo.</param>
        /// <param name="toDoCategoryId">The unique identifier of the todo's category</param>
        /// <param name="userId">The unique identifier of the user.</param>
        public ToDo(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime toDoDate, string toDoCategoryName, Guid userId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the todo cannot be empty or null");

            toDoId = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            ToDoDate = toDoDate;
            Deadline = null;
            CompletionStatus = false;
            parentToDoId = null;
            RepeatFrequency = RepeatFrequency.None;
            this.toDoCategoryName = toDoCategoryName;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with a dealine.
        /// </summary>
        /// <param name="description">The todo's description.</param>
        /// <param name="timeBlock">The time block for the todo.</param>
        /// <param name="difficulty">The difficulty level of the todo.</param>
        /// <param name="toDoDate">The creating date of the todo.</param>
        /// <param name="toDoCategoryName">The unique identifier of the todo's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the todo.</param>
        public ToDo(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime toDoDate, string toDoCategoryName, Guid userId, DateTime deadline)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the todo cannot be empty or null");

            toDoId = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            ToDoDate = toDoDate;
            Deadline = deadline;
            CompletionStatus = false;
            parentToDoId = null;
            RepeatFrequency = RepeatFrequency.None;
            this.toDoCategoryName = toDoCategoryName;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with a deadline and parent todo ID.
        /// </summary>
        /// <param name="description">The todo's description.</param>
        /// <param name="timeBlock">The time block for the todo.</param>
        /// <param name="difficulty">The difficulty level of the todo.</param>
        /// <param name="toDoDate">The creating date of the todo.</param>
        /// <param name="toDoCategoryName">The unique identifier of the todo's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the todo.</param>
        /// <param name="parentToDoId">The unique identifier of the parent todo.</param>
        public ToDo(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime toDoDate, string toDoCategoryName, Guid userId, DateTime deadline, Guid parentToDoId)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the todo cannot be empty or null");

            toDoId = Guid.NewGuid();
            this.description = description;
            this.timeBlock = timeBlock;
            Difficulty = difficulty;
            ToDoDate = toDoDate;
            Deadline = deadline;
            CompletionStatus = false;
            this.parentToDoId = parentToDoId;
            RepeatFrequency = RepeatFrequency.None;
            this.toDoCategoryName = toDoCategoryName;
            UserId = userId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with a deadline, parent todo ID, and repeat frequency.
        /// </summary>
        /// <param name="description">The todo's description.</param>
        /// <param name="timeBlock">The time block for the todo.</param>
        /// <param name="difficulty">The difficulty level of the todo.</param>
        /// <param name="toDoDate">The creating date of the todo.</param>
        /// <param name="toDoCategoryName">The unique identifier of the todo's category.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="deadline">The deadline for the todo.</param>
        /// <param name="parentToDoId">The unique identifier of the parent todo.</param>
        /// <param name="repeatFrequency">The repeat frequency of the todo.</param>
        public ToDo(string description, TimeBlock timeBlock, Difficulty difficulty, DateTime toDoDate, string toDoCategoryName, Guid userId, DateTime deadline, Guid parentToDoId, RepeatFrequency repeatFrequency)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description), "The description of the todo cannot be empty or null");

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
