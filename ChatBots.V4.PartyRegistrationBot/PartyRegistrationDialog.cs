using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotPrompts = Microsoft.Bot.Builder.Prompts;

namespace ChatBots.V4.PartyRegistrationBot
{
    public static class PartyRegistrationForm
    {
        public const string StartDialog = "StartDialog";

        private const string AskNamePrompt = "AskName";
        private const string AskGenderPrompt = "AskGender";
        private const string AskArrivalTimePrompt = "AskArrivalTime";
        private const string AskTotalAttendeesPrompt = "AskTotalAttendees";
        private const string AskCuisinesPreferencesPrompt = "AskCuisinesPreferences";
        private const string AskComplementoryDrinkPrompt = "AskComplementoryDrink";

        private static async Task AskNameStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            await dialogContext.Prompt(AskNamePrompt, "May I know your good name?");
        }

        private static async Task NameValidator(ITurnContext context, BotPrompts.TextResult result)
        {
            if (result.Value.Length <= 6 || (result.Value.Length > 25))
            {
                result.Status = BotPrompts.PromptStatus.TooSmall;
                await context.SendActivity("Your name should be at least 6 and at most 25 characters long.");
            }
        }

        private static async Task AskGenderStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();
            state.Name = (result as BotPrompts.TextResult).Value;

            await dialogContext.Prompt(AskGenderPrompt, "Please select gender, if you want else you can skip.", DataService.GetGenderOptions());
        }

        private static async Task AskArrivalTimeStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();
            state.Gender = (result as BotPrompts.ChoiceResult).Value.Value;

            await dialogContext.Prompt(AskArrivalTimePrompt, "What will be your arrival time?");
        }

        private static async Task ArrivalTimeValidator(ITurnContext context, BotPrompts.DateTimeResult result)
        {
            if (result.Resolution.Count == 0)
            {
                await context.SendActivity("Sorry, I could not understand your preffered time.");
                result.Status = BotPrompts.PromptStatus.NotRecognized;
            }

            var now = DateTime.Now;
            DateTime time = default(DateTime);
            var resolution = result.Resolution.FirstOrDefault(
                res => DateTime.TryParse(res.Value, out time) && time > now);

            if (resolution != null)
            {
                result.Resolution.Clear();
                result.Resolution.Add(resolution);
            }
            else
            {
                await context.SendActivity("Please time after 6 pm");
                result.Status = BotPrompts.PromptStatus.OutOfRange;
            }
        }

        private static async Task AskTotalAttendeesStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();
            state.ArrivalTime = DateTime.Parse((result as BotPrompts.DateTimeResult).Resolution.FirstOrDefault().Value);

            await dialogContext.Prompt(AskTotalAttendeesPrompt, $"How many guests are accompanying you?{Environment.NewLine}If more than 3, you will get complementory drink ! :)");
        }

        private static async Task TotalAttendeesValidator(ITurnContext context, BotPrompts.NumberResult<int> result)
        {
            if (result.Value < 2 || result.Value > 9)
            {
                result.Status = BotPrompts.PromptStatus.OutOfRange;
                await context.SendActivity("You can book entries for minimum 2 and maximum 9 people");
            }
        }

        private static async Task AskCuisinesPreferencesStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();
            state.TotalAttendees = (result as BotPrompts.NumberResult<int>).Value;

            await dialogContext.Prompt(AskCuisinesPreferencesPrompt, "Which type of cuisines you would like to have?", DataService.GetCuisinsOptions());
        }

        private static async Task AskComplementoryDrinkStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();
            if (null == state.CuisinesPreferences)
            {
                state.CuisinesPreferences = new List<string>();
            }
            state.CuisinesPreferences.Add((result as BotPrompts.ChoiceResult).Value.Value);

            if (3 < state.TotalAttendees)
            {
                await dialogContext.Prompt(AskComplementoryDrinkPrompt, "Which complementory drink you would like to have?", DataService.GetComplementoryDrinkChoices());
            }
            else
            {
                await next.Invoke();
            }
        }

        private static async Task FinalStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<PartyRegistrationState>();

            if (null != result)
            {
                state.ComplementoryDrink = (result as BotPrompts.TextResult).Value;
            }

            await dialogContext.Context.SendActivity($"Okay {state.Name}, I will book a table for {state.TotalAttendees} people with preferred food as {string.Join(",", state.CuisinesPreferences)}.");

            if (3 < state.TotalAttendees)
            {
                await dialogContext.Context.SendActivity($"And you will also get {state.ComplementoryDrink} complementory. Njoy!");
            }

            await dialogContext.Context.SendActivity($"Thank you for your interest.");

            await dialogContext.End();
        }

        public static DialogSet Build()
        {
            var dialogs = new DialogSet();

            dialogs.Add(AskNamePrompt, new TextPrompt(NameValidator));
            dialogs.Add(AskGenderPrompt, new ChoicePrompt(Culture.English)
            {
                Style = BotPrompts.ListStyle.Auto
            });

            dialogs.Add(AskArrivalTimePrompt, new DateTimePrompt(Culture.English, ArrivalTimeValidator));
            dialogs.Add(AskTotalAttendeesPrompt, new NumberPrompt<int>(Culture.English, TotalAttendeesValidator));
            dialogs.Add(AskCuisinesPreferencesPrompt, new ChoicePrompt(Culture.English)
            {
                Style = BotPrompts.ListStyle.Auto
            });

            dialogs.Add(AskComplementoryDrinkPrompt, new ChoicePrompt(Culture.English)
            {
                Style = BotPrompts.ListStyle.Auto
            });

            dialogs.Add(StartDialog,
                new WaterfallStep[]
                {
                    AskNameStep,
                    AskGenderStep,
                    AskArrivalTimeStep,
                    AskTotalAttendeesStep,
                    AskCuisinesPreferencesStep,
                    AskComplementoryDrinkStep,
                    FinalStep
    });

            return dialogs;
        }
    }
}
