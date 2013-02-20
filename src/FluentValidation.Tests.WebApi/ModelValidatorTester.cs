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
	using System.Web.Http.Validation;
	using FluentValidation.WebApi;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class ModelValidatorTester
	{
		private Mock<IValidatorFactory> _mockFactory;
		private IEnumerable<ModelValidatorProvider> _modelValidatorProviders;

		public class TestModel { }

		[SetUp]
		public void SetUp()
		{
			_mockFactory = new Mock<IValidatorFactory>();
			_modelValidatorProviders = Enumerable.Empty<ModelValidatorProvider>();
		}

		[Test]
		public void Validate_ThrowsException_IfFactoryCannotCreateValidator()
		{
			WebApiFluentValidationModelValidator<TestModel> validator = new WebApiFluentValidationModelValidator<TestModel>(_modelValidatorProviders, _mockFactory.Object);
			TestModel model = null;

			Assert.Throws<ArgumentException>(() =>
			{
			});

			Assert.Inconclusive();
		}

		[Test]
		public void Validate_ReturnsNoResults_WhenSuccessful()
		{
			ModelValidator validator = null;
			ModelMetadata metadata = null;

			var results = validator.Validate(metadata, null);

			results.Any().ShouldBeFalse();
		}

		[Test]
		public void Validate_WhenInvalid_ConvertsMessagesToWebApiFormat()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void Validate_CreatesValidator_WhenModelIsNotNull()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void Validate_CallsValidateOnValidator()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void Validate_DoesNotCreateValidator_WhenModelIsNull()
		{
			_mockFactory.Setup(x => x.GetValidator(typeof())

			Assert.Inconclusive();
		}
	}
}