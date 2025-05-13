using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.RegistrationPage
{
    /// <summary>
    /// Swagger example for a successful registration (200 OK).
    /// </summary>
    public class RegistrationSuccessExample : IExamplesProvider<ApiResponse<RegistrationResultModel>>
    {
        /// <summary>
        /// Provides an example of a successful registration response.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<RegistrationResultModel> GetExamples()
        {
            return new ApiResponse<RegistrationResultModel>
            {
                Success = true,
                Message = "Registration successful",
                Data = new RegistrationResultModel
                {
                    Success = true,
                    UserId = "user-abc123",
                    ReferralCode = "PLP1013",
                    WasReferred = true,
                    ReferredBy = "Steve Horn",
                    Input = new RegistrationUserModel
                    {
                        FirstName = "Phil",
                        LastName = "Phan",
                        Email = "phil@example.com",
                        PhoneNumber = "952-872-1211",
                        ReferralCode = "vpeng01",
                        BirthDate = DateOnly.Parse("1990-05-09"),
                        ZipCode = "55410"
                    }
                },
                Errors = new List<string>()
            };
        }
    }
}
