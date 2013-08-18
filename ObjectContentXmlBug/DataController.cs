namespace ObjectContentXmlBug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    [Serializable]
    public class DataClass
    {
        private readonly string name;

        private readonly int age;

        private readonly bool awesome;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
        }

        public bool Awesome
        {
            get
            {
                return awesome;
            }
        }

        public DataClass(string name, int age, bool awesome)
        {
            this.name = name;
            this.age = age;
            this.awesome = awesome;
        }
    }

    public class DataFailController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var query =
                new List<DataClass>
                {
                    new DataClass("Peter", 29, true),
                    new DataClass("Kathryn", 26, false)
                }.AsQueryable();

            var conneg = (IContentNegotiator)GlobalConfiguration.Configuration.Services.GetService(typeof(IContentNegotiator));
            var formatter = conneg.Negotiate(
                query.GetType(),
                Request,
                GlobalConfiguration.Configuration.Formatters);

            var response = Request.CreateResponse(HttpStatusCode.OK, query);
            response.Content = new ObjectContent(query.GetType(), query, formatter.Formatter);

            return response;
        }
    }

    public class DataSuccessController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var query =
                new List<DataClass>
                {
                    new DataClass("Peter", 29, true),
                    new DataClass("Kathryn", 26, false)
                }.AsQueryable();

            var conneg = (IContentNegotiator)GlobalConfiguration.Configuration.Services.GetService(typeof(IContentNegotiator));
            var formatter = conneg.Negotiate(
                query.GetType(),
                Request,
                GlobalConfiguration.Configuration.Formatters);

            var response = Request.CreateResponse(HttpStatusCode.OK, query);
            response.Content = new ObjectContent<IQueryable<DataClass>>(query, formatter.Formatter);

            return response;
        }
    }
}
