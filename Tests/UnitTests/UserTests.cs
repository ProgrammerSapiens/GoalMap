using Core.Models;

namespace Tests.UnitTests
{
    public class UserTests
    {
        #region Tests for constructors

        [Fact]
        public void User_Constructor_With_ValidParameters_ShouldInitializePropertiesCorrectly()
        {

        }

        [Fact]
        public void User_Constructor_With_NegativeExperience_ShouldThrowArgumentOutOfRangeException()
        {

        }

        [Fact]
        public void User_Constructor_With_EmptyUserName_ShouldThrowArgumentException()
        {

        }

        [Fact]
        public void User_Constructor_With_Empty_PasswordHash_ShouldThrowArgumentException()
        {

        }

        #endregion

        #region Tests for properties

        [Fact]
        public void User_SetUserName_WithValidValue_ShouldUpdateUserName()
        {

        }

        [Fact]
        public void User_SetUserName_WithNullOrEmpty_ShouldThrowArgumentException()
        {

        }

        [Fact]
        public void User_SetPasswordHash_WithValidValue_ShouldUpdatePasswordHash()
        {

        }

        [Fact]
        public void User_SetPasswordHash_WithNullOrEmpty_ShouldThrowArgumentException()
        {

        }

        [Fact]
        public void User_SetExperience_WithValidValue_ShouldUpdateExperience()
        {

        }

        [Fact]
        public void User_SetExperience_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
        {

        }

        [Fact]
        public void User_Level_ShouldReturnCorrectLevelBasedOnExperience()
        {

        }

        [Fact]
        public void User_SetTasks_ShouldAssignTasksCorrectly()
        {

        }

        [Fact]
        public void User_SetCategories_ShouldAssignCategoriesCorrectly()
        {

        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void User_Id_ShouldBeUnique()
        {

        }

        #endregion

        #region Tests for links

        [Fact]
        public void User_AssignTasks_ShouldUpdateTasksCollection()
        {

        }

        [Fact]
        public void User_AssignCategories_ShouldUpdateCategoriesCollection()
        {

        }

        #endregion

        #region Boundary tests

        [Fact]
        public void User_SetExperience_WithZero_ShouldNotThrowException()
        {

        }

        [Fact]
        public void User_SetUserName_WithEmptyString_ShouldThrowArgumentException()
        {

        }

        [Fact]
        public void User_SetPasswordHash_WithEmptyString_ShouldThrowArguentException()
        {

        }

        #endregion
    }
}
