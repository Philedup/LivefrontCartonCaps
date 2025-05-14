using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.ReferralPage
{
    /// <summary>
    /// Swagger example for a successful referral history response.
    /// </summary>
    public class MyReferralsSuccessExample : IExamplesProvider<ApiResponse<MyReferralsResultModel>>
    {
        /// <summary>
        /// Provides an example of a successful API response for the referral history endpoint.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<MyReferralsResultModel> GetExamples()
        {
            return new ApiResponse<MyReferralsResultModel>
            {
                Success = true,
                Message = "Referral history loaded successfully",
                Data = new MyReferralsResultModel
                {
                    Referrals = new List<ReferralsModel>
                    {
                        new ReferralsModel { Name = "Sam Kirchmeier", Status = "Pending" },
                        new ReferralsModel { Name = "Mike Bollinger", Status = "Complete" }
                    }
                },
                Errors = new List<string>()
            };
        }
    }
}