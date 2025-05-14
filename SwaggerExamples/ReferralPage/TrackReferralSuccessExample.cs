using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.ReferralPage
{
    /// <summary>
    /// Example success response for tracking a referral click.
    /// </summary>
    public class TrackReferralSuccessExample : IExamplesProvider<ApiResponse<TrackReferralResultModel>>
    {
        /// <summary>
        /// Provides an example of a successful API response for tracking a referral click.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<TrackReferralResultModel> GetExamples()
        {
            return ApiResponse<TrackReferralResultModel>.Ok(new TrackReferralResultModel
            {
                ReferralCode = "PLP1013",
                Method = "email",
                DeviceId = "A1B2C3D4E5",
                IpAddress = "192.168.1.100",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
