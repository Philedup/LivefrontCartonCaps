namespace LivefrontCartonCaps.Models.Config
{
    /// <summary>
    /// Settings for the referral system, including the base URL for referral links.
    /// </summary>
    public class ReferralSettings
    {
        /// <summary>
        /// Base URL for generating referral links. This should be set in the appsettings.json file and then replaced from an environment variable during the pipeline build/deploy.
        /// </summary>
        public string ReferralLinkBase { get; set; } = string.Empty;
    }
}