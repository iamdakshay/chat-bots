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
        public static class Emoticons
        {
            public const string Happy = "\U0001F642";
            public const string Sad = "\U0001F61F";
            public const string AllTheBest = "Poke";
            public const string Lock = "\U0001F512";
            public const string ThubmsUp = "\U0001F44D";
            public const string Poke = "\U0001F44D";
            public const string Yo = "\U0001F918";
            public const string MoneyBag = "\U0001F4B0";
        }


        public static class Messages
        {
            public const string GREETING = "Welcome {0}";
            public const string GREETING_FAREWELL = "B'bye {0}";
            public const string GREETING_INFO_ABOUT_ME = "I am here to assist you with space bookings for your single session event. *At any time please type <u>Start Over</u> to start the process again.*";
            public const string BOOKING_CONFIRMATION_INFO_PAYMENT = "Your booking will be confirmed after payment process. " + Constants.Emoticons.MoneyBag;
            public const string BOOKING_CONFIRMATION_INFO_CATERING_LINK = "For adding catering services and payment process, please click <a href='{0}' >here</a>";
            public const string CancelledBookSpace = "Oh! you have canceled space booking" + Constants.Emoticons.Sad;
            public const string Sad = "\U0001F61F";
            public const string AllTheBest = "Poke";
            public const string Lock = "\U0001F512";
            public const string ThubmsUp = "\U0001F44D";
            public const string Poke = "\U0001F44D";
            public const string Yo = "\U0001F918";
        }



        //public const string LUIS_EMPLOYEE_HELPER_APP_ID = "3c32158d-9ee6-48cb-8828-74415c3f27a9";
        //public const string LUIS_SUBSCRIPTION_KEY = "4e8d83205a7849e8841383fe1b3c15c6";
        public const string LUIS_EMPLOYEE_HELPER_APP_ID = "ab9e37fc-5c06-4ff5-bbad-55ca6e9d9f05";
        public const string LUIS_SUBSCRIPTION_KEY = "b810c4da98ad4984a32464828a132e7e";

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

        public enum ComplementoryDrinkOpts { Beer, Scotch, Mojito };

    }
}