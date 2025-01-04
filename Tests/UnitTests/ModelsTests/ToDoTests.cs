using Core.Models;

namespace Tests.UnitTests.ModelsTests
{
    public class ToDoTests
    {
        #region Tests for constructors

        [Fact]
        public void ToDo_Constructor_With_DescriptionAndTimeBlockAndDifficultyAndTaskDate_ShouldInitializePropertiesCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.Equal(description, toDo.Description);
            Assert.Equal(timeBlock, toDo.TimeBlock);
            Assert.Equal(difficulty, toDo.Difficulty);
            Assert.Equal(toDoDate, toDo.ToDoDate);
            Assert.Equal(toDoCategoryId, toDo.ToDoCategoryId);
            Assert.Equal(userId, toDo.UserId);
            Assert.NotEqual(Guid.Empty, toDo.Id);
        }

        [Fact]
        public void ToDo_Constructor_With_Deadline_ShouldInitializeDeadlineCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline);

            Assert.Equal(description, toDo.Description);
            Assert.Equal(timeBlock, toDo.TimeBlock);
            Assert.Equal(difficulty, toDo.Difficulty);
            Assert.Equal(toDoDate, toDo.ToDoDate);
            Assert.Equal(toDoCategoryId, toDo.ToDoCategoryId);
            Assert.Equal(userId, toDo.UserId);
            Assert.NotEqual(Guid.Empty, toDo.Id);
            Assert.Equal(deadline, toDo.Deadline);
        }

        [Fact]
        public void ToDo_Constructor_With_ParentTaskId_ShouldInitializeParentTaskIdCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
            Guid parentToDoId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId);

            Assert.Equal(description, toDo.Description);
            Assert.Equal(timeBlock, toDo.TimeBlock);
            Assert.Equal(difficulty, toDo.Difficulty);
            Assert.Equal(toDoDate, toDo.ToDoDate);
            Assert.Equal(toDoCategoryId, toDo.ToDoCategoryId);
            Assert.Equal(userId, toDo.UserId);
            Assert.NotEqual(Guid.Empty, toDo.Id);
            Assert.Equal(deadline, toDo.Deadline);
            Assert.Equal(parentToDoId, toDo.ParentToDoId);
        }

        [Fact]
        public void ToDo_Constructor_With_RepeatFrequency_ShouldInitializeRepeatFrequencyCorrectly()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
            Guid parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Weekly;

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency);

            Assert.Equal(description, toDo.Description);
            Assert.Equal(timeBlock, toDo.TimeBlock);
            Assert.Equal(difficulty, toDo.Difficulty);
            Assert.Equal(toDoDate, toDo.ToDoDate);
            Assert.Equal(toDoCategoryId, toDo.ToDoCategoryId);
            Assert.Equal(userId, toDo.UserId);
            Assert.NotEqual(Guid.Empty, toDo.Id);
            Assert.Equal(deadline, toDo.Deadline);
            Assert.Equal(parentToDoId, toDo.ParentToDoId);
            Assert.Equal(repeatFrequency, toDo.RepeatFrequency);
        }

        [Fact]
        public void ToDo_Constructors_With_NullDescription_ShouldThrowArgumentException()
        {
            string description = string.Empty;
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
            Guid parentToDoId = Guid.NewGuid();
            RepeatFrequency repeatFrequency = RepeatFrequency.Weekly;

            Assert.Throws<ArgumentException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId));
            Assert.Throws<ArgumentException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline));
            Assert.Throws<ArgumentException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId));
            Assert.Throws<ArgumentException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency));
        }

        [Fact]
        public void ToDo_Constructor_With_Deadline_ShouldThrowArgumentAndArgumentOutOfRangeExceptions()
        {
            string description = "Test description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now.AddDays(2);
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime earlierThenNowDeadline = DateTime.Now.AddDays(-1);
            DateTime earlierThenToDoDateDeadline = DateTime.Now.AddDays(1);

            Assert.Throws<ArgumentOutOfRangeException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, earlierThenNowDeadline));
            Assert.Throws<ArgumentException>(() => new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, earlierThenToDoDateDeadline));
        }

        #endregion

        #region Tests for properties

        [Fact]
        public void ToDo_SetDescription_WithValidValue_ShouldUpdateDescription()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            string newDescription = "Updated description";
            toDo.Description = newDescription;

            Assert.Equal(newDescription, toDo.Description);
        }

        [Fact]
        public void ToDo_SetDescription_WithNullOrEmpty_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            string newDescription = "";

            Assert.Throws<ArgumentException>(() => toDo.Description = newDescription);
        }

        [Fact]
        public void ToDo_SetDeadline_WithFutureDate_ShouldUpdateDeadline()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(4);
            toDo.Deadline = newDeadline;

            Assert.Equal(newDeadline, toDo.Deadline);
        }

        [Fact]
        public void ToDo_SetDeadlineWithPastDate_ShouldThrowArgumentOutOfRangeException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(-1);

            Assert.Throws<ArgumentOutOfRangeException>(() => toDo.Deadline = newDeadline);
        }

        [Fact]
        public void ToDo_SetDeadlineWithLessDateThanTaskDate_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now.AddDays(2);
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(3);

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline);

            DateTime newDeadline = DateTime.Now.AddDays(1);

            Assert.Throws<ArgumentException>(() => toDo.Deadline = newDeadline);
        }

        [Fact]
        public void ToDo_SetTaskDate_WithFutureDate_ShouldUpdateTaskDate()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            DateTime newToDoDate = DateTime.Now.AddDays(1);
            toDo.ToDoDate = newToDoDate;

            Assert.Equal(newToDoDate, toDo.ToDoDate);
        }

        [Fact]
        public void ToDo_SetTaskDate_WithPastDate_ShouldThrowArgumentOutOfRangeException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            DateTime newToDoDate = DateTime.Now.AddDays(-1);

            Assert.Throws<ArgumentOutOfRangeException>(() => toDo.ToDoDate = newToDoDate);
        }

        [Fact]
        public void ToDo_SetCompletionStatus_ShouldUpdateCompletionStatus()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            toDo.CompletionStatus = true;

            Assert.True(toDo.CompletionStatus);
        }

        [Fact]
        public void ToDo_SetRepeatFrequency_ShouldUpdateRepeatFrequency()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            toDo.RepeatFrequency = RepeatFrequency.Daily;

            Assert.Equal(RepeatFrequency.Daily, toDo.RepeatFrequency);
        }

        [Fact]
        public void ToDo_SetTaskCategoryId_ShouldUpdateTaskCategoryId()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Guid newToDoCategoryId = Guid.NewGuid();
            toDo.ToDoCategoryId = newToDoCategoryId;

            Assert.Equal(newToDoCategoryId, toDo.ToDoCategoryId);
        }

        [Fact]
        public void ToDo_SetEmptyTaskCategoryId_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Guid newToDoCategoryId = new Guid();

            Assert.Throws<ArgumentException>(() => toDo.ToDoCategoryId = newToDoCategoryId);
        }

        [Fact]
        public void ToDo_SetUserId_ShouldUpdateUserId()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Guid newUserId = Guid.NewGuid();
            toDo.UserId = newUserId;

            Assert.Equal(newUserId, toDo.UserId);
        }

        [Fact]
        public void ToDo_SetEmptyUserId_ShouldThrowArgumentException()
        {
            string initialDescription = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Middle;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(initialDescription, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Guid newUserId = new Guid();

            Assert.Throws<ArgumentException>(() => toDo.UserId = newUserId);
        }

        #endregion

        #region Tests for default values

        [Fact]
        public void ToDo_Default_CompletionStatus_ShouldBeFalse()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.False(toDo.CompletionStatus, "The default CompletionStatus should be false.");
        }

        [Fact]
        public void ToDo_Default_RepeatFrequency_ShouldBeNone()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.Equal(RepeatFrequency.None, toDo.RepeatFrequency);
        }

        [Fact]
        public void ToDo_Default_Deadline_ShouldBeNull()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.Null(toDo.Deadline);
        }

        #endregion

        #region The identifier uniqueness test

        [Fact]
        public void ToDo_Id_ShouldBeUnique()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo1 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);
            var toDo2 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.NotEqual(toDo1.Id, toDo2.Id);
        }

        #endregion

        #region Tests for links

        [Fact]
        public void ToDo_AssignTaskCategory_ShouldUpdateTaskCategory()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid userId = Guid.NewGuid();
            string toDoCategoryName = "Category name";

            var toDoCategory = new ToDoCategory(toDoCategoryName, userId);
            Guid toDoCategoryId = toDoCategory.ToDoCategoryId;

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);
            toDo.ToDoCategory = toDoCategory;

            Assert.NotNull(toDo.ToDoCategory);
            Assert.Equal(toDoCategoryId, toDo.ToDoCategory.ToDoCategoryId);
            Assert.Equal("Category name", toDo.ToDoCategory.ToDoCategoryName);
            Assert.Equal(userId, toDo.ToDoCategory.UserId);
        }

        [Fact]
        public void ToDo_AssignUser_ShouldUpdateUser()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime taskDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            string userName = "Test user";
            string passwordHash = "Test password";
            int experience = 100;

            var user = new User(userName, passwordHash, experience);
            Guid userId = user.Id;

            var toDo = new ToDo(description, timeBlock, difficulty, taskDate, toDoCategoryId, userId);
            toDo.User = user;

            Assert.NotNull(toDo.User);
            Assert.Equal(userId, toDo.User.Id);
            Assert.Equal(experience, toDo.User.Experience);
            Assert.Equal(passwordHash, toDo.User.PasswordHash);
            Assert.Equal(userName, toDo.User.UserName);
        }

        #endregion

        #region Boundary tests

        [Fact]
        public void ToDo_SetDeadline_InASecond_ShouldNotThrowException()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.UtcNow;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            var exactNow = DateTime.UtcNow.AddSeconds(1);

            var exception = Record.Exception(() => toDo.Deadline = exactNow);

            Assert.Null(exception);
            Assert.Equal(exactNow, toDo.Deadline);
        }

        [Fact]
        public void ToDo_SetTaskDate_OnTheBeginningOfTheDay_ShouldNotThrowException()
        {
            string description = "Initial description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.UtcNow;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var toDo = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId);

            var beginningOfTheDay = DateTime.UtcNow.Date;

            var exception = Record.Exception(() => toDo.ToDoDate = beginningOfTheDay);

            Assert.Null(exception);
            Assert.Equal(beginningOfTheDay, toDo.ToDoDate);
        }

        #endregion

        #region Tests for integration with enumerations

        [Fact]
        public void ToDo_SetTimeBlock_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var timeBlock1 = TimeBlock.Day;
            var timeBlock2 = TimeBlock.Week;
            var timeBlock3 = TimeBlock.Month;
            var timeBlock4 = TimeBlock.Year;

            var toDo1 = new ToDo(description, timeBlock1, difficulty, toDoDate, toDoCategoryId, userId);
            var toDo2 = new ToDo(description, timeBlock2, difficulty, toDoDate, toDoCategoryId, userId);
            var toDo3 = new ToDo(description, timeBlock3, difficulty, toDoDate, toDoCategoryId, userId);
            var toDo4 = new ToDo(description, timeBlock4, difficulty, toDoDate, toDoCategoryId, userId);

            Assert.Equal(timeBlock1, toDo1.TimeBlock);
            Assert.Equal(timeBlock2, toDo2.TimeBlock);
            Assert.Equal(timeBlock3, toDo3.TimeBlock);
            Assert.Equal(timeBlock4, toDo4.TimeBlock);
        }

        [Fact]
        public void ToDo_SetDifficulty_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var difficulty1 = Difficulty.Easy;
            var difficulty2 = Difficulty.Middle;
            var difficulty3 = Difficulty.Hard;
            var difficulty4 = Difficulty.Nightmare;

            var toDo1 = new ToDo(description, timeBlock, difficulty1, toDoDate, toDoCategoryId, userId);
            var toDo2 = new ToDo(description, timeBlock, difficulty2, toDoDate, toDoCategoryId, userId);
            var toDo3 = new ToDo(description, timeBlock, difficulty3, toDoDate, toDoCategoryId, userId);
            var toDo4 = new ToDo(description, timeBlock, difficulty4, toDoDate, toDoCategoryId, userId);

            Assert.Equal(difficulty1, toDo1.Difficulty);
            Assert.Equal(difficulty2, toDo2.Difficulty);
            Assert.Equal(difficulty3, toDo3.Difficulty);
            Assert.Equal(difficulty4, toDo4.Difficulty);
        }

        [Fact]
        public void ToDo_SetRepeatFrequency_ShouldAcceptValidEnumValue()
        {
            string description = "Task description";
            TimeBlock timeBlock = TimeBlock.Day;
            Difficulty difficulty = Difficulty.Easy;
            DateTime toDoDate = DateTime.Now;
            Guid toDoCategoryId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            DateTime deadline = DateTime.Now.AddDays(1);
            Guid parentToDoId = Guid.NewGuid();

            var repeatFrequency1 = RepeatFrequency.None;
            var repeatFrequency2 = RepeatFrequency.Daily;
            var repeatFrequency3 = RepeatFrequency.Weekly;
            var repeatFrequency4 = RepeatFrequency.Monthly;
            var repeatFrequency5 = RepeatFrequency.Yearly;

            var toDo1 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency1);
            var toDo2 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency2);
            var toDo3 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency3);
            var toDo4 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency4);
            var toDo5 = new ToDo(description, timeBlock, difficulty, toDoDate, toDoCategoryId, userId, deadline, parentToDoId, repeatFrequency5);

            Assert.Equal(repeatFrequency1, toDo1.RepeatFrequency);
            Assert.Equal(repeatFrequency2, toDo2.RepeatFrequency);
            Assert.Equal(repeatFrequency3, toDo3.RepeatFrequency);
            Assert.Equal(repeatFrequency4, toDo4.RepeatFrequency);
            Assert.Equal(repeatFrequency5, toDo5.RepeatFrequency);
        }

        #endregion
    }
}