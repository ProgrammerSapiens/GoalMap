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
        private User? user;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the todo category.
        /// </summary>
        public Guid ToDoCategoryId => toDoCategoryId;

        /// <summary>
        /// Gets or sets the name of the todo category.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the todo name is null or empty.</exception>
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
        /// Gets the ID of the user assigned to the todo category.
        /// </summary>
        public Guid UserId => userId;

        /// <summary>
        /// Gets or sets the User assigned to the todo category.
        /// </summary>
        public virtual User? User
        {
            get { return user; }
            set { user = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="ToDoCategory"/> class with the specified parameters.
        /// </summary>
        /// <param name="toDoCategoryName">The name of the todo category.</param>
        /// <param name="userId">The ID of the user assigned to the todo category.</param>
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
