namespace Core.Models
{
    /// <summary>
    /// Represents a model of users.
    /// </summary>
    public class User
    {
        private int id;
        private string userName;
        private string passwordHash;
        private int experience;
        private int level;
        private ICollection<Task> tasks;
        private ICollection<TaskCategory> categories;

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName
        {
            get => userName;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("UserName cannot be null or empty", nameof(value));
                userName = value;
            }
        }

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string PasswordHash
        {
            get => passwordHash;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("PasswordHash cannot be null or empty", nameof(value));
                passwordHash = value;
            }
        }

        /// <summary>
        /// Gets or sets the experience points of the user.
        /// </summary>
        public int Experience
        {
            get => experience;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Experience cannot be negative.");
                experience = value;
            }
        }

        /// <summary>
        /// Gets the current level of the user.
        /// </summary>
        public int Level => level;

        /// <summary>
        /// Gets or sets tasks assigned to the user.
        /// </summary>
        public ICollection<Task> Tasks => tasks ??= new List<Task>();

        /// <summary>
        /// Gets or sets categories created by a user.
        /// </summary>
        public ICollection<TaskCategory> Categories => categories ??= new List<TaskCategory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="userName">The name of the user.</param>
        /// <param name="passwordHash">The hashed password of the user.</param>
        /// <param name="experience">The experience points of the user.</param>
        public User(int id, string userName, string passwordHash, int experience)
        {
            this.id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Experience = experience;
            tasks = new List<Task>();
            categories = new List<TaskCategory>();
        }
    }
}
