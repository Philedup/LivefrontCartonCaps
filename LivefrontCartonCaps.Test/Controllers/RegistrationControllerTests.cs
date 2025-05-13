using LivefrontCartonCaps.Controllers;
using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Models.Shared;
using LivefrontCartonCaps.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class RegistrationControllerTests
{
    private readonly Mock<IRegistrationService> _mockService = new();
    private readonly Mock<ILogger<RegistrationController>> _mockLogger = new();

    private RegistrationController CreateController()
    {
        return new RegistrationController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RegisterUser_ReturnsOk_WhenRegistrationSucceeds()
    {
        // Arrange
        var model = new RegistrationUserModel
        {
            FirstName = "Phil",
            LastName = "Phan",
            Email = "phil@example.com",
            ZipCode = "55410",
            BirthDate = new DateOnly(1980, 5, 9)
        };

        var resultModel = new RegistrationResultModel
        {
            Success = true,
            UserId = "abc123",
            ReferralCode = "PLP1013",
            Input = model
        };

        _mockService.Setup(s => s.RegisterUserAsync(model)).ReturnsAsync(resultModel);
        var controller = CreateController();

        // Act
        var result = await controller.RegisterUser(model);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<RegistrationResultModel>>(ok.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal("abc123", apiResponse.Data.UserId);
        Assert.Equal("PLP1013", apiResponse.Data.ReferralCode);
    }

    [Fact]
    public async Task RegisterUser_ReturnsConflict_WhenNameAlreadyExists()
    {
        // Arrange
        var model = new RegistrationUserModel
        {
            FirstName = "Phil",
            LastName = "Phan",
            Email = "phil@example.com",
            ZipCode = "55410",
            BirthDate = new DateOnly(1980, 5, 9)
        };

        var failedResult = new RegistrationResultModel
        {
            Success = false,
            Input = model
        };

        _mockService.Setup(s => s.RegisterUserAsync(model)).ReturnsAsync(failedResult);
        var controller = CreateController();

        // Act
        var result = await controller.RegisterUser(model);

        // Assert
        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<RegistrationResultModel>>(conflict.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal("A user with this name already exists.", apiResponse.Message);
    }

    [Fact]
    public async Task RegisterUser_Returns400_WhenModelStateIsInvalid()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Email", "The Email field is required.");

        var model = new RegistrationUserModel
        {
            FirstName = string.Empty, // Intentionally invalid
            LastName = string.Empty,  // Intentionally invalid
            Email = string.Empty,     // Intentionally invalid
            ZipCode = string.Empty    // Intentionally invalid
        };

        // Act
        var result = await controller.RegisterUser(model);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<RegistrationResultModel>>(badRequest.Value);
        Assert.False(response.Success);
        Assert.Contains("Validation failed", response.Message);
        Assert.Contains("The Email field is required.", response.Errors);
    }

    [Fact]
    public async Task RegisterUser_Returns500_WhenUnhandledExceptionOccurs()
    {
        // Arrange
        var model = new RegistrationUserModel
        {
            FirstName = "Phil",
            LastName = "Phan",
            Email = "phil@example.com",
            ZipCode = "55410",
            BirthDate = new DateOnly(1990, 5, 9)
        };

        _mockService.Setup(s => s.RegisterUserAsync(model)).ThrowsAsync(new Exception("Unexpected database error"));
        var controller = CreateController();

        // Act
        var result = await controller.RegisterUser(model);

        // Assert
        var errorResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, errorResult.StatusCode);
        var apiResponse = Assert.IsType<ApiResponse<RegistrationResultModel>>(errorResult.Value);
        Assert.False(apiResponse.Success);
        Assert.Contains("An unexpected error occurred", apiResponse.Message);
        Assert.Single(apiResponse.Errors);
    }
}
