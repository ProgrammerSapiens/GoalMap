namespace Tests.UnitTests.ServicesTests
{
    public class ToDoServiceTests
    {
        #region GetToDoByIdAsync(Guid taskId) tests

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTask_WhenTaskIdExists()
        {

        }

        [Fact]
        public async Task GetTaskById_ShouldThrowException_WhenTaskIdDoesNotExist()
        {

        }

        [Fact]
        public async Task GetTaskById_ShouldThrowException_WhenTaskIdIsEmpty()
        {

        }

        #endregion

        #region GetToDoByUserAsync(Guid userId) tests

        [Fact]
        public async Task GetTaskByUser_ShouldReturnTaskList_WhenTasksExist()
        {

        }

        [Fact]
        public async Task GetTaskByUser_ShouldReturnEmptyList_WhenNoTasksExist()
        {

        }

        [Fact]
        public async Task GetTasksByUser_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region GetTodoByDateAsync(Guid userId, DateTime date) tests

        [Fact]
        public async Task GetTasksByDate_ShouldReturnTasksForGivenDate_WhentasksExist()
        {

        }

        [Fact]
        public async Task GetTasksByDate_ShouldReturnEmptyList_WhenNoTasksForDate()
        {

        }

        [Fact]
        public async Task GetTasksByDate_ShouldThrowException_WhenUserIdDoesNotExist()
        {

        }

        #endregion

        #region GetToDoByTimeBlockAsync(Guid userId, TimeBlock timeBlock) tests

        [Fact]
        public async Task GetTasksByTimeBlock_ShoulReturnTasksForGivenTimeBlock_WhenTasksExist()
        {

        }

        [Fact]
        public async Task GetTasksByTimeBlock_ShouldReturnEmptyList_WhenNoTasksFroTimeBlock()
        {

        }

        [Fact]
        public async Task GetTasksByTimeBlock_ShouldThrowException_WhenTimeBlockIsInvalid()
        {

        }

        #endregion

        #region AddToDoAsync(ToDo task) tests

        [Fact]
        public async Task AddTask_ShouldAddTaskSuccessfully_WhenTaskIsValid()
        {

        }

        [Fact]
        public async Task AddTask_ShouldThrowException_WhenTaskIsInvalid()
        {

        }

        [Fact]
        public async Task AddTask_ShouldThrowException_WhenTaskIdNull()
        {

        }

        #endregion

        #region UpdateToDoAsync(ToDo task) tests

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskSuccessfully_WhenTaskIsValid()
        {

        }

        [Fact]
        public async Task UpdateTask_ShouldThrowException_WhenTaskDoesNotExist()
        {

        }

        [Fact]
        public async Task UpdateTask_ShouldThrowException_WhenTaskIsNull()
        {

        }

        #endregion

        #region DeleteToDoAsync(Guid taskId) tests

        [Fact]
        public async Task DeleteTask_ShouldDeleteTask_WhenTaskIdExists()
        {

        }

        [Fact]
        public async Task DeleteTask_ShouldThrowException_WhenTaskIdDoesNotExist()
        {

        }

        [Fact]
        public async Task DeleteTask_ShouldThrowException_WhenTaskIdIsEmpty()
        {

        }

        #endregion

        #region MoveIncompleteToDoAsync(DateTime fromDate, DateTime toDate, Guid userId) tests

        [Fact]
        public async Task MoveIncompleteTasks_ShouldMoveTasks_WhenDatesAndUserIdAreValid()
        {

        }

        [Fact]
        public async Task MoveIncompleteTasks_ShouldHandleNoTasksOnFromDate()
        {

        }

        [Fact]
        public async Task MoveIncompleteTasks_ShouldThrowException_WhenDateRangeIsInvalid()
        {

        }

        #endregion

        #region MoveRepeatedToDoAsync(RepeatFrequency repeatFrequency, Guid userId) tests

        [Fact]
        public async Task MoveRepeatedTasks_ShouldMoveTasks_WhenFrequencyAndUserIdAreValid()
        {

        }

        [Fact]
        public async Task MoveRepeatedTasks_ShouldHandleNoRepeatedTasks()
        {

        }

        [Fact]
        public async Task MoveRepeatedTasks_ShouldThrowException_WhenFrequencyIsInvalid()
        {

        }

        #endregion
    }
}
