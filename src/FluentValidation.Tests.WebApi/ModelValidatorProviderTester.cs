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
	using FluentValidation.WebApi;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class ModelValidatorProviderTester
	{
		public class TestModel { }

		public class TestModelValidator : AbstractValidator<TestModel> { }

		[Validator(typeof(TestModelValidator2))]
		public class TestModel2 { }

		public class TestModelValidator2 : AbstractValidator<TestModel2> { }

		public class TestModel3 { }

		[Test]
		public void GetValidator_UsingValidatorFactory_ReturnsValidator()
		{
			Mock<IValidatorFactory> mockValidatorFactory = new Mock<IValidatorFactory>();
			mockValidatorFactory.Setup(x => x.GetValidator(typeof(TestModel))).Returns(new TestModelValidator());

			ModelValidatorProvider provider = new WebApiFluentValidationModelValidatorProvider(mockValidatorFactory.Object);
			ModelMetadata metadata = CreateMetaData(typeof(TestModel));

			var results = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

			mockValidatorFactory.Verify(x => x.GetValidator(typeof(TestModel)), Times.Once());
			Assert.AreEqual(1, results.Count());
			Assert.IsAssignableFrom<WebApiFluentValidationModelValidator<TestModel>>(results.Single());
		}

		[Test]
		public void GetValidator_WithoutFactory_ReturnsValidatorFromDefaultFactory()
		{
			ModelValidatorProvider provider = new WebApiFluentValidationModelValidatorProvider();
			ModelMetadata metadata = CreateMetaData(typeof(TestModel2));

			IEnumerable<ModelValidator> results = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

			Assert.AreEqual(1, results.Count());
			Assert.IsAssignableFrom<WebApiFluentValidationModelValidator<TestModel2>>(results.Single());
		}

		[Test]
		public void GetValidator_ForUnregisteredValidator_ReturnsNoValidators()
		{
			ModelValidatorProvider provider = new WebApiFluentValidationModelValidatorProvider();
			ModelMetadata metadata = CreateMetaData(typeof(TestModel3));

			IEnumerable<ModelValidator> results = provider.GetValidators(metadata, Enumerable.Empty<ModelValidatorProvider>());

			Assert.AreEqual(0, results.Count());
		}

		protected ModelMetadata CreateMetaData(Type type)
		{
			var meta = new DataAnnotationsModelMetadataProvider();
			return meta.GetMetadataForType(null, type);
		}
	}
}