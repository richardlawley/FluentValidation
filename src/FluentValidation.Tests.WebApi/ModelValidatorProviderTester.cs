namespace FluentValidation.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata;
    using System.Web.Http.Metadata.Providers;
    using System.Web.Http.Validation;
    using System.Web.Http.Validation.Providers;
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
