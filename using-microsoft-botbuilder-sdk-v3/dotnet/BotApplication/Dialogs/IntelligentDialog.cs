using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;

namespace BotApplication.Dialogs
{
    [Serializable]
    [LuisModel(Constants.LUIS_EMPLOYEE_HELPER_APP_ID, Constants.LUIS_SUBSCRIPTION_KEY)]
    public class IntelligentDialog : LuisDialog<object>
    {
        private string userName;
        private DateTime msgReceivedDate;

        public IntelligentDialog(Activity activity)
        {
            userName = activity.From.Name;
            msgReceivedDate = activity.Timestamp.Value.DateTime;
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
            string response = string.Empty;

            if (this.msgReceivedDate.ToString("tt") == "AM")
            {
                response = $"Good morning, {userName}.. :)";
            }
            else
            {
                response = $"Hey {userName}.. :)";
            }

            await context.PostAsync(response);
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

            string name = string.Empty;

            if (luisResult.TryFindEntity("Person.Name", out employeeName))
            {
                name = employeeName.Entity;
            }

            await context.PostAsync($"You have searched for {name}");
            context.Wait(this.MessageReceived);

        }
    }
}