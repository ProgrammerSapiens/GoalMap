﻿namespace Core.Models
{
    /// <summary>
    /// Represents the user's model.
    /// </summary>
    public class User
    {
        #region Fields

        private Guid userId;
        private string userName;
        private string passwordHash;
        private int experience;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid UserId
        {
            get { return userId; }
            private set { userId = value; }
        }

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
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the hashed password is null or empty.</exception>
        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentOutOfRangeException(nameof(value), "The passwordHash of the user cannot be null or empty");
                passwordHash = value;
            }
        }

        /// <summary>
        /// Gets or sets the experience points of the user.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the experience is negative.</exception>
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
        /// Gets the current level of the user based on experience points.
        /// </summary>
        public int Level
        {
            get { return (int)Math.Sqrt(Experience / 100); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor required by Entity Framework.
        /// </summary>
        protected User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified user name.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <exception cref="ArgumentException">Thrown if the user name is null or empty.</exception>
        public User(string userName, Guid? userId = null)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException(nameof(userName), "The userName of the user cannot be null or empty");

            UserId = userId ?? Guid.NewGuid();
            this.userName = userName;
            this.passwordHash = "NotHashed";
            Experience = 0;
        }

        #endregion
    }
}
