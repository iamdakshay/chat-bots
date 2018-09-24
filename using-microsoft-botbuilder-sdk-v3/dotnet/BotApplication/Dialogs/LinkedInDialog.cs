using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var activity = await result as Activity;
            string message = activity.Text;

            if (message.Equals("hello"))
            {
                // Display information about the logged in user
                context.Call(CreateGetTokenDialog(), Data);
            }
            else if (message.ToLowerInvariant().Equals("signout"))
            {
                // Sign the user out from AAD
                await Signout(context);
            }
            else
            {
                await context.PostAsync("You can type 'recents', 'send <recipient_email>', or 'me' to list things from AAD v1.");
                context.Wait(MessageReceivedAsync);
            }
        }


        private GetTokenDialog CreateGetTokenDialog()
        {
            return new GetTokenDialog(
                ConnectionName,
                $"Please sign in to {ConnectionName} to proceed.",
                "Sign In",
                2,
                "Hmm. Something went wrong, let's try again.");
        }

        private async Task Data(IDialogContext context, IAwaitable<GetTokenResponse> tokenResponse)
        {
            var token = await tokenResponse;


            await context.PostAsync(token.Token);

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.linkedin.com"),
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            string link = $"/v1/people/~?format=json";

            try
            {
                var data = await client.GetAsync(link);
                string dataString = await data.Content.ReadAsStringAsync();

                await context.PostAsync(dataString);
            }
            catch (Exception exception)
            {
                await context.PostAsync(exception.Message);
            }

        }

        public static async Task Signout(IDialogContext context)
        {
            await context.SignOutUserAsync(ConnectionName);
            await context.PostAsync($"You have been signed out.");
        }

    }
}