using Application.ClientInterfaces;
using Application.Logic;
using Domain.DTOs.FeedbackDTO;
using Moq;

namespace xUnit.Tests;

public class FeedbackLogicTests
{
    [Theory]
    [InlineData("", "handin123", 7, "Valid comment")] // Empty username
    [InlineData("student123", "", 4, "Valid comment")] // Empty handin
    [InlineData("student123", "handin123", 15, "Valid comment")] // Invalid grade
    [InlineData("student123", "handin123", 4, "")] // Empty comment
    [InlineData("student123", "handin123", 4, "abc")] // Comment less than 5 char.
    
    public void ValidateFeedbackCreation_InvalidData_ExceptionsThrown(
        string studentUsername, string handInId, int grade, string comment)
    {
        // Arrange 
        var feedbackClientMock = new Mock<IFeedbackClient>();
        var feedbackLogic = new FeedbackLogic(feedbackClientMock.Object); 
        
        var addFeedbackDto = new AddFeedbackDTO
        {
            StudentUsername = studentUsername,
            HandInId = handInId,
            Grade = grade,
            Comment = comment
        };

        // Act 
        var exception = Record.Exception(() => feedbackLogic.ValidateFeedbackCreation(addFeedbackDto));

        // Assert 
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }
    
    [Fact]
    public void ValidateFeedbackCreation_ValidData_NoExceptionsThrown()
    {
        // Arrange
        var feedbackClientMock = new Mock<IFeedbackClient>();
        var feedbackLogic = new FeedbackLogic(feedbackClientMock.Object); 
        
        var validDto = new AddFeedbackDTO
        {
            StudentUsername = "student123",
            HandInId = "handin123",
            Grade = 4,
            Comment = "A valid comment"
        };

        // Act
        var exception = Record.Exception(() => feedbackLogic.ValidateFeedbackCreation(validDto));

        // Assert 
        Assert.Null(exception);
    }
}