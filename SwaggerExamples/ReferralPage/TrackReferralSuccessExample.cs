using LivefrontCartonCaps.Models.ReferralsPage;
using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Swagger example for tracking a referral click.
/// </summary>
public class TrackReferralSuccessExample : IExamplesProvider<ReferralClickTrackModel>
{
    /// <summary>
    /// Provides an example of a referral click tracking model.
    /// </summary>
    /// <returns></returns>
    public ReferralClickTrackModel GetExamples() => new ReferralClickTrackModel
    {
        ReferralCode = "PLP1013",
        Method = "email",
        DeviceId = "device-abc123",
        IpAddress = "192.168.1.10",
        Timestamp = DateTime.UtcNow
    };
}
