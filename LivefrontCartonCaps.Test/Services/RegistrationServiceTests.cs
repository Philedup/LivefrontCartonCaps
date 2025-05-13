using LivefrontCartonCaps.Entities;
using LivefrontCartonCaps.MockData;
using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Services;
using Moq;

public class RegistrationServiceTests
{
    [Fact]
    public async Task RegisterUserAsync_ReturnsSuccess_WhenValidUser()
    {
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.IsDuplicateNameAsync("John", "Doe")).ReturnsAsync(false);

        var service = new RegistrationService(mockUserRepo.Object, mockReferralRepo.Object);
        var model = new RegistrationUserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ReferralCode = null,
            BirthDate = DateOnly.Parse("1990-01-01"),
            ZipCode = "12345"
        };

        var result = await service.RegisterUserAsync(model);

        Assert.True(result.Success);
        Assert.Equal(model.FirstName, result.Input.FirstName);
        Assert.NotNull(result.UserId);
        Assert.NotNull(result.ReferralCode);
        Assert.False(result.WasReferred);
        Assert.Null(result.ReferredBy);
    }

    [Fact]
    public async Task RegisterUserAsync_ReturnsConflict_WhenDuplicateUserExists()
    {
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.IsDuplicateNameAsync("John", "Doe")).ReturnsAsync(true);

        var service = new RegistrationService(mockUserRepo.Object, mockReferralRepo.Object);
        var model = new RegistrationUserModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ReferralCode = null,
            BirthDate = DateOnly.Parse("1990-01-01"),
            ZipCode = "12345"
        };

        var result = await service.RegisterUserAsync(model);

        Assert.False(result.Success);
        Assert.Equal(model.FirstName, result.Input.FirstName);
    }

    [Fact]
    public async Task RegisterUserAsync_LinksReferral_WhenReferralCodeValid()
    {
        var referrer = new UserProfile { UserId = "ref123", FirstName = "Alice", LastName = "Smith", ReferralCode = "ALICE01" };
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.IsDuplicateNameAsync("Bob", "Jones")).ReturnsAsync(false);
        mockUserRepo.Setup(repo => repo.GetUserByReferralCodeAsync("ALICE01")).ReturnsAsync(referrer);

        var service = new RegistrationService(mockUserRepo.Object, mockReferralRepo.Object);
        var model = new RegistrationUserModel
        {
            FirstName = "Bob",
            LastName = "Jones",
            Email = "bob.jones@example.com",
            ReferralCode = "ALICE01",
            BirthDate = DateOnly.Parse("1990-01-01"),
            ZipCode = "12345"
        };

        var result = await service.RegisterUserAsync(model);

        Assert.True(result.Success);
        Assert.True(result.WasReferred);
        Assert.Equal("Alice Smith", result.ReferredBy);
        mockReferralRepo.Verify(r => r.AddReferralAsync(It.Is<UserReferral>(
            ur => ur.ReferrerUserId == "ref123" && ur.Status == "Pending")), Times.Once);
    }
}
