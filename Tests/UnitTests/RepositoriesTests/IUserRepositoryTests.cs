using Core.Interfaces;

namespace Tests.UnitTests.RepositoriesTests
{
    public class IUserRepositoryTests
    {
        #region GetUserByUserNameAsync(string userName) tests

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnUser_WhenUserExists()
        {

        }

        [Fact]
        public async Task GetUserByUserNameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {

        }

        #endregion

        #region AddUserAsync(User user) tests

        [Fact]
        public async Task AddUserAsync_ShouldAddUser_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddUserAsync_ShouldThrowException_WhenUserNameAlreadyExists()
        {

        }

        #endregion

        #region UpdateUserAsync(User user) tests

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
        {

        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserDoesNotExist()
        {

        }

        #endregion

        #region IsUserExistsAsync(string username) tests

        [Fact]
        public async Task IsUserExists_ShouldReturnTrue_WhenUserExists()
        {

        }

        [Fact]
        public async Task IsUserExists_ShouldReturnFalse_WhenUserDoesNotExist()
        {

        }

        #endregion
    }
}
