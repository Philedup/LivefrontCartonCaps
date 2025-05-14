namespace LivefrontCartonCaps.Models.ReferralsPage
{
    /// <summary>
    /// Model used for tracking clicks on referral links.
    /// Reserved for potential future use with in-house deep link tracking.
    /// </summary>
    public class TrackReferralResultModel
    {
        /// <summary>
        /// The referral code included in the link.
        /// </summary>
        public string ReferralCode { get; set; } = default!;

        /// <summary>
        /// The channel/method used to share the link (e.g., email, sms, etc.).
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// Optionally track device identifier (if available).
        /// </summary>
        public string? DeviceId { get; set; }

        /// <summary>
        /// Optionally track IP address (if available).
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// When the referral link was clicked.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}