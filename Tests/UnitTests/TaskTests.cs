using Core.Models;
using Task = Core.Models.Task;

namespace Tests.UnitTests
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
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
            DateTime taskDate = DateTime.Now.AddDays(2);
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime earlierThenNowDeadline = DateTime.Now.AddDays(-1);
            DateTime earlierThenTaskDateDeadline = DateTime.Now.AddDays(1);

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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

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
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            DateTime newTaskDate = DateTime.Now.AddDays(1);
            task.TaskDate = newTaskDate;

            Assert.Equal(newTaskDate, task.TaskDate);
        }

        [Fact]
        public void Task_SetTaskDate_WithPastDate_ShouldThrowArgumentOutOfRangeException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(initialDescription, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            DateTime newTaskDate = DateTime.Now.AddDays(-1);

            Assert.Throws<ArgumentOutOfRangeException>(() => task.TaskDate = newTaskDate);
        }

        [Fact]
        public void Task_SetCompletionStatus_ShouldUpdateCompletionStatus()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            DateTime taskDate = DateTime.Now;
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
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Assert.False(task.CompletionStatus, "The default CompletionStatus should be false.");
        }

        [Fact]
        public void Task_Default_RepeatFrequency_ShouldBeNone()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Assert.Equal(RepeatFrequency.None, task.RepeatFrequency);
        }

        [Fact]
        public void Task_Default_Deadline_ShouldBeNull()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Assert.Null(task.Deadline);
        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void Task_Id_ShouldBeUnique()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task1 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);
            var task2 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            Assert.NotEqual(task1.Id, task2.Id);
        }

        #endregion

        #region Tests for links

        [Fact]
        public void Task_AssignTaskCategory_ShouldUpdateTaskCategory()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid userId = Guid.NewGuid();
            string categoryName = "Category name";

            var taskCategory = new TaskCategory(categoryName, userId);
            Guid taskCategoryId = taskCategory.Id;

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);
            task.TaskCategory = taskCategory;

            Assert.NotNull(task.TaskCategory);
            Assert.Equal(taskCategoryId, task.TaskCategory.Id);
            Assert.Equal("Category name", task.TaskCategory.CategoryName);
            Assert.Equal(userId, task.TaskCategory.UserId);
        }

        [Fact]
        public void Task_AssignUser_ShouldUpdateUser()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            string userName = "Test user";
            string passwordHash = "Test password";
            int experience = 100;

            var user = new User(userName, passwordHash, experience);
            Guid userId = user.Id;

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);
            task.User = user;

            Assert.NotNull(task.User);
            Assert.Equal(userId, task.User.Id);
            Assert.Equal(experience, task.User.Experience);
            Assert.Equal(passwordHash, task.User.PasswordHash);
            Assert.Equal(userName, task.User.UserName);
        }

        #endregion

        #region Boundary tests

        [Fact]
        public void Task_SetDeadline_InASecond_ShouldNotThrowException()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.UtcNow;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            var exactNow = DateTime.UtcNow.AddSeconds(1);

            var exception = Record.Exception(() => task.Deadline = exactNow);

            Assert.Null(exception);
            Assert.Equal(exactNow, task.Deadline);
        }

        [Fact]
        public void Task_SetTaskDate_OnTheBeginningOfTheDay_ShouldNotThrowException()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.UtcNow;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var task = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId);

            var beginningOfTheDay = DateTime.UtcNow.Date;

            var exception = Record.Exception(() => task.TaskDate = beginningOfTheDay);

            Assert.Null(exception);
            Assert.Equal(beginningOfTheDay, task.TaskDate);
        }

        #endregion

        #region Tests for integration with enumerations

        [Fact]
        public void Task_SetTimeBlock_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var timeBlock1 = TimeBlock.Day;
            var timeBlock2 = TimeBlock.Week;
            var timeBlock3 = TimeBlock.Month;
            var timeBlock4 = TimeBlock.Year;

            var task1 = new Task(description, timeBlock1, difficulty, taskDate, taskCategoryId, userId);
            var task2 = new Task(description, timeBlock2, difficulty, taskDate, taskCategoryId, userId);
            var task3 = new Task(description, timeBlock3, difficulty, taskDate, taskCategoryId, userId);
            var task4 = new Task(description, timeBlock4, difficulty, taskDate, taskCategoryId, userId);

            Assert.Equal(timeBlock1, task1.TimeBlock);
            Assert.Equal(timeBlock2, task2.TimeBlock);
            Assert.Equal(timeBlock3, task3.TimeBlock);
            Assert.Equal(timeBlock4, task4.TimeBlock);
        }

        [Fact]
        public void Task_SetDifficulty_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var difficulty1 = Difficulty.Easy;
            var difficulty2 = Difficulty.Middle;
            var difficulty3 = Difficulty.Hard;
            var difficulty4 = Difficulty.Nightmare;

            var task1 = new Task(description, timeBlock, difficulty1, taskDate, taskCategoryId, userId);
            var task2 = new Task(description, timeBlock, difficulty2, taskDate, taskCategoryId, userId);
            var task3 = new Task(description, timeBlock, difficulty3, taskDate, taskCategoryId, userId);
            var task4 = new Task(description, timeBlock, difficulty4, taskDate, taskCategoryId, userId);

            Assert.Equal(difficulty1, task1.Difficulty);
            Assert.Equal(difficulty2, task2.Difficulty);
            Assert.Equal(difficulty3, task3.Difficulty);
            Assert.Equal(difficulty4, task4.Difficulty);
        }

        [Fact]
        public void Task_SetRepeatFrequency_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid taskCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
            Guid parentTaskId = Guid.NewGuid();

            var repeatFrequency1 = RepeatFrequency.None;
            var repeatFrequency2 = RepeatFrequency.Daily;
            var repeatFrequency3 = RepeatFrequency.Weekly;
            var repeatFrequency4 = RepeatFrequency.Monthly;
            var repeatFrequency5 = RepeatFrequency.Yearly;

            var task1 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency1);
            var task2 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency2);
            var task3 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency3);
            var task4 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency4);
            var task5 = new Task(description, timeBlock, difficulty, taskDate, taskCategoryId, userId, deadline, parentTaskId, repeatFrequency5);

            Assert.Equal(repeatFrequency1, task1.RepeatFrequency);
            Assert.Equal(repeatFrequency2, task2.RepeatFrequency);
            Assert.Equal(repeatFrequency3, task3.RepeatFrequency);
            Assert.Equal(repeatFrequency4, task4.RepeatFrequency);
            Assert.Equal(repeatFrequency5, task5.RepeatFrequency);
        }

        #endregion
    }
}