using Core.Models;
using Task = Core.Models.Task;

namespace Tests
{
    public class TaskTests
    {
        #region Tests for constructors

        [Fact]
        public void Task_Constructor_With_DescriptionAndTimeBlockAndDifficultyAndTaskDate_ShouldInitializePropertiesCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Assert.Equal(description, task.Description);
            Assert.Equal(timeBlock, task.TimeBlock);
            Assert.Equal(difficulty, task.Difficulty);
            Assert.Equal(taskDate, task.TaskDate);
            Assert.Equal(taskCategoryId, task.TaskCategoryId);
            Assert.Equal(userId, task.UserId);
            Assert.NotEqual(Guid.Empty, task.Id);
        }

        [Fact]
        public void Task_Constructor_With_Deadline_ShouldInitializeDeadlineCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(6);

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline);

            Assert.Equal(description, task.Description);
            Assert.Equal(timeBlock, task.TimeBlock);
            Assert.Equal(difficulty, task.Difficulty);
            Assert.Equal(taskDate, task.TaskDate);
            Assert.Equal(taskCategoryId, task.TaskCategoryId);
            Assert.Equal(userId, task.UserId);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.Equal(deadline, task.Deadline);
        }

        [Fact]
        public void Task_Constructor_With_ParentTaskId_ShouldInitializeParentTaskIdCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(7);
            Guid parentTaskId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId);

            Assert.Equal(description, task.Description);
            Assert.Equal(timeBlock, task.TimeBlock);
            Assert.Equal(difficulty, task.Difficulty);
            Assert.Equal(taskDate, task.TaskDate);
            Assert.Equal(taskCategoryId, task.TaskCategoryId);
            Assert.Equal(userId, task.UserId);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.Equal(deadline, task.Deadline);
            Assert.Equal(parentTaskId, task.ParentTaskId);
        }

        [Fact]
        public void Task_Constructor_With_RepeatFrequency_ShouldInitializeRepeatFrequencyCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(7);
            Guid parentTaskId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Weekly;

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency);

            Assert.Equal(description, task.Description);
            Assert.Equal(timeBlock, task.TimeBlock);
            Assert.Equal(difficulty, task.Difficulty);
            Assert.Equal(taskDate, task.TaskDate);
            Assert.Equal(taskCategoryId, task.TaskCategoryId);
            Assert.Equal(userId, task.UserId);
            Assert.NotEqual(Guid.Empty, task.Id);
            Assert.Equal(deadline, task.Deadline);
            Assert.Equal(parentTaskId, task.ParentTaskId);
            Assert.Equal(repeatFrequency, task.RepeatFrequency);
        }

        [Fact]
        public void Task_Constructors_With_NullDescription_ShouldThrowArgumentException()
        {
            string description = string.Empty;
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(7);
            Guid parentTaskId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Weekly;

            Assert.Throws<ArgumentException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId));
            Assert.Throws<ArgumentException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline));
            Assert.Throws<ArgumentException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId));
            Assert.Throws<ArgumentException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency));
        }

        [Fact]
        public void Task_Constructor_With_Deadline_ShouldThrowArgumentAndArgumentOutOfRangeExceptions()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(5);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime earlierThenNowDeadline = DateTime.Now.AddDays(-5);
            DateTime earlierThenTaskDateDeadline = DateTime.Now.AddDays(4);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, earlierThenNowDeadline));
            Assert.Throws<ArgumentException>(() => new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, earlierThenTaskDateDeadline));
        }

        #endregion

        #region Tests for properties

        [Fact]
        public void Task_SetDescription_WithValidValue_ShouldUpdateDescription()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            string newDescription = "Updated description";
            task.Description = newDescription;

            Assert.Equal(newDescription, task.Description);
        }

        [Fact]
        public void Task_SetDescription_WithNullOrEmpty_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            string newDescription = "";

            Assert.Throws<ArgumentException>(() => task.Description = newDescription);
        }

        [Fact]
        public void Task_SetDeadline_WithFutureDate_ShouldUpdateDeadline()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(3);

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(4);
            task.Deadline = newDeadline;

            Assert.Equal(newDeadline, task.Deadline);
        }

        [Fact]
        public void Task_SetDeadlineWithPastDate_ShouldThrowArgumentOutOfRangeException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(3);

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(-1);

            Assert.Throws<ArgumentOutOfRangeException>(() => task.Deadline = newDeadline);
        }

        [Fact]
        public void Task_SetDeadlineWithLessDateThanTaskDate_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(3);

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(1);

            Assert.Throws<ArgumentException>(() => task.Deadline = newDeadline);
        }

        [Fact]
        public void Task_SetTaskDate_WithFutureDate_ShouldUpdateTaskDate()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            DateTime newTaskDate = DateTime.Now.AddDays(3);
            task.TaskDate = newTaskDate;

            Assert.Equal(newTaskDate, task.TaskDate);
        }

        [Fact]
        public void Task_SetTaskDate_WithPastDate_ShouldThrowArgumentOutOfRangeException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            DateTime newTaskDate = DateTime.Now.AddDays(-3);

            Assert.Throws<ArgumentOutOfRangeException>(() => task.TaskDate = newTaskDate);
        }

        [Fact]
        public void Task_SetCompletionStatus_ShouldUpdateCompletionStatus()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            task.CompletionStatus = true;

            Assert.True(task.CompletionStatus);
        }

        [Fact]
        public void Task_SetRepeatFrequency_ShouldUpdateRepeatFrequency()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            task.RepeatFrequency = RepeatFrequency.Daily;

            Assert.Equal(RepeatFrequency.Daily, task.RepeatFrequency);
        }

        [Fact]
        public void Task_SetTaskCategoryId_ShouldUpdateTaskCategoryId()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Guid newTaskCategoryId = Guid.NewGuid();
            task.TaskCategoryId = newTaskCategoryId;

            Assert.Equal(newTaskCategoryId, task.TaskCategoryId);
        }

        [Fact]
        public void Task_SetEmptyTaskCategoryId_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Guid newTaskCategoryId = new Guid();

            Assert.Throws<ArgumentException>(() => task.TaskCategoryId = newTaskCategoryId);
        }

        [Fact]
        public void Task_SetUserId_ShouldUpdateUserId()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Guid newUserId = Guid.NewGuid();
            task.UserId = newUserId;

            Assert.Equal(newUserId, task.UserId);
        }

        [Fact]
        public void Task_SetEmptyUserId_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Guid newUserId = new Guid();

            Assert.Throws<ArgumentException>(() => task.UserId = newUserId);
        }

        #endregion

        #region Tests for default values

        [Fact]
        public void Task_Default_CompletionStatus_ShouldBeFalse()
        {

        }

        [Fact]
        public void Task_Default_RepeatFrequency_ShouldBeNone()
        {

        }

        [Fact]
        public void Task_Default_Deadline_ShouldBeNull()
        {

        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void Task_Id_ShouldBeUnique()
        {

        }

        #endregion

        #region Tests for links

        [Fact]
        public void Task_AssignTaskCategory_ShouldUpdateTaskCategory()
        {

        }

        [Fact]
        public void Task_AssignUser_ShouldUpdateUser()
        {

        }

        #endregion

        #region Borderlines tests

        [Fact]
        public void Task_SetDeadline_WithExactNow_ShouldNotThrowException()
        {

        }

        [Fact]
        public void Task_SetTaskDate_WithExactNow_ShouldNotThrowException()
        {

        }

        #endregion

        #region Tests for integration with enumerations

        [Fact]
        public void Task_SetTimeBlock_ShouldAcceptValidEnumValue()
        {

        }

        [Fact]
        public void Task_SetDifficulty_ShouldAcceptValidEnumValue()
        {

        }

        [Fact]
        public void Task_SetRepeatFrequency_ShouldAcceptValidEnumValue()
        {

        }

        #endregion
    }
}