namespace LivefrontCartonCaps.Entities
{
    /// <summary>
    /// Represents a user profile in the system. Should be an existing model.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Normally an id or guid but for readablity left as a string
        /// </summary>
        public string UserId { get; set; } = default!;
        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirstName { get; set; } = default!;
        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; set; } = default!;
        /// <summary>
        /// User's email address.
        /// </summary>
        public string? Email { get; set; } = default!;
        /// <summary>
        /// User's phone number.
        /// </summary>
        public string? PhoneNumber { get; set; } = default!;
        /// <summary>
        /// User's Referral Code. Every user has to have one. Note: either need to generate this for existing users or has a process to check if a code exists and then generate it if it doesn't. TODO: Add this to existing user profile table.
        /// </summary>
        public string ReferralCode { get; set; } = default!;
        /// <summary>
        /// Needed some way to determine if the user has successfully completed the registration process. Eventually replace this with business logic to determine if the user is registered or not.
        /// </summary>
        public bool RegistrationComplete { get; set; }
    }
}