using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBots.V3.Common
{
    public class Token
    {
        public ObjectId _id;
        public string UserName;
        public string ContextToken;

        public Token(string userName, string contextToken)
        {
            UserName = userName;
            ContextToken = contextToken;
        }
    }
}