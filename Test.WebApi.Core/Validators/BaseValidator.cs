using System;
using System.Linq;
using Test.Entities;
using FluentValidation;


namespace Test.WebApi.Core.Validators
{
    /// <summary>
    /// Base class for validators
    /// </summary>
    /// <typeparam name="TModel">Type of model being validated</typeparam>
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel> where TModel : EntityBaseModel
    {
        #region Ctor

        protected BaseValidator()
        {
            PostInitialize();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Developers can override this method in custom partial classes in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
        }


        #endregion
    }
}