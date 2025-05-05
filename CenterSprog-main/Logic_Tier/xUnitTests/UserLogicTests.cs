using System.ComponentModel.DataAnnotations;
using Application.ClientInterfaces;
using Application.Logic;
using Domain.DTOs.UserDTO;
using Moq;

namespace xUnit.Tests;

public class UserLogicTests
{

    [Fact]
    public void GenerateUserCreationCredentials_ConfirmLength_NoException()
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);

        string name = "namename";
        var results = userLogic.GenerateUserCreationCredentials(name,8);
        
        Assert.True(results.Item2.Length == 8);
        Assert.True(results.Item1.Length == name.Length+2 );
        
    }

    [Theory]
    [InlineData("", "password123")] // Empty Username
    [InlineData("user123", "")] // Empty Password
    [InlineData("", "")] // Empty Username and Password
    public void ValidateCredentials_InvalidData_ExceptionsThrown(string username, string password)
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);

        var exception = Record.Exception(() => userLogic.ValidateCredentials(username, password));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateCredentials_ValidData_NoExceptionsThrown()
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);

        var validUsername = "user123";
        var validPassword = "password123";

        var exception = Record.Exception(() => userLogic.ValidateCredentials(validUsername, validPassword));

        Assert.Null(exception);
    }
    
    [Theory]
    [InlineData("", "LastName", "Admin", "invalidEmail")] // Empty First Name
    [InlineData("FirstName", "", "Teacher", "email@example")] // Empty Last Name
    [InlineData("FirstName", "LastName", "", "email@.com")] // Empty Role
    [InlineData("FirstName", "LastName", "Admin", "invalidEmail")] // Invalid Email
    [InlineData("FirstName", "LastName", "Admin", "")] // Empty Email
    
    public void ValidateUserCreation_InvalidData_ExceptionsThrown(
        string firstName, string lastName, string role, string email)
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);
        
        var dto = new UserCreationDTO
        {
            FirstName = firstName,
            LastName = lastName,
            Role = role,
            Email = email
        };

        var exception = Record.Exception(() => userLogic.ValidateUserCreation(dto));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateUserCreation_ValidData_NoExceptionsThrown()
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);

        var validDTO = new UserCreationDTO
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Role = "Admin",
            Email = "email@example.com"
        };

        var exception = Record.Exception(() => userLogic.ValidateUserCreation(validDTO));

        Assert.Null(exception);
    }
    [Theory]
    [InlineData("invalidEmail")] // Invalid email format
    [InlineData("email@example")] // Invalid domain
    [InlineData("email@.com")] // Missing domain
    public void ValidateEmail_InvalidData_ExceptionsThrown(string email)
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);
        
        var exception = Record.Exception(() => userLogic.ValidateEmail(email));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateEmail_ValidData_NoExceptionsThrown()
    {
        var userClientMock = new Mock<IUserClient>();
        var userLogic = new UserLogic(userClientMock.Object);
        var validEmail = "name@example.com";

        var exception = Record.Exception(() => userLogic.ValidateEmail(validEmail));

        Assert.Null(exception);
    }
}