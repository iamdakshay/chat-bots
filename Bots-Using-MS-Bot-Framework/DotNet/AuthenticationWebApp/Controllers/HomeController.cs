using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoginWithSharePoint(string userName)
        {
            /// Save User Id to session
            Session["SkypeUserID"] = userName;

            string spAuth_SiteUri = Convert.ToString(ConfigurationManager.AppSettings["SPAUTH_SITEURI"]);
            string spAuth_AppClientId = Convert.ToString(ConfigurationManager.AppSettings["SPAUTH_APPCLIENTID"]);
            string spAuth_RedirectUri = Convert.ToString(ConfigurationManager.AppSettings["SPAUTH_REDIRECTURI"]);

            string url = $"{spAuth_SiteUri}/_layouts/15/appredirect.aspx?client_id={spAuth_AppClientId}&redirect_uri={spAuth_RedirectUri}";


            /// Redirect to login page
            return Redirect(url);
        }

        public ActionResult LoggedinToSharePoint()
        {
            string contextToken = this.Request.Form["SPAppToken"];
            string userName = Convert.ToString(Session["SkypeUserID"]);

            new Mongo().Insert("ContextTokens", new Token(userName, contextToken));

            return View();
        }
    }
}