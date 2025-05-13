using LivefrontCartonCaps.Entities;

namespace LivefrontCartonCaps.MockData
{
    /// <summary>
    /// Mock implementation of the <see cref="IUserProfileRepository"/> interface for testing purposes.
    /// Simulates asynchronous operations on an in-memory user list.
    /// </summary>
    public class MockUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _users = new()
        {
            new() { UserId = "user1", FirstName = "Phil", LastName = "Phan", Email = "me@philphan.com", ReferralCode = "plp1013", PhoneNumber = "952-872-1211" },
            new() { UserId = "user2", FirstName = "Sam", LastName = "Kirchmeier", Email = "skirchmeier@livefront.com", ReferralCode = "vpeng01", PhoneNumber = "952-872-1001" },
            new() { UserId = "user3", FirstName = "Adam", LastName = "May", Email = "amay@livefront.com", ReferralCode = "direng02", PhoneNumber = "952-872-1002" },
            new() { UserId = "user4", FirstName = "Brian", LastName = "Yencho", Email = "byencho@livefront.com", ReferralCode = "direng03", PhoneNumber = "952-872-1003" },
            new() { UserId = "user5", FirstName = "Forrest", LastName = "Tracy", Email = "ftracy@livefront.com", ReferralCode = "direng04", PhoneNumber = "952-872-1004" },
            new() { UserId = "user6", FirstName = "Steve", LastName = "Horn", Email = "shorn@livefront.com", ReferralCode = "direng05", PhoneNumber = "952-872-1005" },
            new() { UserId = "user7", FirstName = "James", LastName = "Fishwick", Email = "jfishwick@livefront.com", ReferralCode = "dirsol06", PhoneNumber = "952-872-1006" },
            new() { UserId = "user8", FirstName = "Collin", LastName = "Flynn", Email = "cflynn@livefront.com", ReferralCode = "prieng07", PhoneNumber = "952-872-1007" },
            new() { UserId = "user9", FirstName = "Sean", LastName = "Weiser", Email = "sweiser@livefront.com", ReferralCode = "prieng08", PhoneNumber = "952-872-1008" },
            new() { UserId = "user10", FirstName = "Adrian", LastName = "Missy", Email = "amissy@livefront.com", ReferralCode = "solarc09", PhoneNumber = "952-872-1009" },
            new() { UserId = "user11", FirstName = "Ameer", LastName = "Akashe", Email = "aakashe@livefront.com", ReferralCode = "solarc10", PhoneNumber = "952-872-1010" },
            new() { UserId = "user12", FirstName = "Sean", LastName = "Ephraim", Email = "sephraim@livefront.com", ReferralCode = "solarc11", PhoneNumber = "952-872-1011" },
            new() { UserId = "user13", FirstName = "Will", LastName = "Redington", Email = "wredington@livefront.com", ReferralCode = "solarc12", PhoneNumber = "952-872-1012" }
        };

        /// <summary>
        /// Retrieves a user by their unique user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The matching <see cref="UserProfile"/> if found; otherwise, <c>null</c>.</returns>
        public Task<UserProfile?> GetUserByIdAsync(string userId) =>
            Task.FromResult(_users.FirstOrDefault(u => u.UserId == userId));

        /// <summary>
        /// Retrieves a user by their referral code.
        /// </summary>
        /// <param name="code">The referral code to search by.</param>
        /// <returns>The matching <see cref="UserProfile"/> if found; otherwise, <c>null</c>.</returns>
        public Task<UserProfile?> GetUserByReferralCodeAsync(string code) =>
            Task.FromResult(_users.FirstOrDefault(u => u.ReferralCode.Equals(code, StringComparison.OrdinalIgnoreCase)));

        /// <summary>
        /// Adds a new user to the mock repository.
        /// </summary>
        /// <param name="user">The user profile to add.</param>
        /// <returns>A completed task when the user has been added.</returns>
        public Task AddUserAsync(UserProfile user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Determines whether a user with the same first and last name already exists.
        /// </summary>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <returns><c>true</c> if a duplicate exists; otherwise, <c>false</c>.</returns>
        public Task<bool> IsDuplicateNameAsync(string firstName, string lastName) =>
            Task.FromResult(_users.Any(u =>
                u.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)));
    }
}
