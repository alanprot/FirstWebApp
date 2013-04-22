using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using WebApp.Domain;

namespace WebApp.Controllers
{
    [System.Web.Script.Services.ScriptService]
    public class HomeController : Controller
    {
        //
        public ActionResult Index()
        {
            List<string> teste = new List<string>();
            teste.Add("sad");
            ViewBag.Teste = teste;
            return View();
        }

        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public JsonResult api(string route)
        {
            TesteDomain a = new TesteDomain();
            a.Nome = "alan";
            a.Idade = 25;
            a.Save();
            return Json(a);
        }



    }
}
