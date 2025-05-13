using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.ReferralPage
{
    /// <summary>
    /// Swagger example for 400 Bad Request when tracking a referral click with missing data.
    /// </summary>
    public class TrackReferralBadRequestExample : IExamplesProvider<ApiResponse<object>>
    {
        /// <summary>
        /// Provides an example of a 400 Bad Request response when the referral code is missing.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<object> GetExamples()
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "ReferralCode is required",
                Data = null,
                Errors = new List<string> { "ReferralCode is required" }
            };
        }
    }
}
