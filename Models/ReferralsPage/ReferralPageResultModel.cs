namespace LivefrontCartonCaps.Models.ReferralsPage
{
    /// <summary>
    /// Page view model for the new referral page.
    /// </summary>
    public class ReferralPageResultModel
    {
        /// <summary>
        /// The unique referral code assigned to the user.
        /// </summary>
        public required string ReferralCode { get; set; }

        /// <summary>
        /// Optional: The phone number to which the referral will be sent via SMS. Put this property here in case it is needed by the app.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Optional: The email address to which the referral will be sent. Put this property here in case it is needed by the app
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The base referral link (e.g., https://cartoncaps.app/r). Added a property for this in case the link can change depending on business logic, remove if link will always be the same and can be handled in the app.
        /// </summary>
        public required string ReferralLinkBase { get; set; }

        /// <summary>
        /// The full referral link with code parameter appended. Remove this property if app can handle building the link.
        /// </summary>
        public string FullReferralLink => $"{ReferralLinkBase}?referral_code={ReferralCode}";

        /// <summary>
        /// Optional: Custom subject line for the referral email.
        /// </summary>
        public string? EmailSubject { get; set; }

        /// <summary>
        /// Optional: Custom message body for the referral email.
        /// </summary>
        public string? EmailMessage { get; set; }

        /// <summary>
        /// Optional: Custom text message to send via SMS.
        /// </summary>
        public string? TextMessage { get; set; }
    }

}
