using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;
using System.Text;
using Common;

namespace BotApplication.Dialogs
{
    [Serializable]
    [LuisModel(Constants.LUIS_EMPLOYEE_HELPER_APP_ID, Constants.LUIS_SUBSCRIPTION_KEY)]
    public class SharePointDialog : LuisDialog<object>
    {
        private string userName;
        private DateTime msgReceivedDate;

        public SharePointDialog(Activity activity)
        {
            userName = activity.From.Name;
            msgReceivedDate = activity.Timestamp ?? DateTime.Now;
        }

        [LuisIntent("")]
        [LuisIntent("none")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult luisResult)
        {

            await context.PostAsync($"Sorry {this.userName}, I did not understand you.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greet.Welcome")]
        public async Task GreetWelcome(IDialogContext context, LuisResult luisResult)
        {
            StringBuilder response = new StringBuilder();

            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response.Append($"Good morning, {userName}.. :)");
            }
            else
            {
                response.Append($"Hey {userName}.. :)");
            }

            string sharepointLoginUrl = ConfigurationManager.AppSettings["SHAREPOINT_LOGIN_URI"];
            response.Append($"<br>Click <a href='{sharepointLoginUrl}?userName={this.userName}' >here</a> to login");

            await context.PostAsync(response.ToString());
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greet.Farewell")]
        public async Task GreetFarewell(IDialogContext context, LuisResult luisResult)
        {
            string response = string.Empty;

            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good bye, {userName}.. Have a nice day.. :)";
            }
            else
            {
                response = $"b'bye {userName}, Take care..";
            }

            await context.PostAsync(response);
            context.Wait(this.MessageReceived);

        }

        [LuisIntent("Search.People")]
        public async Task SearchPeople(IDialogContext context, LuisResult luisResult)
        {
            EntityRecommendation employeeName;

            string searchTerm_PersonName = string.Empty;

            if (luisResult.TryFindEntity("Person.Name", out employeeName))
            {
                searchTerm_PersonName = employeeName.Entity;
            }

            if (string.IsNullOrWhiteSpace(searchTerm_PersonName))
            {
                await context.PostAsync($"Unable to get search term.");
            }
            else
            {
                await context.PostAsync(new SharePoint(this.userName).FindUsersByName(searchTerm_PersonName));
            }
            context.Wait(this.MessageReceived);

        }
    }
}