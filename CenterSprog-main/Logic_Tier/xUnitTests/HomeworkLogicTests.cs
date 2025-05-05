using Application.ClientInterfaces;
using Application.Logic;
using Domain.DTOs.HomeworkDTO;
using Moq;

namespace xUnit.Tests;

public class HomeworkLogicTests
{
    [Theory]
    [InlineData("", "Title123", "Valid description")]
    [InlineData("Lesson123", "", "Valid description")]
    [InlineData("Lesson123", "Title123", "")]
    [InlineData("", "", "")]
    [InlineData("Lesson123", "Title123", "Short")] // Description < then 10 char.
    
    public void ValidateHomeworkCreation_InvalidData_ExceptionsThrown(
        string lessonId, string title, string description)
    {
        var homeworkClientMock = new Mock<IHomeworkClient>();
        var homeworkLogic = new HomeworkLogic(homeworkClientMock.Object);

        var homeworkCreationDTO = new HomeworkCreationDTO
        {
            LessonId = lessonId,
            Title = title,
            Description = description
        };

        var exception = Record.Exception(() => homeworkLogic.ValidateHomeworkCreation(homeworkCreationDTO));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateHomeworkCreation_ValidData_NoExceptionsThrown()
    {
        var homeworkClient = new Mock<IHomeworkClient>();
        var homeworkLogic = new HomeworkLogic(homeworkClient.Object);

        var validDTO = new HomeworkCreationDTO
        {
            LessonId = "Lesson123",
            Title = "Valid Title",
            Description = "A valid description"
        };

        var exception = Record.Exception(() => homeworkLogic.ValidateHomeworkCreation(validDTO));

        Assert.Null(exception);
    }
}