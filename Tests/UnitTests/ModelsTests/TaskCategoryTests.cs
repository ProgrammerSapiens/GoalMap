using Core.Models;

namespace Tests.UnitTests.ModelsTests
{
    public class TaskCategoryTests
    {
        #region Tests for constructors

        [Fact]
        public void TaskCategory_Constructor_With_ValidCategoryNameAndUserId_ShouldInitializePropertiesCorrectly()
        {
            string categoryName = "Test category";
            Guid userId = Guid.NewGuid();

            var taskCategory = new TaskCategory(categoryName, userId);

            Assert.Equal(categoryName, taskCategory.CategoryName);
            Assert.Equal(userId, taskCategory.UserId);
            Assert.NotEqual(Guid.Empty, taskCategory.Id);
        }

        [Fact]
        public void TaskCategory_Constructor_With_EmptyCategoryName_ShouldThrowArgumentException()
        {
            Guid userId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => new TaskCategory(string.Empty, userId));
        }

        [Fact]
        public void TaskCategory_Constructor_With_EmptyUserId_ShouldThrowArgumentException()
        {
            string categoryName = "Test Category";

            Assert.Throws<ArgumentException>(() => new TaskCategory(categoryName, Guid.Empty));
        }

        #endregion

        #region Tests for properties

        [Fact]
        public void TaskCategory_SetCategoryName_WithValidValue_ShouldUpdateCategoryName()
        {
            string categoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var taskCategory = new TaskCategory(categoryName, userId);

            string newCategoryName = "New test category";
            taskCategory.CategoryName = newCategoryName;

            Assert.Equal(newCategoryName, taskCategory.CategoryName);
        }

        [Fact]
        public void TaskCategory_SetCategoryName_WithNullOrEmpty_ShouldThrowArgumentNullException()
        {
            string categoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var taskCategory = new TaskCategory(categoryName, userId);

            string newCategoryName = string.Empty;

            Assert.Throws<ArgumentException>(() => taskCategory.CategoryName = newCategoryName);
        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void TaskCategory_Id_ShouldBeUnique()
        {
            string categoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var taskCategory1 = new TaskCategory(categoryName, userId);
            var taskCategory2 = new TaskCategory(categoryName, userId);

            Assert.NotEqual(taskCategory1.Id, taskCategory2.Id);
        }

        #endregion

        #region Tests for links

        [Fact]
        public void TaskCategory_SetUser_ShouldAssignUserCorrectly()
        {
            string userName = "Test User";
            string passwordHash = "Test Password";
            int experience = 100;

            var user = new User(userName, passwordHash, experience);

            string categoryName = "Test Category";
            Guid userId = user.Id;

            var taskCategory = new TaskCategory(categoryName, userId);

            taskCategory.User = user;

            Assert.NotNull(taskCategory.User);
            Assert.Equal(userName, taskCategory.User.UserName);
            Assert.Equal(experience, taskCategory.User.Experience);
            Assert.Equal(passwordHash, taskCategory.User.PasswordHash);
            Assert.Equal(userId, taskCategory.User.Id);
        }

        #endregion
    }
}
