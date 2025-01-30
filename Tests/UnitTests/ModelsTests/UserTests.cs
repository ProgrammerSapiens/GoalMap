﻿using Core.Models;

namespace Tests.UnitTests.ModelsTests
{
    public class UserTests
    {
        #region Tests for constructors

        //[Fact]
        //public void User_Constructor_With_ValidParameters_ShouldInitializePropertiesCorrectly()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";

        //    var user = new User(userName, passwordHash);

        //    Assert.NotEqual(Guid.Empty, user.UserId);
        //    Assert.Equal(userName, user.UserName);
        //    Assert.Equal(passwordHash, user.PasswordHash);
        //}

        //[Fact]
        //public void User_Constructor_With_EmptyUserName_ShouldThrowArgumentException()
        //{
        //    string passwordHash = "Test password";

        //    Assert.Throws<ArgumentException>(() => new User(string.Empty, passwordHash));
        //}

        //[Fact]
        //public void User_Constructor_With_Empty_PasswordHash_ShouldThrowArgumentException()
        //{
        //    string userName = "Test user";
        //    int experience = 100;

        //    Assert.Throws<ArgumentException>(() => new User(userName, string.Empty));
        //}

        #endregion

        #region Tests for properties

        //[Fact]
        //public void User_SetUserName_WithValidValue_ShouldUpdateUserName()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";

        //    var user = new User(userName, passwordHash);

        //    string newUserName = "New test user";
        //    user.UserName = newUserName;

        //    Assert.Equal(newUserName, user.UserName);
        //}

        //[Fact]
        //public void User_SetUserName_WithNullOrEmpty_ShouldThrowArgumentException()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user = new User(userName, passwordHash);

        //    string newUserName = string.Empty;

        //    Assert.Throws<ArgumentException>(() => user.UserName = newUserName);
        //}

        //[Fact]
        //public void User_SetPasswordHash_WithValidValue_ShouldUpdatePasswordHash()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user = new User(userName, passwordHash);

        //    string newPasswordHash = "New test password";
        //    user.PasswordHash = newPasswordHash;

        //    Assert.Equal(newPasswordHash, user.PasswordHash);
        //}

        //[Fact]
        //public void User_SetPasswordHash_WithNullOrEmpty_ShouldThrowArgumentException()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user = new User(userName, passwordHash);

        //    string newPasswordHash = string.Empty;

        //    Assert.Throws<ArgumentOutOfRangeException>(() => user.PasswordHash = newPasswordHash);
        //}

        //[Fact]
        //public void User_SetExperience_WithValidValue_ShouldUpdateExperience()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user = new User(userName, passwordHash);

        //    int newExperience = user.Experience + 100;
        //    user.Experience = newExperience;

        //    Assert.Equal(newExperience, user.Experience);
        //}

        //[Fact]
        //public void User_SetExperience_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user = new User(userName, passwordHash);

        //    int newExperience = -1;

        //    Assert.Throws<ArgumentOutOfRangeException>(() => user.Experience = newExperience);
        //}

        //[Theory]
        //[InlineData(0, 0)]
        //[InlineData(99, 0)]
        //[InlineData(100, 1)]
        //[InlineData(400, 2)]
        //[InlineData(900, 3)]
        //[InlineData(1600, 4)]
        //public void User_Level_ShouldReturnCorrectLevelBasedOnExperience(int experience, int expectedLevel)
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";

        //    var user = new User(userName, passwordHash);

        //    int actualLevel = user.Level;

        //    Assert.Equal(expectedLevel, actualLevel);
        //}

        //#endregion

        //#region The identifier uniqueness test

        //[Fact]
        //public void User_Id_ShouldBeUnique()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;

        //    var user1 = new User(userName, passwordHash);
        //    var user2 = new User(userName, passwordHash);

        //    Assert.NotEqual(user1, user2);
        //}

        //#endregion

        //#region Tests for links

        //[Fact]
        //public void User_AssignTasks_ShouldUpdateTasksCollection()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;
        //    var user = new User(userName, passwordHash);

        //    string description = "Test description";
        //    TimeBlock timeBlock = TimeBlock.Day;
        //    Difficulty difficulty = Difficulty.Medium;
        //    DateTime toDoDate = DateTime.Now;
        //    string toDoCategoryName = "Other";
        //    Guid userId = Guid.NewGuid();
        //    var toDos = new List<ToDo>
        //    {
        //        new ToDo (description,timeBlock,difficulty,toDoDate,toDoCategoryName,userId),
        //        new ToDo (description,timeBlock,difficulty,toDoDate,toDoCategoryName,userId)
        //    };

        //    user.ToDos = toDos;

        //    Assert.NotNull(user.ToDos);
        //    Assert.Equal(2, user.ToDos.Count);
        //    Assert.Contains(toDos[0], user.ToDos);
        //    Assert.Contains(toDos[1], user.ToDos);
        //}

        //[Fact]
        //public void User_AssignCategories_ShouldUpdateCategoriesCollection()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";
        //    int experience = 100;
        //    var user = new User(userName, passwordHash);

        //    var toDoCategories = new List<ToDoCategory>
        //    {
        //        new ToDoCategory(Guid.NewGuid(),"Category 1"),
        //        new ToDoCategory(Guid.NewGuid(), "Category 2")
        //    };

        //    user.ToDoCategories = toDoCategories;

        //    Assert.NotNull(user.ToDoCategories);
        //    Assert.Equal(2, user.ToDoCategories.Count);
        //    Assert.Contains(toDoCategories[0], user.ToDoCategories);
        //    Assert.Contains(toDoCategories[1], user.ToDoCategories);
        //}

        #endregion

        #region Boundary tests

        //[Fact]
        //public void User_SetExperience_WithZero_ShouldNotThrowException()
        //{
        //    string userName = "Test user";
        //    string passwordHash = "Test password";

        //    var user = new User(userName, passwordHash);

        //    Assert.Equal(experience, user.Experience);
        //}

        #endregion
    }
}
