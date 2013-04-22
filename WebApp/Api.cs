using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using WebApp.Domain;

namespace WebApp
{
    [System.Web.Script.Services.ScriptService]
    [ServiceContract]
    public partial class Api : WebService
    {

        [WebInvoke(UriTemplate = "/Users",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "GET")]
        [OperationContract]
        public IEnumerable<TesteDomain> Users()
        {
            return TesteDomain.FindAll();
        }

        [WebInvoke(UriTemplate = "/Users/{id}",
     RequestFormat = WebMessageFormat.Json,
     ResponseFormat = WebMessageFormat.Json,
     Method = "GET")]
        [OperationContract]
        public TesteDomain GetUser(string id)
        {
            return TesteDomain.GetById(id);
        }

        [WebInvoke(UriTemplate = "/UsersPerPage/{page}",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json,
Method = "POST")]
        [OperationContract]
        public IEnumerable<TesteDomain> GetUserByObj(TesteDomain user, string page)
        {
            int pageInt = 0;
            Int32.TryParse(page, out pageInt);

            if (string.IsNullOrEmpty(page) || pageInt == 0)
            {
                return TesteDomain.FindAll(x => x.Nome.Contains(user.Nome));
            }
            
            return TesteDomain.GetPage((a, b) => a.Idade - b.Idade, pageInt, x => x.Nome.Contains(user.Nome));
        }

    }
}