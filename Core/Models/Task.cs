namespace Core.Models
{
    /// <summary>
    /// Represents the task's model.
    /// </summary>
    public class Task
    {
        private int id;
        private string title;
        private string description;
        private string taskType;
        private int difficulty;
        private DateTime? deadline;
        private DateTime? taskDate;
        private bool completionStatus;
        private int? parentTaskId;
        private string? repeatFrequency;
        private TaskCategory? typeCategory;
        private int userId;
        private User user;

        /// <summary>
        /// Gets the unique identifier of the task.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets or sets the title of the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the title is null or empty.</exception>
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The title of the task cannot be empty or null");
                title = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the description is null or empty.</exception>
        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The description of the task cannot be empty or null");
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the type is null or empty.</exception>
        public string TaskType
        {
            get => taskType;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The taskType of the task cannot be empty or null");
                taskType = value;
            }
        }

        /// <summary>
        /// Gets or sets the difficulty level of the task (1 to 5).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the difficulty is less than 1 or greater than 5.</exception>
        public int Difficulty
        {
            get => difficulty;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(value), "The difficulty of the task must be between 1 and 5");
                difficulty = value;
            }
        }

        /// <summary>
        /// Gets or sets the deadline for the task.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the deadline is in the past.</exception>
        public DateTime? Deadline
        {
            get => deadline;
            set
            {
                if (value.HasValue && value.Value < DateTime.Now)
                    throw new ArgumentOutOfRangeException(nameof(value), "The deadline of the task cannot be in the past");
                deadline = value;
            }
        }

        /// <summary>
        /// Gets or sets the date associated with the task.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the date is in the past.</exception>
        public DateTime? TaskDate
        {
            get => taskDate;
            set
            {
                if (value.HasValue && value.Value < DateTime.Now)
                    throw new ArgumentOutOfRangeException(nameof(value), "The taskDate of the task cannot be in the past");
                taskDate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task is completed.
        /// </summary>
        public bool CompletionStatus { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent task, if applicable.
        /// </summary>
        public int? ParentTaskId { get; set; }

        /// <summary>
        /// Gets or sets the repeat frequency of the task.
        /// </summary>
        public string? RepeatFrequency { get; set; }

        /// <summary>
        /// Gets or sets the category of the task.
        /// </summary>
        public TaskCategory? TypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user assigned to the task.
        /// </summary>
        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        /// <summary>
        /// Gets or sets the user assigned to the task.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the User is null.</exception>
        public User User
        {
            get => user;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value), "The task must be linked to the user");
                user = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <param name="title">The title of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="taskType">The type of the task.</param>
        /// <param name="difficulty">The difficulty level of the task (1 to 5).</param>
        /// <param name="userId">The ID of the user assigned to the task.</param>
        /// <param name="typeCategory">The category of the task.</param>
        /// <param name="user">The user assigned to the task.</param>
        public Task(int id, string title, string description, string taskType, int difficulty, int userId, TaskCategory typeCategory, User user)
        {
            this.id = id;
            Title = title;
            Description = description;
            TaskType = taskType;
            Difficulty = difficulty;
            UserId = userId;
            TypeCategory = typeCategory;
            User = user;

            //Default values
            CompletionStatus = false;
            ParentTaskId = null;
            RepeatFrequency = null;
            Deadline = null;
            TaskDate = null;
        }
    }
}
