namespace LivefrontCartonCaps.Entities
{
    /// <summary>
    /// Represents a referral made by a user to another user.
    /// </summary>
    public class UserReferral
    {
        /// <summary>
        /// Unique identifier for the referral.
        /// </summary>
        public string ReferrerUserId { get; set; } = default!;

        /// <summary>
        /// Unique identifier for the referred user.
        /// </summary>
        public string ReferredUserId { get; set; } = default!;

        /// <summary>
        /// Status of the referral.
        /// </summary>
        public string Status { get; set; } = "Pending"; // Could also enum this later

        /// <summary>
        /// Date and time when the referral was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }

}
