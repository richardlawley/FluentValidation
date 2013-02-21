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

namespace FluentValidation.Tests.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.Metadata;
	using System.Web.Http.Metadata.Providers;
	using System.Web.Http.Validation;
	using FluentValidation.Attributes;
	using FluentValidation.Results;
	using FluentValidation.WebApi;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class ModelValidatorTester
	{
		private Mock<IValidatorFactory> _mockFactory;
		private IEnumerable<ModelValidatorProvider> _modelValidatorProviders;
		private ModelValidator _validator;

		public class TestModel
		{
			public int Value { get; set; }
		}

		public class GreaterThan10ModelValidator : AbstractValidator<TestModel>
		{
			public GreaterThan10ModelValidator()
			{
				RuleFor(x => x.Value).GreaterThan(10);
			}
		}

		[SetUp]
		public void SetUp()
		{
			// Setup the Factory with one pass-through method for the model type we're using
			_mockFactory = new Mock<IValidatorFactory>();
			_mockFactory
				.Setup(x => x.GetValidator<TestModel>())
				.Returns(() => (IValidator<TestModel>)_mockFactory.Object.GetValidator(typeof(TestModel)));

			_modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();
			_validator = new WebApiFluentValidationModelValidator<TestModel>(_modelValidatorProviders, _mockFactory.Object);
		}

		[Test]
		public void Validate_ThrowsException_IfFactoryCannotCreateValidator()
		{
			_mockFactory.Setup(x => x.GetValidator(It.IsAny<Type>())).Returns((IValidator)null);

			TestModel model = new TestModel();
			ModelMetadata metadata = CreateMetaData(model);

			Assert.Throws<ArgumentException>(() =>
			{
				_validator.Validate(metadata, null);
			});
		}

		[Test]
		public void Validate_ReturnsNoResults_WhenSuccessful()
		{
			_mockFactory.Setup(x => x.GetValidator(typeof(TestModel))).Returns(() => new GreaterThan10ModelValidator());

			TestModel model = new TestModel { Value = 11 };
			ModelMetadata metadata = CreateMetaData(model);

			var results = _validator.Validate(metadata, null);

			results.Any().ShouldBeFalse();
		}

		[Test]
		public void Validate_WhenInvalid_ConvertsMessagesToWebApiFormat()
		{
			// Fixed failure validator
			ValidationFailure inFailure = new ValidationFailure("Value", "Foo");
			Mock<IValidator<TestModel>> mockValidator = new Mock<IValidator<TestModel>>(MockBehavior.Strict);
			mockValidator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(new ValidationResult(new[] { inFailure }));
			_mockFactory.Setup(x => x.GetValidator(typeof(TestModel))).Returns(mockValidator.Object);

			TestModel model = new TestModel();
			ModelMetadata metadata = CreateMetaData(model);

			IEnumerable<ModelValidationResult> results = _validator.Validate(metadata, null);

			results.Count().ShouldEqual(1);
			ModelValidationResult outFailure = results.First();

			outFailure.MemberName.ShouldEqual(inFailure.PropertyName);
			outFailure.Message.ShouldEqual(inFailure.ErrorMessage);
		}

		[Test]
		public void Validate_CallsValidateOnValidator()
		{
			Mock<IValidator<TestModel>> mockValidator = new Mock<IValidator<TestModel>>(MockBehavior.Strict);
			mockValidator.Setup(x => x.Validate(It.IsAny<ValidationContext>())).Returns(new ValidationResult());
			_mockFactory.Setup(x => x.GetValidator(typeof(TestModel))).Returns(mockValidator.Object);

			TestModel model = new TestModel();
			ModelMetadata metadata = CreateMetaData(model);

			var results = _validator.Validate(metadata, null);

			mockValidator.Verify(x => x.Validate(It.IsAny<ValidationContext>()), Times.Once(), "Validate on internal validator was not called");
		}

		/// <summary>
		/// Ensures that the ModelValidator creates an internal validator when passed a non-null model
		/// </summary>
		[Test]
		public void Validate_CreatesValidator_WhenModelIsNotNull()
		{
			SetupValidatorFactory();

			ModelMetadata metadata = CreateMetaData<TestModel>(new TestModel());
			var results = _validator.Validate(metadata, null);

			_mockFactory.Verify(x => x.GetValidator(typeof(TestModel)), Times.Once(), "ValidatorFactory was not called");
		}

		/// <summary>
		/// Ensures that the ModelValidator does not create an internal validator when passed a null model
		/// </summary>
		[Test]
		public void Validate_DoesNotCreateValidator_WhenModelIsNull()
		{
			SetupValidatorFactory();

			ModelMetadata metadata = CreateMetaData<TestModel>(null);
			var result = _validator.Validate(metadata, null);

			_mockFactory.Verify(x => x.GetValidator(typeof(TestModel)), Times.Never(), "ValidatorFactory was called");
		}

		private void SetupValidatorFactory()
		{
			_mockFactory
				.Setup(x => x.GetValidator(typeof(TestModel)))
				.Returns(new GreaterThan10ModelValidator());
		}

		protected ModelMetadata CreateMetaData<TModelType>(TModelType model)
		{
			var meta = new DataAnnotationsModelMetadataProvider();
			return meta.GetMetadataForType(() => model, typeof(TModelType));
		}
	}
}