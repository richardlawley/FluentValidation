#region License

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at http://www.codeplex.com/FluentValidation

#endregion License

namespace FluentValidation.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using FluentValidation.Internal;
	using FluentValidation.Results;

	/// <summary>
	/// ModelValidator to wrap FluentValidation.  The purpose of this is to create the appropriate FluentValidation
	/// IValidator instance on demand to validate a model.  The instance must be constructed each time because this
	/// object (the ModelValidator) is cached by WebAPI.
	/// </summary>
	/// <typeparam name="TModel">Type of the model object being validated</typeparam>
	public class WebApiFluentValidationModelValidator<TModel> : ModelValidator
	{
		private readonly IValidatorFactory _factory;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="validatorProviders">Existing Validator Providers</param>
		/// <param name="factory">Factory for creating the underlying validator</param>
		public WebApiFluentValidationModelValidator(IEnumerable<ModelValidatorProvider> validatorProviders, IValidatorFactory factory)
			: base(validatorProviders)
		{
			if (factory == null) { throw new ArgumentNullException("factory"); }
			_factory = factory;
		}

		/// <summary>
		/// Generates a FluentValidation validator appropriate for the specified model, validates the model and
		/// converts the results to a format expected by WebAPI
		/// </summary>
		/// <param name="metadata">Model Metadata containing the model to validate</param>
		/// <param name="container">Container</param>
		/// <returns>
		/// A list of validation failures
		/// </returns>
		public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
		{
			if (metadata.Model != null)
			{
				// Create the validator we'll be using.  This should be successful as there shouldn't be an instance of
				// this class around for a validator that does not exist.
				IValidator validator = _factory.GetValidator<TModel>();
				if (validator == null)
				{
					throw new ArgumentException(String.Format("Could not locate a validator for Type {0}", typeof(TModel).FullName));
				}

				// Perform the validation
				IValidatorSelector selector = new DefaultValidatorSelector();
				ValidationContext context = new ValidationContext(metadata.Model, new PropertyChain(), selector);
				ValidationResult result = validator.Validate(context);

				// We only want a result if it's invalid
				if (!result.IsValid)
				{
					return ConvertValidationResultToModelValidationResults(result);
				}
			}

			// No result = valid
			return Enumerable.Empty<ModelValidationResult>();
		}

		/// <summary>
		/// Converts a FluentValidation Validation Result to Web API ModelValidationResult
		/// </summary>
		/// <param name="result">Re</param>
		/// <returns></returns>
		protected virtual IEnumerable<ModelValidationResult> ConvertValidationResultToModelValidationResults(ValidationResult result)
		{
			return result.Errors.Select(x => new ModelValidationResult
			{
				MemberName = x.PropertyName,
				Message = x.ErrorMessage
			});
		}
	}
}