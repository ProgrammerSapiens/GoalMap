namespace Core.Models
{
    /// <summary>
    /// Represents the task category's model.
    /// </summary>
    public class TaskCategory
    {
        private int id;
        private string name;
        private bool isHabit;
        private int? userId;
        private User user;

        /// <summary>
        /// Gets the unique identifier of the task category.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets or sets the name of the task category.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the task name is null or empty.</exception>
        public string Name
        {
            get => name;
            set
            {
                if(string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name), "The name of the task category can not be null or empty");
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value, is it a habit.
        /// </summary>
        public bool IsHabit
        {
            get => isHabit;
            set => isHabit = value;
        }

        /// <summary>
        /// Gets or sets the ID of the user assigned to the task category.
        /// </summary>
        public int? UserId
        {
            get => userId;
            set => userId = value;
        }

        /// <summary>
        /// Gets or sets the User assigned to the task category.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the User is null.</exception>
        public User User
        {
            get => user;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(user), "The task category must be linked to the user");
                user = value;
            }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TaskCategory"/> class with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier of the task category.</param>
        /// <param name="name">The name of the task category.</param>
        /// <param name="isHabit">An idicator of whether it is a habit.</param>
        /// <param name="userId">The ID of the user assigned to the task category.</param>
        /// <param name="user">The user assigned to the task category.</param>
        public TaskCategory(int id, string name, bool isHabit, int? userId, User user)
        {
            this.id = id;
            Name = name;
            IsHabit = isHabit;
            UserId = userId;
            User = user;
        }
    }
}
