using ChatBots.V3.Common.LinkedIn;
using ChatBots.V3.Common.LinkedIn.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace BotApplication.Dialogs
{
    [Serializable]
    public class LinkedInDialog : IDialog<object>
    {
        private static readonly string ConnectionName = ConfigurationManager.AppSettings["ConnectionName"];

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            ShowOptions(context);
        }

        private readonly List<string> rootOptions = new List<string>()
        {
            "Show My Profile",
            "Show Professional Information",
            "Share on LinkedIn",
            "Sign Out"
        };

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(
                context,// Current Dialog Context                
                OnOptionSelected,// Callback after option selection                
                rootOptions,// Available Options                
                "What you would like to do today?",// Prompt text                 
                "Not a valid option",// Invalid input message                
                3,// How many times retry                
                PromptStyle.Auto,// Display Style
                null);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case "Show My Profile":
                        context.Call(CreateGetTokenDialog(), ShowBasicProfile);
                        break;
                    case "Show Professional Information":
                        context.Call(CreateGetTokenDialog(), ShowProfessionalProfile);
                        break;
                    case "Share on LinkedIn":

                        break;
                    case "Sign Out":
                        await Signout(context);
                        break;
                }
            }
            catch (TooManyAttemptsException exception)
            {
                await context.PostAsync("Ooops! Too many attemps. You can try again!");

            }
        }

        private GetTokenDialog CreateGetTokenDialog()
        {
            return new GetTokenDialog(ConnectionName, $"Please sign in to LinkedIn to proceed.", "Sign In", 2, "Hmm. Something went wrong, let's try again.");
        }

        private async Task ShowBasicProfile(IDialogContext context, IAwaitable<GetTokenResponse> tokenResponse)
        {
            try
            {
                var token = await tokenResponse;

                LinkedInProfile linkedinProfile = await (new LinkedInService().GetProfile(token.Token));

                IMessageActivity basicProfileMessage = context.MakeMessage();
                basicProfileMessage.Attachments = new List<Attachment>();

                ThumbnailCard basicProfileCard = new ThumbnailCard
                {
                    Title = linkedinProfile.FirstName + " " + linkedinProfile.LastName,
                    Images = new List<CardImage>()
                    {
                    new CardImage(linkedinProfile.PictureUrl)
                    },
                    Subtitle = $"{linkedinProfile.Num_Connections}{(linkedinProfile.Num_Connections_Capped ? "+" : "")} connections",
                    Tap = new CardAction("openUrl", "View Public Profile", null, linkedinProfile.PublicProfileUrl)
                };

                basicProfileMessage.Attachments.Add(basicProfileCard.ToAttachment());
                await context.PostAsync(basicProfileMessage);
            }
            catch (Exception exception)
            {
                await context.PostAsync(exception.Message);
            }
            ShowOptions(context);
        }

        private async Task ShowProfessionalProfile(IDialogContext context, IAwaitable<GetTokenResponse> tokenResponse)
        {
            try
            {
                var token = await tokenResponse;

                List<string> fields = new List<string>() {
                    LinkedInConstants.ProfileFields.POSITIONS,
                    LinkedInConstants.ProfileFields.SPECIALTIES
                    };

                LinkedInProfile linkedinProfile = await (new LinkedInService().GetProfile(token.Token, fields));

                IMessageActivity professionalProfileMessage = context.MakeMessage();
                professionalProfileMessage.Attachments = new List<Attachment>();

                ReceiptCard professionalProfileCard = new ReceiptCard
                {
                    Title = linkedinProfile.FirstName + " " + linkedinProfile.LastName
                };

                if (null != linkedinProfile.Positions && 0 < linkedinProfile.Positions.Total)
                {
                    professionalProfileCard.Items = new List<ReceiptItem>();

                    linkedinProfile.Positions.Values.ForEach(x =>
                    {
                        professionalProfileCard.Items.Add(new ReceiptItem()
                        {
                            Title = x.Title + " @ " + x.Company.Name,
                            Subtitle = x.Summary
                        });
                    });
                }

                professionalProfileMessage.Attachments.Add(professionalProfileCard.ToAttachment());

                await context.PostAsync(professionalProfileMessage);
            }
            catch (Exception exception)
            {
                await context.PostAsync(exception.Message);
            }
            ShowOptions(context);
        }

        public static async Task Signout(IDialogContext context)
        {
            await context.SignOutUserAsync(ConnectionName);
            await context.PostAsync($"You have been signed out.");
        }

    }
}