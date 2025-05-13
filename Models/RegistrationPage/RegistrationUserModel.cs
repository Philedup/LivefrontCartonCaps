using System.ComponentModel.DataAnnotations;

namespace LivefrontCartonCaps.Models.RegistrationPage
{
    /// <summary>
    /// User Registration Model for the Registration page. Probably an existing model.
    /// </summary>
    public class RegistrationUserModel
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// User's Email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Optional: User's phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Optional referral code provided by another user. NOTE: This is the new field that would need to be added to existing registration process
        /// </summary>
        public string? ReferralCode { get; set; }

        /// <summary>
        /// User's date of birth.
        /// </summary>
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// User's ZIP code (5-digit or ZIP+4 format).
        /// </summary>
        [RegularExpression(@"^\d{5}(?:[-\s]?\d{4})?$",
            ErrorMessage = "Please enter a valid ZIP code (e.g., 12345 or 12345-6789).")]
        public required string ZipCode { get; set; }
    }
}
