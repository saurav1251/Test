using FluentValidation;

namespace Test.WebApi.Core.Validators
{
    /// <summary>
    /// Validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Set credit card validator
        /// </summary>
        /// <typeparam name="TModel">Type of model being validated</typeparam>
        /// <param name="ruleBuilder">Rule builder</param>
        /// <returns>Result</returns>
        public static IRuleBuilderOptions<TModel, string> IsCreditCard<TModel>(this IRuleBuilder<TModel, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CreditCardPropertyValidator());
        }

        /// <summary>
        /// Set decimal validator
        /// </summary>
        /// <typeparam name="TModel">Type of model being validated</typeparam>
        /// <param name="ruleBuilder">Rule builder</param>
        /// <param name="maxValue">Maximum value</param>
        /// <returns>Result</returns>
        public static IRuleBuilderOptions<TModel, decimal> IsDecimal<TModel>(this IRuleBuilder<TModel, decimal> ruleBuilder, decimal maxValue)
        {
            return ruleBuilder.SetValidator(new DecimalPropertyValidator(maxValue));
        }

        /// <summary>
        /// Set phone number validator
        /// </summary>
        /// <typeparam name="TModel">Type of model being validated</typeparam>
        /// <param name="ruleBuilder">Rule builder</param>
        /// <param name="customerSettings">Customer settings</param>
        /// <returns>Result</returns>
        public static IRuleBuilderOptions<TModel, string> IsPhoneNumber<TModel>(this IRuleBuilder<TModel, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhoneNumberPropertyValidator());
        }
    }
}