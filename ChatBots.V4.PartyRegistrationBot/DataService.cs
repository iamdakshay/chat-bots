using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using System.Collections.Generic;

namespace ChatBots.V4.PartyRegistrationBot
{
    public static class DataService
    {
        public static ChoicePromptOptions GetGenderOptions()
        {
            var choices = new List<Choice>
            {
                new Choice { Value = "Male", Synonyms = new List<string> { "male", "man" } },
                new Choice { Value = "Female", Synonyms = new List<string> { "female", "woman", "lady" } },
                new Choice { Value = "Skip", Synonyms = new List<string> { "leave it", "next" } },
            };

            return new ChoicePromptOptions()
            {
                Choices = choices,
                RetryPromptString = "Sorry, that isn't on the list. Please pick again."
            };
        }

        public static ChoicePromptOptions GetCuisinsOptions()
        {
            var choices = new List<Choice>
            {
                new Choice {
                    Value = "Continental",
                    Synonyms = new List<string>
                    {
                        "continental"
                    }
                },
                new Choice
                {
                    Value = "Italian",
                    Synonyms = new List<string>
                    {
                        "italian"
                    }
                },
                new Choice
                {
                    Value = "PanAsian",
                    Synonyms = new List<string>
                    {
                        "panasian"
                    }
                },
            };

            return new ChoicePromptOptions()
            {
                Choices = choices,
                RetryPromptString = "Sorry, that isn't on the list. Please pick again."
            };
        }

        public static ChoicePromptOptions GetComplementoryDrinkChoices()
        {
            var choices = new List<Choice>
            {
                new Choice {
                    Value = "Beer",
                    Synonyms = new List<string>
                    {
                        "beer"
                    }
                },
                new Choice
                {
                    Value = "Scotch",
                    Synonyms = new List<string>
                    {
                        "scotch"
                    }
                },
                new Choice
                {
                    Value = "Mojito",
                    Synonyms = new List<string>
                    {
                        "mojito"
                    }
                },
            };

            return new ChoicePromptOptions()
            {
                Choices = choices,
                RetryPromptString = "Sorry, that isn't on the list. Please pick again."
            };
        }
    }
}
