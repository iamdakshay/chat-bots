using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using static BotApplication.Constants;

namespace BotApplication.Forms
{
    [Serializable]
    public class PartyRegistrationForm
    {
        [Prompt("May I know your good name?")]
        public string Name; // type: String

        [Optional]
        [Template(TemplateUsage.EnumSelectOne, "Please select gender, if you want else you can skip. {||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public GenderOpts? Gender; // type: Enumeration

        [Prompt("What will be your arrival time?")]
        public DateTime ArrivalTime; // type: DateTime

        [Numeric(1, 6)]
        [Prompt("How many guests are accompanying you?<br>If more than 3, you will get complementory drink ! :)")]
        public Int16 TotalAttendees; // type: Integral

        [Prompt("Which type of cuisines you would like to have? {||}")]
        public List<CuisinesOpts> CuisinesPreferences; // type: List of enumerations

        [Template(TemplateUsage.EnumSelectOne, "Which complementory drink you would like to have? {||}", ChoiceStyle = ChoiceStyleOptions.Carousel)]
        public ComplementoryDrinkOpts? ComplementoryDrink; // type: Enumeration

        public static IForm<PartyRegistrationForm> BuildForm()
        {
            return new FormBuilder<PartyRegistrationForm>()
                    .Message("Hey, Welcome to the registartion for New Year's Eve party !")

                    .Field(nameof(Name))
                    .Field(nameof(Gender))
                    .Field(nameof(ArrivalTime))
                    .Field(nameof(TotalAttendees))
                    .Field(nameof(CuisinesPreferences))
                    .Field(new FieldReflector<PartyRegistrationForm>(nameof(ComplementoryDrink))
                        .SetType(null)
                        .SetActive(state => state.TotalAttendees > 3)
                        .SetDefine(async (state, field) =>
                        {
                            field
                            .AddDescription(ComplementoryDrinkOpts.Beer, Convert.ToString(ComplementoryDrinkOpts.Beer),
                            "<<image path>>")
                            .AddTerms(ComplementoryDrinkOpts.Beer, Convert.ToString(ComplementoryDrinkOpts.Beer))

                            .AddDescription(ComplementoryDrinkOpts.Scotch, Convert.ToString(ComplementoryDrinkOpts.Scotch),
                            "<<image path>>")
                            .AddTerms(ComplementoryDrinkOpts.Scotch, Convert.ToString(ComplementoryDrinkOpts.Scotch))

                            .AddDescription(ComplementoryDrinkOpts.Mojito, Convert.ToString(ComplementoryDrinkOpts.Mojito),
                            "<<image path>>")
                            .AddTerms(ComplementoryDrinkOpts.Mojito, Convert.ToString(ComplementoryDrinkOpts.Mojito));

                            return true;
                        }))
                    .Confirm(async (state) =>
                    {
                        return new PromptAttribute("Hi {Name}, Please review your selection. No. of guests: {TotalAttendees}, Cuisines: {CuisinesPreferences}. Do you want to continue? {||}");
                    })
                    .Build();
        }
    }
}