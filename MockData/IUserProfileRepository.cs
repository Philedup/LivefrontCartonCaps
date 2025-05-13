using LivefrontCartonCaps.Entities;

namespace LivefrontCartonCaps.MockData
{
    /// <summary>
    /// Mock repository for user profiles, simulating asynchronous database operations.
    /// </summary>
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique user ID.</param>
        /// <returns>A matching <see cref="UserProfile"/> or null.</returns>
        Task<UserProfile?> GetUserByIdAsync(string userId);

        /// <summary>
        /// Gets a user by their referral code.
        /// </summary>
        /// <param name="code">The referral code to match.</param>
        /// <returns>A matching <see cref="UserProfile"/> or null.</returns>
        Task<UserProfile?> GetUserByReferralCodeAsync(string code);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to add.</param>
        Task AddUserAsync(UserProfile user);

        /// <summary>
        /// Checks if a user with the same first and last name already exists in the repository.
        /// </summary>
        /// <param name="firstName">First name of the user.</param>
        /// <param name="lastName">Last name of the user.</param>
        /// <returns>True if a matching name exists; otherwise, false.</returns>
        Task<bool> IsDuplicateNameAsync(string firstName, string lastName);
    }
}
