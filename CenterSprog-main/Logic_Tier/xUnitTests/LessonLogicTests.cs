using Application.ClientInterfaces;
using Application.Logic;
using Moq;

namespace xUnit.Tests;

public class LessonLogicTests
{
    [Theory]
    [InlineData("", "Description123", 1638825600, "Id12345")] // Empty Topic
    [InlineData("Topic123", "", 1638825600, "Id12345")] // Empty Description
    [InlineData("St", "Description123", 1638825600, "Id12345")] // Short Topic
    [InlineData("Topic123", "Short", 1638825600, "Id12345")] // Short Description
    [InlineData("Topic123", "Description123", 1638825600, "")] // Empty Id
    [InlineData("Topic123", "Description123", 0, "Id12345")] // Null Date
    
    public void ValidateLessonCreationAndUpdate_InvalidData_ExceptionsThrown(
        string topic, string description, long date, string id)
    {
        var lessonClientMock = new Mock<ILessonClient>();
        var lessonLogic = new LessonLogic(lessonClientMock.Object);

        var exception = Record.Exception(() => lessonLogic.ValidateLessonCreationAndUpdate(topic, description, date, id));

        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void ValidateLessonCreationAndUpdate_ValidData_NoExceptionsThrown()
    {
        var lessonClientMock = new Mock<ILessonClient>();
        var lessonLogic = new LessonLogic(lessonClientMock.Object);

        var validId = "Id123456";
        var validTopic = "Valid Topic";
        var validDescription = "Valid description";
        var validDate = 1638825600;

        var exception = Record.Exception(() => lessonLogic.ValidateLessonCreationAndUpdate(validTopic, validDescription, validDate, validId));

        Assert.Null(exception);
    }
}