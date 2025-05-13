using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.ReferralPage
{
    /// <summary>
    /// Swagger example for a successful referral page load.
    /// </summary>
    public class ReferralPageSuccessExample : IExamplesProvider<ApiResponse<ReferralPageResultModel>>
    {
        /// <summary>
        /// Provides an example of a successful referral page load (200 OK).
        /// </summary>
        /// <returns></returns>
        public ApiResponse<ReferralPageResultModel> GetExamples()
        {
            return new ApiResponse<ReferralPageResultModel>
            {
                Success = true,
                Message = "Referral page loaded successfully",
                Data = new ReferralPageResultModel
                {
                    ReferralCode = "PLP1013",
                    ReferralLinkBase = "https://cartoncaps.link/abfilefa90p",
                    Email = "phil@example.com",
                    PhoneNumber = "952-872-1211",
                    EmailSubject = "You're invited to try the Carton Caps app!",
                    EmailMessage = "Join me in earning cash for our school by using the Carton Caps app...",
                    TextMessage = "Check out Carton Caps! Use my code PLP1013 to sign up and support schools: https://cartoncaps.link/abfilefa90p?referral_code=PLP1013&method=sms"
                },
                Errors = new List<string>()
            };
        }
    }
}