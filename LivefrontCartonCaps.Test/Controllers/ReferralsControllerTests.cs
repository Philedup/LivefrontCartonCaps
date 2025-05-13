using LivefrontCartonCaps.Controllers;
using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using LivefrontCartonCaps.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class ReferralsControllerTests
{
    private readonly Mock<IReferralService> _mockReferralService = new();
    private readonly Mock<ILogger<ReferralsController>> _mockLogger = new();

    private ReferralsController CreateController() =>
        new ReferralsController(_mockReferralService.Object, _mockLogger.Object);

    [Fact]
    public async Task GetReferralPage_ReturnsOk_WhenUserExists()
    {
        var model = new ReferralPageResultModel { ReferralCode = "ABC123", ReferralLinkBase = "https://test.link" };
        _mockReferralService.Setup(s => s.GetReferralPageModelAsync("user1")).ReturnsAsync(model);
        var controller = CreateController();

        var result = await controller.GetReferralPage();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<ReferralPageResultModel>>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal("ABC123", apiResponse.Data.ReferralCode);
    }

    [Fact]
    public async Task GetReferralPage_ReturnsNotFound_WhenUserDoesNotExist()
    {
        _mockReferralService.Setup(s => s.GetReferralPageModelAsync("user1"))
            .ThrowsAsync(new KeyNotFoundException("User not found"));
        var controller = CreateController();

        var result = await controller.GetReferralPage();

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<ReferralPageResultModel>>(notFound.Value);
        Assert.False(apiResponse.Success);
    }

    [Fact]
    public async Task GetReferralPage_Returns500_WhenUnexpectedExceptionOccurs()
    {
        _mockReferralService.Setup(s => s.GetReferralPageModelAsync("user1"))
            .ThrowsAsync(new Exception("Database failure"));
        var controller = CreateController();

        var result = await controller.GetReferralPage();

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
        var response = Assert.IsType<ApiResponse<ReferralPageResultModel>>(objectResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task GetMyReferrals_ReturnsOk_WithReferralList()
    {
        var mockReferrals = new MyReferralsResultModel
        {
            Referrals = new List<ReferralsModel> { new() { Name = "Phil Phan", Status = "Accepted" } }
        };
        _mockReferralService.Setup(s => s.GetMyReferralsAsync("user1")).ReturnsAsync(mockReferrals);
        var controller = CreateController();

        var result = await controller.GetMyReferrals();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse<MyReferralsResultModel>>(okResult.Value);
        Assert.True(apiResponse.Success);
        Assert.Single(apiResponse.Data.Referrals);
    }

    [Fact]
    public async Task GetMyReferrals_Returns500_OnException()
    {
        _mockReferralService.Setup(s => s.GetMyReferralsAsync("user1"))
            .ThrowsAsync(new Exception("Internal error"));
        var controller = CreateController();

        var result = await controller.GetMyReferrals();

        var response = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, response.StatusCode);
        var apiResponse = Assert.IsType<ApiResponse<MyReferralsResultModel>>(response.Value);
        Assert.False(apiResponse.Success);
    }

    [Fact]
    public void TrackReferralClick_ReturnsAccepted_WhenValidModel()
    {
        var controller = CreateController();
        var model = new ReferralClickTrackModel
        {
            ReferralCode = "ABC123",
            Method = "sms",
            DeviceId = "dev01",
            IpAddress = "127.0.0.1"
        };

        var result = controller.TrackReferralClick(model);

        var accepted = Assert.IsType<AcceptedResult>(result);
        Assert.Equal(202, accepted.StatusCode);
    }

    [Fact]
    public void TrackReferralClick_ReturnsBadRequest_WhenReferralCodeMissing()
    {
        var controller = CreateController();
        var model = new ReferralClickTrackModel { ReferralCode = "" };

        var result = controller.TrackReferralClick(model);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse<object>>(badRequest.Value);
        Assert.False(apiResponse.Success);
        Assert.Equal("ReferralCode is required", apiResponse.Message);
    }
}