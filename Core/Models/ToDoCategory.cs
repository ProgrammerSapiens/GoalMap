namespace Core.Models
{
    /// <summary>
    /// Represents the todo category's model.
    /// </summary>
    public class ToDoCategory
    {
        #region Fields

        private Guid toDoCategoryId;
        private string toDoCategoryName;
        private Guid userId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the to-do category.
        /// </summary>
        public Guid ToDoCategoryId
        {
            get { return toDoCategoryId; }
            private set { toDoCategoryId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the to-do category.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the category name is null or empty.</exception>
        public string ToDoCategoryName
        {
            get { return toDoCategoryName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value), "Category name cannot be null or empty.");
                toDoCategoryName = value;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the user associated with this category.
        /// </summary>
        public Guid UserId
        {
            get { return userId; }
            private set { userId = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor required by Entity Framework.
        /// </summary>
        protected ToDoCategory() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCategory"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user associated with this category.</param>
        /// <param name="toDoCategoryName">The name of the to-do category.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="userId"/> is empty or <paramref name="toDoCategoryName"/> is null or empty.</exception>
        public ToDoCategory(Guid userId, string toDoCategoryName)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException(nameof(userId), "User ID cannot be empty");
            if (string.IsNullOrEmpty(toDoCategoryName))
                throw new ArgumentException(nameof(toDoCategoryName), "Category name cannot be null or empty.");

            toDoCategoryId = Guid.NewGuid();
            this.toDoCategoryName = toDoCategoryName;
            this.userId = userId;
        }

        #endregion
    }
}
