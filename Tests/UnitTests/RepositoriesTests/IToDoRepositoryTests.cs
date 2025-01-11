using Core.Interfaces;

namespace Tests.UnitTests.RepositoriesTests
{
    public class IToDoRepositoryTests
    {
        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnToDo_WhenToDoExists()
        {

        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldReturnNull_WhenToDoDoesNotExist()
        {

        }

        #endregion

        #region GetToDosAsync(Guid userId, DateTime date, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDosAsync_ShouldReturnToDos_WhenToDosExistForUserAndTimeBlock()
        {

        }

        [Fact]
        public async Task GetToDosAsync_ShouldReturnEmptyList_WhenNoToDosExistForUserAndTimeBlock()
        {

        }

        #endregion

        #region AddToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task AddToDoAsync_ShouldAddToDo_WhenDataIsValid()
        {

        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenToDoIdAlreadyExists()
        {

        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateToDo_WhenToDoExists()
        {

        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenToDoDoesNotExist()
        {

        }

        #endregion

        #region UpdateToDosAsync(IEnumerable<ToDo> toDos) tests

        [Fact]
        public async Task UpdateToDosAsync_ShouldUpdateAllToDos_WhenAllToDosExist()
        {

        }

        [Fact]
        public async Task UpdateToDosAsync_ShouldThrowException_WhenAnyToDoDoesNotExist()
        {

        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteToDo_WhenToDoExists()
        {

        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldDoNothing_WhenToDoDoesNotExist()
        {

        }

        #endregion

        #region IsToDoExistsAsync(Guid toDoId) tests

        [Fact]
        public async Task IsToDoExistsAsync_ShouldReturnTrue_WhenToDoExists()
        {

        }

        [Fact]
        public async Task IsToDoExistsAsync_ShouldReturnFalse_WhenToDoDoesNotExist()
        {

        }

        #endregion
    }
}
