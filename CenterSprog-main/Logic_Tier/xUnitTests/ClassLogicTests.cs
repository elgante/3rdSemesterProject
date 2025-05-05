using Application.ClientInterfaces;
using Application.Logic;
using Domain.DTOs.ClassDTO;
using Domain.DTOs.HomeworkDTO;
using Moq;
using xUnit;
namespace xUnit.Tests;

public class ClassLogicTests
{
    [Fact]
    public void ValidateClassCreation_ValidData_NoExceptionsThrown()
    {
        // ARRANGE creating instances, initializing
        var classClientMock = new Mock<IClassClient>();
        var classLogic = new ClassLogic(classClientMock.Object);
    
        var validDto = new ClassCreationDTO()
        {
            Title = "Sample title",
            Room = "C03.07"
        };
        
        // ACT invoking method to validate
        var exception = Record.Exception(() => classLogic.ValidateClassCreation(validDto));
    
        
        // ASSERT verify that the returned result is the expected one
        Assert.Null(exception); // no exceptions should be thrown
        
    }

    [Theory]
    [InlineData("", "Room123")] // Empty Title
    [InlineData("Math Class", "")] // Empty Room
    [InlineData("", "")] // Empty Title and Room
    [InlineData(null, null)] // Title and Room are null
    
    public void ValidateClassCreation_InvalidData_ExceptionsThrown(string title, string room)
    {
        // ARRANGE
        var classClientMock = new Mock<IClassClient>();
        var classLogic = new ClassLogic(classClientMock.Object);
    
        var invalidDto = new ClassCreationDTO()
        {
            Title = title,
            Room = room
        };
        
        // ACT
        var exception = Record.Exception(() => classLogic.ValidateClassCreation(invalidDto));
        
        // ASSERT
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception); // type Exception is thrown
        
    }
}