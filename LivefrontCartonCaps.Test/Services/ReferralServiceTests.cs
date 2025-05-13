using LivefrontCartonCaps.Entities;
using LivefrontCartonCaps.MockData;
using LivefrontCartonCaps.Models.Config;
using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

public class ReferralServiceTests
{
    private ReferralService CreateService(
        Mock<IUserProfileRepository> userRepoMock,
        Mock<IUserReferralRepository> referralRepoMock,
        IOptions<ReferralSettings> settings)
    {
        var loggerMock = new Mock<ILogger<ReferralService>>();
        return new ReferralService(userRepoMock.Object, referralRepoMock.Object, settings, loggerMock.Object);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ReturnsValidModel_WhenUserExists()
    {
        // Arrange
        var userId = "user1";
        var referralCode = "PLP1013";
        var phone = "123-456-7890";
        var email = "phil@example.com";
        var baseLink = "https://cartoncaps.link/abfilefa90p";
        var expectedFullLink = $"{baseLink}?referral_code={referralCode}";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new UserProfile
        {
            UserId = userId,
            FirstName = "Phil",
            ReferralCode = referralCode,
            PhoneNumber = phone,
            Email = email
        });

        var mockSettings = Options.Create(new ReferralSettings
        {
            ReferralLinkBase = baseLink
        });

        var service = CreateService(mockUserRepo, mockReferralRepo, mockSettings);

        // Act
        var result = await service.GetReferralPageModelAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(referralCode, result.ReferralCode);
        Assert.Equal(baseLink, result.ReferralLinkBase);
        Assert.Equal(expectedFullLink, result.FullReferralLink);

        Assert.NotNull(result.EmailSubject);
        Assert.Contains("Carton Caps", result.EmailSubject, StringComparison.OrdinalIgnoreCase);

        Assert.NotNull(result.EmailMessage);
        Assert.Contains(expectedFullLink, result.EmailMessage);
        Assert.Contains("&method=email", result.EmailMessage);

        Assert.NotNull(result.TextMessage);
        Assert.Contains(expectedFullLink, result.TextMessage);
        Assert.Contains("&method=sms", result.TextMessage);

        Assert.Equal(phone, result.PhoneNumber);
        Assert.Equal(email, result.Email);

        Assert.False(string.IsNullOrWhiteSpace(result.EmailSubject), "EmailSubject should not be null or whitespace.");
        Assert.False(string.IsNullOrWhiteSpace(result.EmailMessage), "EmailMessage should not be null or whitespace.");
        Assert.False(string.IsNullOrWhiteSpace(result.TextMessage), "TextMessage should not be null or whitespace.");

        Assert.StartsWith("https://", result.FullReferralLink);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ThrowsKeyNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userId = "nonexistent-user";
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((UserProfile?)null);

        var mockSettings = Options.Create(new ReferralSettings
        {
            ReferralLinkBase = "https://cartoncaps.link/abfilefa90p"
        });

        var service = CreateService(mockUserRepo, mockReferralRepo, mockSettings);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetReferralPageModelAsync(userId));

        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ThrowsInvalidOperationException_WhenReferralLinkBaseIsMissing()
    {
        // Arrange
        var userId = "user1";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new UserProfile
        {
            UserId = userId,
            FirstName = "Phil",
            ReferralCode = "PLP1013",
            PhoneNumber = "123-456-7890",
            Email = "phil@example.com"
        });

        var emptySettings = Options.Create(new ReferralSettings { ReferralLinkBase = "" });

        var service = CreateService(mockUserRepo, mockReferralRepo, emptySettings);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetReferralPageModelAsync(userId));

        Assert.Equal("ReferralLinkBase is not configured properly.", ex.Message);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ThrowsInvalidOperationException_WhenReferralCodeIsMissing()
    {
        // Arrange
        var userId = "user1";
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new UserProfile
        {
            UserId = userId,
            FirstName = "Phil",
            ReferralCode = "", // Invalid referral code
            PhoneNumber = "123-456-7890",
            Email = "phil@example.com"
        });

        var settings = Options.Create(new ReferralSettings
        {
            ReferralLinkBase = "https://cartoncaps.link/abc"
        });

        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetReferralPageModelAsync(userId));

        Assert.Equal("ReferralCode is required for building the referral link.", ex.Message);
    }

    [Fact]
    public async Task GetMyReferralsAsync_ReturnsReferralList_WithResolvedNames()
    {
        // Arrange
        var userId = "user1";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        var referrals = new List<UserReferral>
        {
            new() { ReferrerUserId = userId, ReferredUserId = "user2", Status = "Pending" },
            new() { ReferrerUserId = userId, ReferredUserId = "user3", Status = "Accepted" }
        };

        var referredUsers = new Dictionary<string, UserProfile>
        {
            ["user2"] = new UserProfile { UserId = "user2", FirstName = "Jane", LastName = "Smith" },
            ["user3"] = new UserProfile { UserId = "user3", FirstName = "Bob", LastName = "Jones" }
        };

        mockReferralRepo.Setup(r => r.GetReferralsByUserAsync(userId)).ReturnsAsync(referrals);
        mockUserRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => referredUsers.GetValueOrDefault(id));

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://dummy" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act
        var result = await service.GetMyReferralsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Referrals.Count);
        Assert.Contains(result.Referrals, r => r.Name == "Jane Smith" && r.Status == "Pending");
        Assert.Contains(result.Referrals, r => r.Name == "Bob Jones" && r.Status == "Accepted");
    }

    [Fact]
    public async Task GetMyReferralsAsync_ReturnsUnknownName_WhenReferredUserNotFound()
    {
        // Arrange
        var userId = "user1";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        var referrals = new List<UserReferral>
        {
            new() { ReferrerUserId = userId, ReferredUserId = "missing-user", Status = "Pending" }
        };

        mockReferralRepo.Setup(r => r.GetReferralsByUserAsync(userId)).ReturnsAsync(referrals);
        mockUserRepo.Setup(r => r.GetUserByIdAsync("missing-user")).ReturnsAsync((UserProfile?)null);

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://dummy" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act
        var result = await service.GetMyReferralsAsync(userId);

        // Assert
        Assert.Single(result.Referrals);
        Assert.Equal("Unknown", result.Referrals[0].Name);
        Assert.Equal("Pending", result.Referrals[0].Status);
    }

    [Theory]
    [InlineData("Pending", "Alice", "Wonderland")]
    [InlineData("Accepted", "Charlie", "Bucket")]
    [InlineData("Rejected", "Daisy", "Hill")]
    public async Task GetMyReferralsAsync_ReturnsExpectedStatus_ForEachReferral(
    string status, string firstName, string lastName)
    {
        // Arrange
        var userId = "userX";
        var referredUserId = "referredX";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockReferralRepo.Setup(r => r.GetReferralsByUserAsync(userId)).ReturnsAsync(new List<UserReferral>
    {
        new() { ReferrerUserId = userId, ReferredUserId = referredUserId, Status = status }
    });

        mockUserRepo.Setup(r => r.GetUserByIdAsync(referredUserId)).ReturnsAsync(new UserProfile
        {
            UserId = referredUserId,
            FirstName = firstName,
            LastName = lastName
        });

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://dummy" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act
        var result = await service.GetMyReferralsAsync(userId);

        // Assert
        Assert.Single(result.Referrals);
        var referral = result.Referrals[0];
        Assert.Equal($"{firstName} {lastName}", referral.Name);
        Assert.Equal(status, referral.Status);
    }

    [Fact]
    public async Task GetMyReferralsAsync_ReturnsUnknown_WhenReferredUserProfileIsMissing()
    {
        // Arrange
        var userId = "user1";
        var missingUserId = "ghost123";

        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockReferralRepo.Setup(r => r.GetReferralsByUserAsync(userId)).ReturnsAsync(new List<UserReferral>
    {
        new() { ReferrerUserId = userId, ReferredUserId = missingUserId, Status = "Pending" }
    });

        mockUserRepo.Setup(r => r.GetUserByIdAsync(missingUserId)).ReturnsAsync((UserProfile?)null);

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://dummy" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act
        var result = await service.GetMyReferralsAsync(userId);

        // Assert
        Assert.Single(result.Referrals);
        Assert.Equal("Unknown", result.Referrals[0].Name);
        Assert.Equal("Pending", result.Referrals[0].Status);
    }

    [Fact]
    public async Task GetMyReferralsAsync_ReturnsEmptyList_WhenUserHasNoReferrals()
    {
        // Arrange
        var userId = "user1";
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockReferralRepo.Setup(r => r.GetReferralsByUserAsync(userId)).ReturnsAsync(new List<UserReferral>());

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://dummy" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act
        var result = await service.GetMyReferralsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Referrals);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ThrowsInvalidOperationException_WhenReferralCodeIsNull()
    {
        // Arrange
        var userId = "user-null-code";
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(new UserProfile
        {
            UserId = userId,
            FirstName = "Test",
            ReferralCode = null,
            PhoneNumber = "123-456-7890",
            Email = "test@example.com"
        });

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "https://carton" });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetReferralPageModelAsync(userId));

        Assert.Equal("ReferralCode is required for building the referral link.", ex.Message);
    }

    [Fact]
    public async Task GetReferralPageModelAsync_ThrowsInvalidOperationException_WhenReferralLinkBaseIsWhitespace()
    {
        // Arrange
        var userId = "user-whitespace-link";
        var mockUserRepo = new Mock<IUserProfileRepository>();
        var mockReferralRepo = new Mock<IUserReferralRepository>();

        mockUserRepo.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(new UserProfile
        {
            UserId = userId,
            FirstName = "Test",
            ReferralCode = "CODE123",
            PhoneNumber = "123-456-7890",
            Email = "test@example.com"
        });

        var settings = Options.Create(new ReferralSettings { ReferralLinkBase = "   " });
        var service = CreateService(mockUserRepo, mockReferralRepo, settings);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetReferralPageModelAsync(userId));

        Assert.Equal("ReferralLinkBase is not configured properly.", ex.Message);
    }
}
