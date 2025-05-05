using Application.ClientInterfaces;
using Application.Logic;
using Domain.DTOs.HomeworkDTO;
using Moq;

namespace xUnit.Tests;

public class HandInHomeworkLogicTests
{
    [Theory]
    [InlineData("", "Homework123", "Valid Answer")]
    [InlineData("Student123", "", "Valid Answer")]
    [InlineData("Student123", "Homework123", "")]
    [InlineData("", "", "")]
    
    public void ValidateHandInCreation_InvalidData_ExceptionsThrown(
        string studentUsername, string homeworkId, string answer)
    {
        var handInClientMock = new Mock<IHandInHomeworkClient>();
        var handInLogic = new HandInHomeworkLogic(handInClientMock.Object);

        var handInDTO = new HomeworkHandInDTO
        {
            StudentUsername = studentUsername,
            HomeworkId = homeworkId,
            Answer = answer
        };

        var exception = Record.Exception(() => handInLogic.ValidateHandInCreation(handInDTO));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateHandInCreation_ValidData_NoExceptionsThrown()
    {
        var handInClientMock = new Mock<IHandInHomeworkClient>();
        var handInLogic = new HandInHomeworkLogic(handInClientMock.Object);

        var validDTO = new HomeworkHandInDTO
        {
            StudentUsername = "Student123",
            HomeworkId = "Homework123",
            Answer = "A valid answer"
        };

        var exception = Record.Exception(() => handInLogic.ValidateHandInCreation(validDTO));

        Assert.Null(exception);
    }
}