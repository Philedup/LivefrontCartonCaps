using LivefrontCartonCaps.Entities;
using LivefrontCartonCaps.MockData;
using LivefrontCartonCaps.Models.RegistrationPage;

namespace LivefrontCartonCaps.Services
{
    /// <summary>
    /// Service for managing user registrations, including referral logic.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserProfileRepository _userRepo;
        private readonly IUserReferralRepository _referralRepo;

        /// <summary>
        /// Constructor for the RegistrationService.
        /// </summary>
        /// <param name="userRepo">The user profile repository.</param>
        /// <param name="referralRepo">The referral repository.</param>
        public RegistrationService(IUserProfileRepository userRepo, IUserReferralRepository referralRepo)
        {
            _userRepo = userRepo;
            _referralRepo = referralRepo;
        }

        /// <summary>
        /// Registers a new user, optionally linking the account to a referral code.
        /// Returns a result containing user metadata and useful response data for verification and testing.
        /// </summary>
        /// <param name="model">The user registration data submitted by the client.</param>
        /// <returns>
        /// A <see cref="RegistrationResultModel"/> containing the generated user ID,
        /// referral code, referral information, and a copy of the submitted data.
        /// </returns>
        public async Task<RegistrationResultModel> RegisterUserAsync(RegistrationUserModel model)
        {
            if (await _userRepo.IsDuplicateNameAsync(model.FirstName, model.LastName))
            {
                return new RegistrationResultModel
                {
                    Success = false,
                    Input = model
                };
            }

            // Simulate generating a unique user ID and referral code
            var newUserId = Guid.NewGuid().ToString();
            var referralCode = Guid.NewGuid().ToString("N")[..6].ToUpper();

            var newUser = new UserProfile
            {
                UserId = newUserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ReferralCode = referralCode // TODO: Add this to existing registration process
            };

            await _userRepo.AddUserAsync(newUser);

            bool wasReferred = false;
            string? referredByName = null;

            // TODO: Add this block to existing registration process or create a new method to handle referral logic
            if (!string.IsNullOrWhiteSpace(model.ReferralCode))
            {
                var referringUser = await _userRepo.GetUserByReferralCodeAsync(model.ReferralCode);
                if (referringUser != null)
                {
                    wasReferred = true;
                    referredByName = $"{referringUser.FirstName} {referringUser.LastName}";

                    await _referralRepo.AddReferralAsync(new UserReferral
                    {
                        ReferrerUserId = referringUser.UserId,
                        ReferredUserId = newUserId,
                        Status = "Pending" //TODO: assuming a default status and that something will update it later to make it complete
                    });
                }
            }

            return new RegistrationResultModel
            {
                Success = true,
                UserId = newUserId,
                ReferralCode = referralCode,
                WasReferred = wasReferred,
                ReferredBy = referredByName,
                Input = model
            };
        }
    }
}
