using Common.Contracts;
using Microsoft.SharePoint.Client;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SharePoint
    {
        private string _userName;
        private string _siteUri;

        public SharePoint(string userName)
        {
            _userName = userName;
            _siteUri = ConfigurationManager.AppSettings["SHAREPOINT_SITE_URI"];
        }

        public string FindUsersByName(string searchTermName)
        {
            string users = string.Empty;

            Token token = new Mongo().Get<Token>("AccessTokens", "UserName", this._userName);

            using (ClientContext context = TokenHelper.GetClientContextWithContextToken(_siteUri, token.AccessToken, "localhost:44331"))
            {
                CamlQuery query = new CamlQuery();
                query.ViewXml = "";
                ListItemCollection peopleDetails = context.Web.Lists.GetByTitle("PeopleDetails").GetItems(query);
                context.Load(peopleDetails);
                context.ExecuteQuery();

                users = string.Join("<br>", peopleDetails.Select(x => x["Title"] + "(" + x["ContactNumber"] + "),"));
            }
            return users;
        }
    }
}
