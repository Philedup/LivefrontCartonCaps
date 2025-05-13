using LivefrontCartonCaps.Entities;

namespace LivefrontCartonCaps.MockData
{
    /// <summary>
    /// Mock implementation of the IUserReferralRepository interface for testing purposes.
    /// </summary>
    public class MockUserReferralRepository : IUserReferralRepository
    {
        private readonly List<UserReferral> _referrals = new()
        {
            new() { ReferrerUserId = "user1", ReferredUserId = "user3", Status = "Pending" },
            new() { ReferrerUserId = "user1", ReferredUserId = "user6", Status = "Accepted" }
        };

        /// <summary>
        /// Gets the list of referrals made by the specified user.
        /// </summary>
        /// <param name="referrerUserId">The ID of the user who made the referrals.</param>
        /// <returns>A list of <see cref="UserReferral"/> entries.</returns>
        public Task<List<UserReferral>> GetReferralsByUserAsync(string referrerUserId)
        {
            var result = _referrals
                .Where(r => r.ReferrerUserId == referrerUserId)
                .ToList();

            return Task.FromResult(result);
        }

        /// <summary>
        /// Adds a new referral to the repository.
        /// </summary>
        /// <param name="referral">The referral entity to add.</param>
        /// <returns>A completed task once the referral has been added.</returns>
        public Task AddReferralAsync(UserReferral referral)
        {
            _referrals.Add(referral);
            return Task.CompletedTask;
        }
    }
}
