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
#endregion

namespace FluentValidation.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.Metadata;
	using System.Web.Http.Validation;
	using FluentValidation.Attributes;

	/// <summary>
	/// Provides ModelValidator instances to validate a type of model
	/// </summary>
	public class WebApiFluentValidationModelValidatorProvider : ModelValidatorProvider
	{
		private IValidatorFactory _factory;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebApiFluentValidationModelValidatorProvider" /> class.
		/// </summary>
		/// <param name="factory">Factory for constructing FluentValidation validators.  AttributedValidatorFactory will be used if this is left null</param>
		public WebApiFluentValidationModelValidatorProvider(IValidatorFactory factory = null)
		{
			if (factory == null)
			{
				// No validator factory was supplied, so use the one which reads the Validator attribute on the model class
				factory = new AttributedValidatorFactory();
			}
			_factory = factory;
		}

		/// <summary>
		/// Returns ModelValidators suitable for validating the model type specified within metadata.
		/// </summary>
		/// <param name="metadata">Model metadata containing the type of model to be validated</param>
		/// <param name="validatorProviders">Existing Model Validator Providers</param>
		/// <returns></returns>
		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<ModelValidatorProvider> validatorProviders)
		{
            List<ModelValidator> validators = new List<ModelValidator>();

			// Attempt to create a validator for the Model Type specified in the metadata
			IValidator validator = _factory.GetValidator(metadata.ModelType);
			if (validator != null)
			{
				// As WebAPI caches the ModelValidator returned by this method, we won't be storing any instance of the
				// validator itself.  Instead, create a ModelValidator that knows how to construct the IValidator 
				// instance when it is needed.
				Type modelValidatorType = typeof(WebApiFluentValidationModelValidator<>).MakeGenericType(metadata.ModelType);
				ModelValidator modelValidator = (ModelValidator)Activator.CreateInstance(modelValidatorType, validatorProviders, _factory);

                validators.Add(modelValidator);
			}

            return validators;
		}
	}
}