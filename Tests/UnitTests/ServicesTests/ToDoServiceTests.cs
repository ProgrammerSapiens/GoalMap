namespace Tests.UnitTests.ServicesTests
{
    public class ToDoServiceTests
    {
        #region GetToDoByIdAsync(Guid toDoId) tests

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTask_WhenTaskIdExists()
        {

        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenTaskIdDoesNotExist()
        {

        }

        [Fact]
        public async Task GetToDoByIdAsync_ShouldThrowException_WhenTaskIdIsEmpty()
        {

        }

        #endregion

        #region GetToDosByUserAsync(Guid userId) tests

        [Fact]
        public async Task GetToDosByUserAsync_ShouldReturnTaskList_WhenTasksExist()
        {

        }

        [Fact]
        public async Task GetToDosByUserAsync_ShouldReturnEmptyList_WhenNoTasksExist()
        {

        }

        [Fact]
        public async Task GetToDosByUserAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region GetTodosByDateAsync(Guid userId, DateTime date) tests

        [Fact]
        public async Task GetToDosByDateAsync_ShouldReturnTasksForGivenDate_WhentasksExist()
        {

        }

        [Fact]
        public async Task GetToDosByDateAsync_ShouldReturnEmptyList_WhenNoTasksForDate()
        {

        }

        [Fact]
        public async Task GetToDosByDateAsync_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region GetToDosByTimeBlockAsync(Guid userId, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShoulReturnTasksForGivenTimeBlock_WhenTasksExist()
        {

        }

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShouldReturnEmptyList_WhenNoTasksFroTimeBlock()
        {

        }

        [Fact]
        public async Task GetToDosByTimeBlockAsync_ShouldThrowException_WhenTimeBlockIsInvalid()
        {

        }

        #endregion

        #region AddToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task AddToDoAsync_ShouldAddTaskSuccessfully_WhenTaskIsValid()
        {

        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenTaskIsInvalid()
        {

        }

        [Fact]
        public async Task AddToDoAsync_ShouldThrowException_WhenTaskIdNull()
        {

        }

        #endregion

        #region UpdateToDoAsync(ToDo toDo) tests

        [Fact]
        public async Task UpdateToDoAsync_ShouldUpdateTaskSuccessfully_WhenTaskIsValid()
        {

        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenTaskDoesNotExist()
        {

        }

        [Fact]
        public async Task UpdateToDoAsync_ShouldThrowException_WhenTaskIsNull()
        {

        }

        #endregion

        #region DeleteToDoAsync(Guid toDoId) tests

        [Fact]
        public async Task DeleteToDoAsync_ShouldDeleteTask_WhenTaskIdExists()
        {

        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenTaskIdDoesNotExist()
        {

        }

        [Fact]
        public async Task DeleteToDoAsync_ShouldThrowException_WhenTaskIdIsEmpty()
        {

        }

        #endregion

        #region MoveIncompleteToDosAsync(DateTime fromDate, DateTime toDate, Guid userId) tests

        [Fact]
        public async Task MoveIncompleteTodosAsync_ShouldMoveTasks_WhenDatesAndUserIdAreValid()
        {

        }

        [Fact]
        public async Task MoveIncompleteToDosAsync_ShouldHandleNoTasksOnFromDate()
        {

        }

        [Fact]
        public async Task MoveIncompleteToDosAsync_ShouldThrowException_WhenDateRangeIsInvalid()
        {

        }

        #endregion

        #region MoveRepeatedToDosAsync(RepeatFrequency repeatFrequency, Guid userId) tests

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldMoveTasks_WhenFrequencyAndUserIdAreValid()
        {

        }

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldHandleNoRepeatedTasks()
        {

        }

        [Fact]
        public async Task MoveRepeatedToDosAsync_ShouldThrowException_WhenFrequencyIsInvalid()
        {

        }

        #endregion
    }
}
