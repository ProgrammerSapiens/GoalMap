namespace Core.Models
{
    /// <summary>
    /// Represents the task category's model.
    /// </summary>
    public class TaskCategory
    {
        private Guid id;
        private string categoryName;
        private Guid userId;
        private User? user;

        /// <summary>
        /// Gets the unique identifier of the task category.
        /// </summary>
        public Guid Id => id;

        /// <summary>
        /// Gets or sets the name of the task category.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the task name is null or empty.</exception>
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The name of the task category can not be null or empty");
                categoryName = value;
            }
        }

        /// <summary>
        /// Gets the ID of the user assigned to the task category.
        /// </summary>
        public Guid UserId => userId;

        /// <summary>
        /// Gets or sets the User assigned to the task category.
        /// </summary>
        public User? User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TaskCategory"/> class with the specified parameters.
        /// </summary>
        /// <param name="categoryName">The name of the task category.</param>
        /// <param name="userId">The ID of the user assigned to the task category.</param>
        public TaskCategory(string categoryName, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException(nameof(userId), "The user ID cannot be empty");
            if (string.IsNullOrEmpty(categoryName))
                throw new ArgumentException(nameof(categoryName), "The name of the task category can not be null or empty");

            id = Guid.NewGuid();
            this.categoryName = categoryName;
            this.userId = userId;
        }
    }
}
