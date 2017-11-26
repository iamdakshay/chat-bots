using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Configuration;
using System.Text;
using Common;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace BotApplication.Dialogs
{
    [Serializable]
    [LuisModel(Constants.LUIS_EMPLOYEE_HELPER_APP_ID, Constants.LUIS_SUBSCRIPTION_KEY)]
    public class AzureDialog : LuisDialog<object>
    {
        private string _channel;
        private string _user;
        public string _resourcesPath;

        public AzureDialog(Activity activity, string resourcesPath)
        {
            _channel = activity.ChannelId;
            _user = activity.From.Id;
            _resourcesPath = resourcesPath;
        }

        [LuisIntent("")]
        [LuisIntent("none")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult luisResult)
        {
            await context.PostAsync($"Sorry, I did not understand you. \U0001F44D");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greet.Welcome")]
        public async Task GreetWelcome(IDialogContext context, LuisResult luisResult)
        {
            StateClient stateClient = new StateClient(new MicrosoftAppCredentials("xxxxxxxxxxx-4d43-4131-be5b-xxxxxxxxxxxx", "jkmbt39>:xxxxxxxxxxxx!~"));
            //BotData userData = stateClient.BotState.GetUserData(context.Activity.ChannelId, context.Activity.From.Id);
            BotData userData = stateClient.BotState.GetUserData("skype", context.Activity.From.Id);
            string accesstoken = userData.GetProperty<string>("AccessToken");

            if (string.IsNullOrEmpty(accesstoken))
            {
                string loginUrl = $"https://localhost:44332/home/LoginWithAzure?channelId={this._channel}&userId={this._user}";
                var message = context.MakeMessage();

                ThumbnailCard thumbnailCard = new ThumbnailCard();
                thumbnailCard.Title = $"Good morning dude..";
                thumbnailCard.Subtitle = "Sign in with Azure \U0001F511";
                thumbnailCard.Buttons = new List<CardAction>();
                thumbnailCard.Buttons.Add(new CardAction()
                {
                    Value = $"{loginUrl}",
                    Type = "signin",
                    Title = "Sign In"
                });
                thumbnailCard.Images = new List<CardImage>();
                thumbnailCard.Images.Add(new CardImage($"{this._resourcesPath}/Images/ChatBot.png"));

                message.Attachments = new List<Attachment>();
                message.Attachments.Add(thumbnailCard.ToAttachment());
                await context.PostAsync(message);
            }
            else
            {
                ClaimsPrincipal jwtToken = await Helper.Validate(accesstoken);
                await context.PostAsync($"You are logged in as {jwtToken.Identity.Name}");
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greet.Farewell")]
        public async Task GreetFarewell(IDialogContext context, LuisResult luisResult)
        {
            StateClient stateClient = new StateClient(new MicrosoftAppCredentials("xxxxxxxxxx-4d43-4131-be5b-xxxxxxxxxxx", "jkmbt39>:xxxxxxxxxxxx!~"));
            //BotData userData = stateClient.BotState.GetUserData(context.Activity.ChannelId, context.Activity.From.Id);
            BotData userData = stateClient.BotState.GetUserData("skype", context.Activity.From.Id);
            userData.RemoveProperty("AccessToken");
            stateClient.BotState.SetUserData(context.Activity.ChannelId, context.Activity.From.Id, userData);
            await context.PostAsync("b'bye \U0001F44B Take care");
            context.Wait(this.MessageReceived);
        }
    }
}