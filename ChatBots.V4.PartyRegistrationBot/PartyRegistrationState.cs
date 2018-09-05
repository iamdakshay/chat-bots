using System;
using System.Collections.Generic;

namespace ChatBots.V4.PartyRegistrationBot
{
    /// <summary>
    /// Class for storing conversation state. 
    /// </summary>
    public class PartyRegistrationState : Dictionary<string, object>
    {
        private const string NameKey = "name";
        private const string GenderKey = "gender";
        private const string ArrivalTimeKey = "arrivaltime";
        private const string TotalAttendeesKey = "totalattendees";
        private const string CuisinesPreferencesKey = "cuisinespreferences";
        private const string ComplementoryDrinkKey = "complementorydrink";

        public PartyRegistrationState()
        {
            this[NameKey] = null;
            this[GenderKey] = 0;
            this[ArrivalTimeKey] = null;
            this[TotalAttendeesKey] = null;
            this[CuisinesPreferencesKey] = null;
            this[ComplementoryDrinkKey] = null;
        }

        public string Name
        {
            get => (string)this[NameKey];
            set => this[NameKey] = value;
        }

        public string Gender
        {
            get => (string)this[GenderKey];
            set => this[GenderKey] = value;
        }

        public DateTime ArrivalTime
        {
            get => (DateTime)this[ArrivalTimeKey];
            set => this[ArrivalTimeKey] = value;
        }

        public int TotalAttendees
        {
            get => (int)this[TotalAttendeesKey];
            set => this[TotalAttendeesKey] = value;
        }

        public List<string> CuisinesPreferences
        {
            get => (List<string>)this[CuisinesPreferencesKey];
            set => this[CuisinesPreferencesKey] = value;
        }

        public string ComplementoryDrink
        {
            get => (string)this[ComplementoryDrinkKey];
            set => this[ComplementoryDrinkKey] = value;
        }
    }
}
