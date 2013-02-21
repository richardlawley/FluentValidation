namespace FluentValidation.Tests.WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Metadata.Providers;
	using System.Web.Http.ModelBinding;
	using System.Web.Http.ModelBinding.Binders;
	using System.Web.Http.Validation;
	using System.Web.Http.ValueProviders.Providers;
	using FluentValidation.Attributes;
	using FluentValidation.WebApi;
	using NUnit.Framework;

	[TestFixture]
	public class WebApiValidatorTester
	{
		//private HttpActionContext _actionContext;

		//[SetUp]
		//public void SetUp()
		//{
		//	_actionContext = new HttpActionContext
		//	{
		//		ControllerContext = new HttpControllerContext
		//		{
		//			Configuration = new HttpConfiguration
		//			{
		//			}
		//		}
		//	};

		//	// Set up this Fixture to use a default FV MVP with no factory
		//	_actionContext.ControllerContext.Configuration.Services.Add(
		//		typeof(ModelValidatorProvider),
		//		new WebApiFluentValidationModelValidatorProvider());
		//}

		//public class PersonValidator : AbstractValidator<Person>
		//{
		//	public PersonValidator()
		//	{
		//		RuleFor(p => p.Name).NotEmpty();
		//		RuleFor(p => p.Email).EmailAddress();
		//	}
		//}

		//[Validator(typeof(PersonValidator))]
		//public class Person
		//{
		//	public string Name { get; set; }

		//	public string Email { get; set; }
		//}

		//[Test]
		//public void Should_not_have_valid_fields_when_uninitialized()
		//{
		//	var binder = new MutableObjectModelBinder();

		//	var person = new Person();

		//	var context = CreateModelBindingContext(person);

		//	binder.BindModel(_actionContext, context);

		//	context.ModelState.IsValidField("test.Name").ShouldBeFalse();
		//	context.ModelState.IsValidField("test.Email").ShouldBeFalse();
		//}

		//private static ModelBindingContext CreateModelBindingContext<T>(T instance)
		//{
		//	var modelName = "test";
		//	var context = new ModelBindingContext
		//	{
		//		ModelName = modelName,
		//		ModelMetadata = new DataAnnotationsModelMetadataProvider().GetMetadataForType(null, typeof(T)),
		//		ModelState = new ModelStateDictionary(),
		//		FallbackToEmptyPrefix = true,
		//		ValueProvider = new NameValuePairsValueProvider(GetValues(instance, modelName), new CultureInfo("en-US"))
		//	};
		//	return context;
		//}

		//private static IEnumerable<KeyValuePair<string, string>> GetValues<T>(T instance, string modelName)
		//{
		//	var values = new List<KeyValuePair<string, string>>();

		//	var type = typeof(T);
		//	foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
		//	{
		//		if (property.CanRead)
		//		{
		//			var value = property.GetValue(instance, null);
		//			values.Add(new KeyValuePair<string, string>(string.Format("{0}.{1}", modelName, property.Name), value == null ? null : value.ToString()));
		//		}
		//	}

		//	return values;
		//}
	}
}