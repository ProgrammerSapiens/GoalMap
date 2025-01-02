namespace Core.Models
{
    /// <summary>
    /// Represents the user's model.
    /// </summary>
    public class User
    {
        private Guid id;
        private string userName;
        private string passwordHash;
        private int experience;
        private ICollection<Task>? tasks;
        private ICollection<TaskCategory>? categories;

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid Id => id;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the user name is null or empty.</exception>
        public string UserName
        {
            get { return userName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The userName of the user cannot be null or empty");
                userName = value;
            }
        }

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the hashed password is null or empty.</exception>
        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "The passwordHash of the user cannot be null or empty");
                passwordHash = value;
            }
        }

        /// <summary>
        /// Gets or sets the experience points of the user.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throwm if the experience is negative.</exception>
        public int Experience
        {
            get { return experience; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "The experience of the user cannot be negative.");
                experience = value;
            }
        }

        /// <summary>
        /// Gets the current level of the user.
        /// </summary>
        public int Level => (int)Math.Sqrt(Experience / 100);

        /// <summary>
        /// Gets or sets tasks assigned to the user.
        /// </summary>
        public ICollection<Task>? Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }

        /// <summary>
        /// Gets or sets categories created by a user.
        /// </summary>
        public ICollection<TaskCategory>? Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified parameters.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="passwordHash">The hashed password of the user.</param>
        /// <param name="experience">The experience points of the user.</param>
        public User(string userName, string passwordHash, int experience)
        {
            id = Guid.NewGuid();
            this.userName = userName;
            this.passwordHash = passwordHash;
            Experience = experience;
        }
    }
}
