namespace LivefrontCartonCaps.Models.ReferralsPage
{
    /// <summary>
    /// Referrals Model for the My Referral list on the Referrals page.
    /// </summary>
    public class ReferralsModel
    {
        /// <summary>
        /// Name of the referral.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Status of the referral (e.g., "Pending", "Accepted", "Rejected").
        /// </summary>
        public required string Status { get; set; }
    }
}
