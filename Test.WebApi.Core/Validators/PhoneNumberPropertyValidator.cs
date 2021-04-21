using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation.Validators;

namespace Test.WebApi.Core.Validators
{
    /// <summary>
    /// Phohe number validator
    /// </summary>
    public class PhoneNumberPropertyValidator : PropertyValidator
    {

        /// <summary>
        /// Ctor
        /// </summary>
        public PhoneNumberPropertyValidator()
            : base("Phone number is not valid")
        {
        }

        /// <summary>
        /// Is valid?
        /// </summary>
        /// <param name="context">Validation context</param>
        /// <returns>Result</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return IsValid(context.PropertyValue as string);
        }

        /// <summary>
        /// Is valid?
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="customerSettings">Customer settings</param>
        /// <returns>Result</returns>
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, ValidatorDefaults.PhoneNumberValidationRule, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }
    }
}
