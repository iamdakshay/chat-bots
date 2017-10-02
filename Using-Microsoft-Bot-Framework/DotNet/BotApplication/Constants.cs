using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BotApplication
{
    public static class Constants
    {
        public const string LUIS_EMPLOYEE_HELPER_APP_ID = "3c32158d-9ee6-48cb-8828-74415c3f27a9";
        public const string LUIS_SUBSCRIPTION_KEY = "4e8d83205a7849e8841383fe1b3c15c6";

        public enum SandwichOptions
        {
            BLT, BlackForestHam, BuffaloChicken, ChickenAndBaconRanchMelt, ColdCutCombo, MeatballMarinara,
            OvenRoastedChicken, RoastBeef, RotisserieStyleChicken, SpicyItalian, SteakAndCheese, SweetOnionTeriyaki, Tuna,
            TurkeyBreast, Veggie
        };

        public enum GenderOpts { Male, Female };

        public enum CuisinesOpts
        {
            [Terms("except", "but", "not", "no", "all", "everything")]
            Everything,
            Continental, Italian, Thai, Chinese, PanAsian, Labanese
        };

        public enum ComplementoryDrinkOpts {  Beer, Scotch, Mojito };

    }
}