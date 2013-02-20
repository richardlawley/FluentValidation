using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Validation;
using FluentValidation.WebApi;
using Moq;
using NUnit.Framework;

namespace FluentValidation.Tests.WebApi
{
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
            Assert.Inconclusive();
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
            Assert.Inconclusive();
        }


    }
}
