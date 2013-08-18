namespace ObjectContentXmlBug
{
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WebAPI.Testing;

    [TestClass]
    public class BugTest
    {
        private Browser browser;

        [TestInitialize]
        public void Setup()
        {
            var config = new HttpConfiguration();
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            browser = new Browser(config);
        }

        [TestMethod]
        public void Fails()
        {
            var response = browser.Get(
            "/api/datafail/",
            (with) =>
            {
                with.Header("Accept", "application/xml");
                with.HttpRequest();
            });

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Content.Headers.GetValues("Content-Type").FirstOrDefault().StartsWith("application/xml"));
        }

        [TestMethod]
        public void Succeeds()
        {
            var response = browser.Get(
            "/api/datasuccess/",
            (with) =>
            {
                with.Header("Accept", "application/xml");
                with.HttpRequest();
            });

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Content.Headers.GetValues("Content-Type").FirstOrDefault().StartsWith("application/xml"));
        }
    }
}
