using ChatBots.V3.Common.LinkedIn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatBots.V3.Common.LinkedIn
{
    public class LinkedInService : ILinkedInService
    {
        public async Task<LinkedInProfile> GetProfile(string AuthorizationToken, List<string> Fields = null)
        {
            try
            {
                if (null == Fields || 0 == Fields.Count)
                {
                    Fields = new List<string>() {
                        LinkedInConstants.ProfileFields.FIRST_NAME,
                        LinkedInConstants.ProfileFields.LAST_NAME,
                        LinkedInConstants.ProfileFields.PICTURE_URL,
                        LinkedInConstants.ProfileFields.NUM_CONNECTIONS,
                        LinkedInConstants.ProfileFields.NUM_CONNECTIONS_CAPPED
                        };
                }
                string basicProfileFields = $":({string.Join(",", Fields.Select(x => x))})";

                string link = $"/v1/people/~{basicProfileFields}?format=json";

                HttpResponseMessage data = null;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(LinkedInConstants.BASE_API); client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    data = await client.GetAsync(link);
                }

                string dataString = await data.Content.ReadAsStringAsync();
                LinkedInProfile linkedInProfile = JsonConvert.DeserializeObject<LinkedInProfile>(dataString);

                return linkedInProfile;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
