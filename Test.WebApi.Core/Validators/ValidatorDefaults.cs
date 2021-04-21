namespace Test.WebApi.Core.Validators
{
    /// <summary>
    /// Represents default values related to validators
    /// </summary>
    public static partial class ValidatorDefaults
    {
        /// <summary>
        /// Gets the name of a rule set used to validate model
        /// </summary>
        public static string ValidationRuleSet => "Validate";
        /// <summary>
        /// Gets or sets a phone number validation rule
        /// </summary>
        public static string PhoneNumberValidationRule => "";
    }
}