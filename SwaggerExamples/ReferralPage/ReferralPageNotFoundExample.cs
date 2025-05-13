using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.ReferralPage
{
    /// <summary>
    /// Swagger example for a 404 when referral data is not found.
    /// </summary>
    public class ReferralPageNotFoundExample : IExamplesProvider<ApiResponse<ReferralPageResultModel>>
    {
        /// <summary>
        /// Provides an example of a 404 response when referral data is not found.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<ReferralPageResultModel> GetExamples()
        {
            return new ApiResponse<ReferralPageResultModel>
            {
                Success = false,
                Message = "User not found",
                Data = null,
                Errors = new List<string>()
            };
        }
    }
}