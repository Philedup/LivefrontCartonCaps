namespace LivefrontCartonCaps.Models.ReferralsPage
{
    /// <summary>
    /// Model for a user's referrals. Broke this out into its own model to keep the code clean, organized, and reusable in case referrals are used in another area of the app. 
    /// Otherwise could also be part of the ReferralPageModel and loaded at the same time.
    /// </summary>
    public class MyReferralsResultModel
    {
        /// <summary>
        /// List of individual referrals associated with the user.
        /// </summary>
        public List<ReferralsModel> Referrals { get; set; } = new();
    }
}
