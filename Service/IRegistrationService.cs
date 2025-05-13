using LivefrontCartonCaps.Models.RegistrationPage;

namespace LivefrontCartonCaps.Services
{
    /// <summary>
    /// Service for managing user registrations, including referral logic.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Registers a new user, optionally linking the account to a referral code.
        /// </summary>
        /// <param name="model">The registration details submitted by the user.</param>
        /// <returns>
        /// A task that resolves to a <see cref="RegistrationResultModel"/>, 
        /// containing user ID, referral info, and echo of submitted data.
        /// </returns>
        Task<RegistrationResultModel> RegisterUserAsync(RegistrationUserModel model);
    }
}