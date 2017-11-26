using Common;
using Common.Contracts;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
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

        public ActionResult LoginWithAzure(string channelId, string userId)
        {
            /// Save User Id to session
            Session["channelId"] = channelId;
            Session["userId"] = userId;

            string tenantId = "8c3dad1d-xxxx-xxxx-xxxx-8263372eced6";
            string clientId = "5e9569bf-xxxx-xxxx-xxxx-fd33a25b9267";
            string redirect_uri = $"https://localhost:44332/HOME/LoggedinToAzureAD";

            string url = $"https://login.microsoftonline.com/{tenantId}/oauth2/authorize?client_id={clientId}&response_type=code&redirect_uri={redirect_uri}";

            /// Redirect to login page
            return Redirect(url);
        }

        public ActionResult LoggedinToAzureAD()
        {
            string authorizationcode = Convert.ToString(this.Request.QueryString["code"]);

            string tenantId = "8c3dad1d-xxxx-xxxx-xxxx-8263372eced6";
            string clientId = "5e9569bf-xxxx-xxxx-xxxx-fd33a25b9267";
            string clientSecret = "xxxxxxxxxxxxx/ffjB2Qc7K1uQhml2ZL96f73lT1yEA=";
            string appresourceId = "https://xxxxxxxxx.com/6b63059c-f1dc-4355-a338-6ac20346c217";
            string redirect_uri = "https://localhost:44332/HOME/LoggedinToAzureAD";

            //Build the URI
            var builder = new UriBuilder($"https://login.microsoftonline.com/{tenantId}/oauth2/token");

            //Add the question as part of the body
            NameValueCollection postBody = new NameValueCollection(){
                { "client_id", $"{clientId}" },
                { "client_secret", $"{clientSecret}" },
                { "grant_type", "authorization_code" },
                { "code", $"{authorizationcode}" },
                { "redirect_uri", $"{redirect_uri}"},
                { "resource", $"{appresourceId}" }
            };

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                var responseString = System.Text.Encoding.UTF8.GetString(client.UploadValues(builder.Uri, postBody));

                JObject json = JObject.Parse(responseString);

                string accessToken = Convert.ToString(json["access_token"]);
                StateClient stateClient = new StateClient(new MicrosoftAppCredentials("440c6efb-4d43-4131-be5b-80392707e165", "jkmbt39>:fpNROHJGC241!~"));
                BotData userData = stateClient.BotState.GetUserData(Convert.ToString(Session["channelId"]), Convert.ToString(Session["userId"]));
                userData.SetProperty<string>("AccessToken", accessToken);
                stateClient.BotState.SetUserData(Convert.ToString(Session["channelId"]), Convert.ToString(Session["userId"]), userData);
            }
            return View();
        }
    }
}