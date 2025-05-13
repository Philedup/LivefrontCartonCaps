using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples.RegistrationPage
{
    /// <summary>
    /// Example for a registration conflict response.
    /// </summary>
    public class RegistrationConflictExample : IExamplesProvider<ApiResponse<RegistrationResultModel>>
    {
        /// <summary>
        /// Provides an example of a registration conflict response.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<RegistrationResultModel> GetExamples()
        {
            return new ApiResponse<RegistrationResultModel>
            {
                Success = false,
                Message = "A user with this name already exists.",
                Data = null,
                Errors = new List<string>()
            };
        }
    }
}
