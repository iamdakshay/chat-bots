using ChatBots.V3.Common.LinkedIn.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatBots.V3.Common.LinkedIn
{
    public interface ILinkedInService
    {
        Task<LinkedInProfile> GetProfile(string AuthorizationToken, List<string> Fields = null);
    }
}
