using LivefrontCartonCaps.Models.Shared;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.SwaggerExamples
{
    /// <summary>
    /// Base class to provide reusable 500 Internal Server Error examples.
    /// Used to standardize server error examples across endpoints.
    /// </summary>
    /// <typeparam name="T">The data type wrapped by ApiResponse.</typeparam>
    public abstract class BaseApiResponse500Example<T> : IExamplesProvider<ApiResponse<T>> where T : class
    {
        /// <summary>
        /// Provides an example of a 500 Internal Server Error response.
        /// </summary>
        /// <returns></returns>
        public ApiResponse<T> GetExamples()
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "An unexpected error occurred",
                Data = null,
                Errors = new List<string>
                {
                    "System.NullReferenceException: Object reference not set to an instance of an object."
                }
            };
        }
    }
}