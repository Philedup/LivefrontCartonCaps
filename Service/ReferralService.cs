using LivefrontCartonCaps.MockData;
using LivefrontCartonCaps.Models.Config;
using LivefrontCartonCaps.Models.ReferralsPage;
using Microsoft.Extensions.Options;

namespace LivefrontCartonCaps.Services
{
    /// <summary>
    /// Service for managing user referrals and generating referral links.
    /// </summary>
    public class ReferralService : IReferralService
    {
        private readonly IUserProfileRepository _userRepo;
        private readonly IUserReferralRepository _referralRepo;
        private readonly ReferralSettings _referralSettings;
        private readonly ILogger<ReferralService> _logger;

        /// <summary>
        /// Constructor for the ReferralService.
        /// </summary>
        /// <param name="userRepo">User profile repository.</param>
        /// <param name="referralRepo">Referral tracking repository.</param>
        /// <param name="referralSettings">Injected configuration settings.</param>
        /// <param name="logger">Injected logger for diagnostics.</param>
        public ReferralService(
            IUserProfileRepository userRepo,
            IUserReferralRepository referralRepo,
            IOptions<ReferralSettings> referralSettings,
            ILogger<ReferralService> logger)
        {
            _userRepo = userRepo;
            _referralRepo = referralRepo;
            _referralSettings = referralSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Gets the referral page model for the specified user.
        /// Normally this would query a database for the user's referral metadata.
        /// </summary>
        /// <param name="userId">The unique identifier of the user requesting referral data.</param>
        /// <returns>A populated referral result model for the user.</returns>
        /// <exception cref="Exception">Thrown if the user is not found.</exception>
        public async Task<ReferralPageResultModel> GetReferralPageModelAsync(string userId)
        {
            _logger.LogInformation("Fetching referral page model for user {UserId}", userId);

            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found when fetching referral page model: {UserId}", userId);
                throw new KeyNotFoundException("User not found");
            }

            // TODO: this is setup in the appsettings.json file, environment variable will need to be created and replaced in pipeline builds when deploying to qa/production. Also would want to have a dev base url to test with.
            // INFO: This link is a standard shareable link. Deferred deep linking behavior (e.g., redirecting users to app store or app screen after install)
            // is assumed to be handled by a third-party provider like Firebase or Branch.io.
            // If server-side tracking is needed later, we may store or track clicks via a POST /api/referrals/track endpoint.
            var linkBase = _referralSettings.ReferralLinkBase;
            if (string.IsNullOrWhiteSpace(linkBase))
            {
                _logger.LogError("ReferralLinkBase is missing in configuration.");
                throw new InvalidOperationException("ReferralLinkBase is not configured properly.");
            }

            if (string.IsNullOrWhiteSpace(user.ReferralCode))
            {
                _logger.LogError("User {UserId} is missing a referral code.", userId);
                throw new InvalidOperationException("ReferralCode is required for building the referral link.");
            }

            var referralLink = $"{linkBase}?referral_code={user.ReferralCode}";

            // TODO: this is a hardcoded message, but assumption is we would pull from a table the message we want sent to the user. Allows flexibility to change the message without needing to redeploy the app.
            // includes tracking parameters for email and sms text messages, all other types are assumed the link was copied and pasted and sent off
            var emailSubject = "You're invited to try the Carton Caps app!";

            _logger.LogInformation("Generated referral link for user {UserId}: {ReferralLink}", userId, referralLink);

            return new ReferralPageResultModel
            {
                ReferralCode = user.ReferralCode,
                ReferralLinkBase = linkBase,
                EmailSubject = emailSubject,
                EmailMessage = ComposeEmailMessage(referralLink),
                TextMessage = ComposeTextMessage(referralLink, user.ReferralCode),

                // fields I'm not sure we need. Would normally ask for more clarification here if the app needs this or not.
                // Could also be a security issue by passing it, so would lean towards not including it if the app already has the information.
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }

        /// <summary>
        /// Generates the email message body including referral link.
        /// </summary>
        /// <param name="referralLink">Base referral link.</param>
        /// <returns>Formatted email message string.</returns>
        private static string ComposeEmailMessage(string referralLink) =>
            $@"Hey!

            Join me in earning cash for our school by using the Carton Caps app. It’s an easy way to make a difference. 
            All you have to do is buy Carton Caps participating products (like Cheerios!) and scan your grocery receipt. 
            Carton Caps are worth $0.10 each and they add up fast! Twice a year, our school receives a check to help pay 
            for whatever we need – equipment, supplies or experiences the kids love!

            Download the Carton Caps app here: {referralLink}&method=email";

        /// <summary>
        /// Generates a short SMS message with referral link.
        /// </summary>
        /// <param name="referralLink">Base referral link.</param>
        /// <param name="code">Referral code.</param>
        /// <returns>Formatted SMS message string.</returns>
        private static string ComposeTextMessage(string referralLink, string code) =>
            $"Check out Carton Caps! Use my code {code} to sign up and support schools: {referralLink}&method=sms";

        /// <summary>
        /// List of referrals made by the user. This would be a list of users that have signed up using the referral code.
        /// Screenshot shows a My Referrals table with a name and status, so making the assumption that there is more than one status other than "Complete".
        /// This could also be included as part of the referral page model, but since it is a separate section in the app, I am keeping it separate in case we want to re-use this call somewhere else in the app.
        /// If no plans to reuse we'd just roll it into the GetReferralPageModel api call and return the data in the same model.
        /// </summary>
        /// <param name="userId">The user whose referral history to fetch.</param>
        /// <returns>A result model containing a list of referrals.</returns>
        public async Task<MyReferralsResultModel> GetMyReferralsAsync(string userId)
        {
            _logger.LogInformation("Fetching referral history for user {UserId}", userId);

            var referrals = await _referralRepo.GetReferralsByUserAsync(userId);

            var result = new MyReferralsResultModel
            {
                Referrals = new List<ReferralsModel>()
            };

            foreach (var r in referrals)
            {
                var referredUser = await _userRepo.GetUserByIdAsync(r.ReferredUserId);
                var fullName = referredUser != null
                    ? $"{referredUser.FirstName} {referredUser.LastName}"
                    : "Unknown";

                result.Referrals.Add(new ReferralsModel
                {
                    Name = fullName,
                    Status = r.Status
                });
            }

            _logger.LogInformation("Retrieved {Count} referral(s) for user {UserId}", result.Referrals.Count, userId);
            return result;
        }
    }
}
