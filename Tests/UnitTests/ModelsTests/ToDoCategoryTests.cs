using Core.Models;

namespace Tests.UnitTests.ModelsTests
{
    public class ToDoCategoryTests
    {
        #region Tests for constructors

        [Fact]
        public void ToDo_Constructor_With_ValidCategoryNameAndUserId_ShouldInitializePropertiesCorrectly()
        {
            string toDoCategoryName = "Test category";
            Guid userId = Guid.NewGuid();

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            Assert.Equal(toDoCategoryName, toDoCategory.ToDoCategoryName);
            Assert.Equal(userId, toDoCategory.UserId);
            Assert.NotEqual(Guid.Empty, toDoCategory.ToDoCategoryId);
        }

        [Fact]
        public void ToDoCategory_Constructor_With_EmptyCategoryName_ShouldThrowArgumentException()
        {
            Guid userId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => new ToDoCategory(userId, string.Empty));
        }

        [Fact]
        public void ToDoCategory_Constructor_With_EmptyUserId_ShouldThrowArgumentException()
        {
            string toDoCategoryName = "Test Category";

            Assert.Throws<ArgumentException>(() => new ToDoCategory(Guid.Empty, toDoCategoryName));
        }

        #endregion

        #region Tests for properties

        [Fact]
        public void ToDoCategory_SetCategoryName_WithValidValue_ShouldUpdateCategoryName()
        {
            string toDoCategoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            string newToDoCategoryName = "New test category";
            toDoCategory.ToDoCategoryName = newToDoCategoryName;

            Assert.Equal(newToDoCategoryName, toDoCategory.ToDoCategoryName);
        }

        [Fact]
        public void ToDoCategory_SetCategoryName_WithNullOrEmpty_ShouldThrowArgumentNullException()
        {
            string toDoCategoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var toDoCategory = new ToDoCategory(userId, toDoCategoryName);

            string newToDoCategoryName = string.Empty;

            Assert.Throws<ArgumentException>(() => toDoCategory.ToDoCategoryName = newToDoCategoryName);
        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void ToDoCategory_Id_ShouldBeUnique()
        {
            string toDoCategoryName = "Test Category";
            Guid userId = Guid.NewGuid();

            var toDoCategory1 = new ToDoCategory(userId, toDoCategoryName);
            var toDoCategory2 = new ToDoCategory(userId, toDoCategoryName);

            Assert.NotEqual(toDoCategory1.ToDoCategoryId, toDoCategory2.ToDoCategoryId);
        }

        #endregion
    }
}
