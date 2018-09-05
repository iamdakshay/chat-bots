using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace ChatBots.V4.PartyRegistrationBot
{
    public class PartyRegistrationBot : IBot
    {
        private readonly DialogSet dialogs;

        public PartyRegistrationBot()
        {
            dialogs = PartyRegistrationForm.Build();
        }

        public async Task OnTurn(ITurnContext context)
        {
            var state = context.GetConversationState<PartyRegistrationState>();
            var dialogCtx = dialogs.CreateContext(context, state);

            if (context.Activity.Type == ActivityTypes.Message)
            {
                switch (context.Activity.Type)
                {
                    case ActivityTypes.Message:

                        await dialogCtx.Continue();

                        if (!context.Responded)
                        {
                            string strMessage = $"Hey, Welcome to the registration for New Year's Eve party !";
                            await context.SendActivity(strMessage);
                            await dialogCtx.Begin(PartyRegistrationForm.StartDialog);
                        }
                        break;
                }
            }
        }
    }
}
