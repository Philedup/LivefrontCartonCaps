using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.RegistrationPage
{
    /// <summary>
    /// Swagger example for a 400 Bad Request response due to validation errors.
    /// </summary>
    public class RegistrationValidationErrorExample : IExamplesProvider<ApiResponse<RegistrationResultModel>>
    {
        /// <summary>
        /// Provides an example of a 400 Bad Request response for registration validation errors.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<RegistrationResultModel> GetExamples()
        {
            return new ApiResponse<RegistrationResultModel>
            {
                Success = false,
                Message = "Validation failed",
                Data = null,
                Errors = new List<string>
                {
                    "The Email field is required.",
                    "Please enter a valid ZIP code (e.g., 12345 or 12345-6789)."
                }
            };
        }
    }
}
