using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Validation;
using FluentValidation.Attributes;
using FluentValidation.WebApi;
using Moq;
using NUnit.Framework;

namespace FluentValidation.Tests.WebApi
{
    /// <summary>
    /// Tests an Integrated Web API Application using an in-memory self-host
    /// </summary>
    public abstract class WebApiIntegrationTest : IDisposable
    {
        protected string BaseUrl { get; private set; }

        protected HttpConfiguration Configuration { get; private set; }

        protected HttpServer WebApiServer { get; private set; }

        protected MediaTypeFormatter MediaTypeFormatter { get; private set; }

        protected HttpClient WebApiClient { get; private set; }

        public WebApiIntegrationTest()
        {
            BaseUrl = "http://testsite.local/";
            MediaTypeFormatter = new JsonMediaTypeFormatter();

            Configuration = new HttpConfiguration();
            Configuration.Routes.MapHttpRoute(
                name: "DefaultAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = RouteParameter.Optional });
            Configuration.Services.Add(typeof(ModelValidatorProvider), new WebApiFluentValidationModelValidatorProvider());

            InitialiseSelfHostedApplication();
        }


        protected void InitialiseSelfHostedApplication()
        {
            WebApiServer = new HttpServer(Configuration);
            WebApiClient = new HttpClient(WebApiServer);
        }

        public HttpRequestMessage CreateJsonRequest(string url, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri(BaseUrl + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;

            return request;
        }

        public HttpRequestMessage CreateJsonRequest<T>(string url, HttpMethod method, T content) where T : class
        {
            var request = CreateJsonRequest(url, method);
            request.Content = new ObjectContent<T>(content, MediaTypeFormatter);

            return request;
        }

        public void Dispose()
        {
            if (WebApiClient != null)
            {
                WebApiClient.Dispose();
                WebApiClient = null;
            }

            if (WebApiServer != null)
            {
                WebApiServer.Dispose();
                WebApiServer = null;
            }
        }
    }
}
