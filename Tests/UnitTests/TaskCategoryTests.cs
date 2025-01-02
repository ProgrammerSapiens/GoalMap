using Core.Models;

namespace Tests.UnitTests
{
    public class TaskCategoryTests
    {
        #region Tests for constructors

        [Fact]
        public void TaskCategory_Constructor_With_ValidCategoryNameAndUserId_ShouldInitializePropertiesCorrectly()
        {

        }

        [Fact]
        public void TaskCategory_Constructor_With_EmptyCategoryName_ShouldThrowArgumentNullException()
        {

        }

        [Fact]
        public void TaskCategory_Constructor_With_NullCategoryName_ShouldThrowArgumentNullException()
        {

        }

        #endregion

        #region Tests for properties

        [Fact]
        public void TaskCategory_SetCategoryName_WithValidValue_ShouldUpdateCategoryName()
        {

        }

        [Fact]
        public void TaskCategory_SetCategoryName_WithNullOrEmpty_ShouldThrowArgumentNullException()
        {

        }

        [Fact]
        public void TaskCategory_SetUserId_ShouldUpdateUserId()
        {

        }

        [Fact]
        public void TaskCategory_CategoryName_ShouldReturnCorrectValueAfterInitialization()
        {

        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void TaskCategory_Id_ShouldBeUnique()
        {

        }

        #endregion

        #region Tests for links

        [Fact]
        public void TaskCategory_SetUser_ShouldAssignUserCorrectly()
        {

        }

        #endregion

        #region Boundary tests

        [Fact]
        public void TaskCategory_SetCategoryName_WithEmptyString_ShouldThrowArgumentNullException()
        {

        }

        #endregion
    }
}
