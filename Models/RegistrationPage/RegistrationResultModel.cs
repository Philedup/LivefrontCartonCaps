namespace LivefrontCartonCaps.Models.RegistrationPage
{
    /// <summary>
    /// Response returned after a user registration attempt. Probably an existing model.
    /// </summary>
    public class RegistrationResultModel
    {
        /// <summary>
        /// Indicates whether the registration was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The unique identifier generated for the new user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// The referral code generated for the new user.
        /// </summary>
        public string? ReferralCode { get; set; }

        /// <summary>
        /// Indicates whether the user was referred by another user.
        /// </summary>
        public bool WasReferred { get; set; }

        /// <summary>
        /// The name of the user who referred the new user, if applicable.
        /// </summary>
        public string? ReferredBy { get; set; }

        /// <summary>
        /// Echo of the original input model used during registration.
        /// Helps users confirm what data they submitted.
        /// </summary>
        public RegistrationUserModel? Input { get; set; }
    }
}
