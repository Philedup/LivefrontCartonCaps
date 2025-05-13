using LivefrontCartonCaps.Models.RegistrationPage;
using LivefrontCartonCaps.Models.Shared;
using LivefrontCartonCaps.Services;
using LivefrontCartonCaps.SwaggerExamples.RegistrationPage;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.Controllers
{
    /// <summary>
    /// API Controller for managing user registrations, including referral logic.
    /// </summary>
    [ApiController]
    [Route("api/registration")]
    [Produces("application/json")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationController> _logger;

        /// <summary>
        /// Constructor for the RegistrationController.
        /// </summary>
        /// <param name="registrationService">Injected registration service.</param>
        /// <param name="logger">Injected logger service.</param>
        public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user with the provided information, including optional referral code.
        /// </summary>
        /// <param name="model">The registration data submitted by the client.</param>
        /// <returns>
        /// A response indicating whether registration was successful, including referral info if applicable.
        /// </returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Validation errors in the submitted registration data</response>
        /// <response code="409">A user with the same first and last name already exists</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RegistrationResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RegistrationResultModel>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<RegistrationResultModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<RegistrationResultModel>), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(Registration500Example))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegistrationSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(RegistrationConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RegistrationValidationErrorExample))]
        public async Task<ActionResult<ApiResponse<RegistrationResultModel>>> RegisterUser([FromBody] RegistrationUserModel model)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<RegistrationResultModel>.Fail("Validation failed", validationErrors));
            }

            try
            {
                var result = await _registrationService.RegisterUserAsync(model);

                if (!result.Success)
                {
                    return Conflict(ApiResponse<RegistrationResultModel>.Fail("A user with this name already exists."));
                }

                return Ok(ApiResponse<RegistrationResultModel>.Ok(result, "Registration successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user registration. Input: {@Input}", model);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<RegistrationResultModel>.Fail("An unexpected error occurred", new List<string> { ex.Message }));
            }
        }
    }
}
