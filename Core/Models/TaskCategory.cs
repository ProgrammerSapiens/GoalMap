namespace Core.Models
{
    /// <summary>
    /// Represents the task category's model.
    /// </summary>
    public class TaskCategory
    {
        private Guid id;
        private string name;
        private Guid userId;
        private User? user;

        /// <summary>
        /// Gets the unique identifier of the task category.
        /// </summary>
        public Guid Id => id;

        /// <summary>
        /// Gets or sets the name of the task category.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the task name is null or empty.</exception>
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name), "The name of the task category can not be null or empty");
                name = value;
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
        /// <param name="name">The name of the task category.</param>
        /// <param name="userId">The ID of the user assigned to the task category.</param>
        public TaskCategory(string name, Guid userId)
        {
            id = new Guid();
            this.name = name;
            this.userId = userId;
        }
    }
}
