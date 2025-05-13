using LivefrontCartonCaps.Entities;

namespace LivefrontCartonCaps.MockData
{
    /// <summary>
    /// Interface for accessing and managing user referrals.
    /// </summary>
    public interface IUserReferralRepository
    {
        /// <summary>
        /// Gets the list of referrals made by the specified user.
        /// </summary>
        /// <param name="referrerUserId">The ID of the user who made the referrals.</param>
        /// <returns>A list of <see cref="UserReferral"/> entries.</returns>
        Task<List<UserReferral>> GetReferralsByUserAsync(string referrerUserId);

        /// <summary>
        /// Adds a new referral to the repository.
        /// </summary>
        /// <param name="referral">The referral entity to add.</param>
        /// <returns>A completed task when the referral is added.</returns>
        Task AddReferralAsync(UserReferral referral);
    }
}
