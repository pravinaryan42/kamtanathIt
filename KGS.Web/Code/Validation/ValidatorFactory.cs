
using KGS.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using KGS.Dto;

namespace KGS.Web.Code.Validation
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        private static Dictionary<Type, IValidator> _validators = new Dictionary<Type, IValidator>();

        static ValidatorFactory()
        {
            //Add validators here eg. below
            //_validators.Add(typeof(IValidator<T>), new ValidatorClass());
            _validators.Add(typeof(IValidator<SignUpModel>), new SignUpModelValidator());
            _validators.Add(typeof(IValidator<UserLoginViewModel>), new LoginValidator());
     
        }

        /// <summary>
        /// Creates an instance of a validator with the given type.
        /// </summary>
        /// <param name="validatorType">Type of the validator.</param>
        /// <returns>The newly created validator</returns>
        public override IValidator CreateInstance(Type validatorType)
        {
            IValidator validator;
            if (_validators.TryGetValue(validatorType, out validator))
                return validator;
            return validator;
        }
    }
}