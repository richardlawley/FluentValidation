using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using FluentValidation.Attributes;
using NUnit.Framework;

namespace FluentValidation.Tests.WebApi
{
    #region Test Controller and Model

    [Validator(typeof(TestModelValidator))]
    public class TestModel
    {
        public int Number { get; set; }
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Number).GreaterThan(5);
        }
    }

    public class IntegrationTestController : ApiController
    {
        [HttpPost]
        public bool ValidationTest(TestModel model)
        {
            return ModelState.IsValid;
        }
    }

    #endregion Test Controller and Model

    public class ValidatorIntegrationTest : WebApiIntegrationTest
    {
        [SetUp]
        public void SetUp()
        {
            InitialiseSelfHostedApplication();
        }

        /// <summary>
        /// When posting to an Action, the FluentValidator should be called by the Web API framework, which will cause
        /// the action to return False.
        /// </summary>
        [Test]
        public void ValidatorIsCalled_WhenAnActionIsCalled()
        {
            var request = CreateJsonRequest("api/IntegrationTest/ValidationTest", HttpMethod.Post, new TestModel { Number = 0 });
            using (var response = WebApiClient.SendAsync(request).Result)
            {
                bool result = response.Content.ReadAsAsync<bool>().Result;
                Assert.IsFalse(result);
            }
        }
    }
}