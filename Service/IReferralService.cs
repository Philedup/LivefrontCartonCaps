using LivefrontCartonCaps.Models.ReferralsPage;

namespace LivefrontCartonCaps.Services
{
    /// <summary>
    /// Service for managing user referrals and generating referral links.
    /// </summary>
    public interface IReferralService
    {
        /// <summary>
        /// Gets the referral page model for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the requesting user.</param>
        /// <returns>A <see cref="ReferralPageResultModel"/> containing referral details and share links.</returns>
        Task<ReferralPageResultModel> GetReferralPageModelAsync(string userId);

        /// <summary>
        /// Gets the list of referrals made by the current user.
        /// </summary>
        /// <param name="userId">The unique ID of the user requesting their referral history.</param>
        /// <returns>A <see cref="MyReferralsResultModel"/> containing referred users and statuses.</returns>
        Task<MyReferralsResultModel> GetMyReferralsAsync(string userId);
    }
}
