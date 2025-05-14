using LivefrontCartonCaps.Models.ReferralsPage;
using LivefrontCartonCaps.Models.Shared;
using LivefrontCartonCaps.Services;
using LivefrontCartonCaps.SwaggerExamples.ReferralPage;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace LivefrontCartonCaps.Controllers
{
    /// <summary>
    /// API Controller for managing user referrals and generating referral links.
    /// </summary>
    [ApiController]
    [Route("api/referrals")]
    [Produces("application/json")]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralService;
        private readonly ILogger<ReferralsController> _logger;
        private const string AuthenticatedUserId = "user1"; // TODO: Replace with actual authenticated user ID in production

        /// <summary>
        /// Constructor for the ReferralsController.
        /// </summary>
        /// <param name="referralService">Injected referral service.</param>
        /// <param name="logger">Injected logger service.</param>
        public ReferralsController(IReferralService referralService, ILogger<ReferralsController> logger)
        {
            _referralService = referralService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the referral page data (referral code, message templates, base link).
        /// </summary>
        /// <returns>Referral link info for the current user.</returns>
        /// <response code="200">Referral page data returned successfully</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Unexpected server error</response>
        [HttpGet("page")]
        [ProducesResponseType(typeof(ApiResponse<ReferralPageResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ReferralPageResultModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<ReferralPageResultModel>), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReferralPageSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ReferralPageNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ReferralPage500Example))]
        public async Task<ActionResult<ApiResponse<ReferralPageResultModel>>> GetReferralPage()
        {
            try
            {
                var result = await _referralService.GetReferralPageModelAsync(AuthenticatedUserId);
                return Ok(ApiResponse<ReferralPageResultModel>.Ok(result));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found in GetReferralPage for user {UserId}", AuthenticatedUserId);
                return NotFound(ApiResponse<ReferralPageResultModel>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetReferralPage for user {UserId}", AuthenticatedUserId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ReferralPageResultModel>.Fail("An unexpected error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Returns the list of referrals made by the current user.
        /// </summary>
        /// <returns>A list of users referred by the current user.</returns>
        /// <response code="200">Referral history returned successfully</response>
        /// <response code="500">Unexpected server error</response>
        [HttpGet("history")]
        [ProducesResponseType(typeof(ApiResponse<MyReferralsResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MyReferralsResultModel>), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MyReferralsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(MyReferrals500Example))]
        public async Task<ActionResult<ApiResponse<MyReferralsResultModel>>> GetMyReferrals()
        {

            try
            {
                var result = await _referralService.GetMyReferralsAsync(AuthenticatedUserId);
                return Ok(ApiResponse<MyReferralsResultModel>.Ok(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetMyReferrals for user {UserId}", AuthenticatedUserId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<MyReferralsResultModel>.Fail("An unexpected error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Tracks a referral link click for future analytics or fraud mitigation.
        /// Currently a placeholder endpoint for potential in-house tracking.
        /// </summary>
        /// <param name="model">Click tracking details.</param>
        /// <returns>200 OK if tracking was accepted, 400 Bad Request if input is invalid.</returns>
        [HttpPost("track")]
        [ProducesResponseType(typeof(ApiResponse<TrackReferralResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TrackReferralSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(TrackReferralBadRequestExample))]
        public IActionResult TrackReferralClick([FromBody] TrackReferralResultModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ReferralCode))
            {
                return BadRequest(ApiResponse<object>.Fail("ReferralCode is required"));
            }

            // Log or store for future use
            _logger.LogInformation("Tracking referral click: Code={ReferralCode}, Method={Method}, Device={DeviceId}, IP={Ip}, Time={Time}",
                model.ReferralCode, model.Method, model.DeviceId, model.IpAddress, model.Timestamp);

            // Placeholder for future logic
            return Ok(ApiResponse<TrackReferralResultModel>.Ok(model));
        }

    }
}
