using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChatBots.V3.Common.LinkedIn.Models
{
    
    public class Positions
    {
        [JsonProperty("_total")]
        public int Total { get; set;}

        [JsonProperty("values")]
        public List<Position> Values { get; set;}
    }
    
    public class Position
    {
        [JsonProperty("company")]
        public Company Company { get; set;}

        [JsonProperty("title")]
        public string Title { get; set;}

        [JsonProperty("isCurrent")]
        public bool IsCurrent { get; set;}

        [JsonProperty("summary")]
        public string Summary { get; set;}
    }
    
    public class Company
    {
        [JsonProperty("industry")]
        public string Industry;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("size")]
        public string Size;

        [JsonProperty("type")]
        public string Type;
    }


    
    public class LinkedInProfile
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set;}

        [JsonProperty("lastName")]
        public string LastName { get; set;}

        [JsonProperty("headline")]
        public string Headline { get; set;}

        [JsonProperty("location")]
        public string Location { get; set;}

        [JsonProperty("positions")]
        public Positions Positions { get; set;}

        [JsonProperty("pictureUrl")]
        public string PictureUrl { get; set;}

        [JsonProperty("public-profile-url")]
        public bool PublicProfileUrl { get; set;}

        [JsonProperty("site-standard-profile-request")]
        public string SiteStandardProfileRequest { get; set;}

        [JsonProperty("specialties")]
        public string Specialties { get; set;}

        [JsonProperty("industry")]
        public string Industry { get; set;}

        [JsonProperty("summary")]
        public string Summary { get; set;}

        [JsonProperty("numConnections")]
        public string Num_Connections { get; set;}

        [JsonProperty("numConnectionsCapped")]
        public bool Num_Connections_Capped { get; set;}
    }
}
