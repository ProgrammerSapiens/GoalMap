using Core.Models;

namespace Tests.UnitTests.ModelsTests
{
    public class UserTests
    {
        #region Tests for constructors

        [Fact]
        public void User_Constructor_With_ValidParameters_ShouldInitializePropertiesCorrectly()
        {
            string userName = "Test user";

            var user = new User(userName);

            Assert.NotEqual(Guid.Empty, user.UserId);
            Assert.Equal(userName, user.UserName);
            Assert.Equal("NotHashed", user.PasswordHash);
        }

        [Fact]
        public void User_Constructor_With_EmptyUserName_ShouldThrowArgumentException() => Assert.Throws<ArgumentException>(() => new User(string.Empty));

        #endregion

        #region Tests for properties

        [Fact]
        public void User_SetUserName_WithValidValue_ShouldUpdateUserName()
        {
            string userName = "Test user";

            var user = new User(userName);

            string newUserName = "New test user";
            user.UserName = newUserName;

            Assert.Equal(newUserName, user.UserName);
        }

        [Fact]
        public void User_SetUserName_WithNullOrEmpty_ShouldThrowArgumentException()
        {
            string userName = "Test user";

            var user = new User(userName);

            string newUserName = string.Empty;

            Assert.Throws<ArgumentException>(() => user.UserName = newUserName);
        }

        [Fact]
        public void User_SetPasswordHash_WithValidValue_ShouldUpdatePasswordHash()
        {
            string userName = "Test user";

            var user = new User(userName);

            string newPasswordHash = "New test password";
            user.PasswordHash = newPasswordHash;

            Assert.Equal(newPasswordHash, user.PasswordHash);
        }

        [Fact]
        public void User_SetPasswordHash_WithNullOrEmpty_ShouldThrowOutOfRangeException()
        {
            string userName = "Test user";

            var user = new User(userName);

            string newPasswordHash = string.Empty;

            Assert.Throws<ArgumentOutOfRangeException>(() => user.PasswordHash = newPasswordHash);
        }

        [Fact]
        public void User_SetExperience_WithValidValue_ShouldUpdateExperience()
        {
            string userName = "Test user";

            var user = new User(userName);

            int newExperience = user.Experience + 100;
            user.Experience = newExperience;

            Assert.Equal(newExperience, user.Experience);
        }

        [Fact]
        public void User_SetExperience_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
        {
            string userName = "Test user";

            var user = new User(userName);

            int newExperience = -1;

            Assert.Throws<ArgumentOutOfRangeException>(() => user.Experience = newExperience);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(99, 0)]
        [InlineData(100, 1)]
        [InlineData(400, 2)]
        [InlineData(900, 3)]
        [InlineData(1600, 4)]
        public void User_Level_ShouldReturnCorrectLevelBasedOnExperience(int experience, int expectedLevel)
        {
            string userName = "Test user";

            var user = new User(userName) { Experience = experience };

            int actualLevel = user.Level;

            Assert.Equal(expectedLevel, actualLevel);
        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void User_Id_ShouldBeUnique()
        {
            string userName = "Test user";

            var user1 = new User(userName);
            var user2 = new User(userName);

            Assert.NotEqual(user1, user2);
        }

        #endregion
    }
}
