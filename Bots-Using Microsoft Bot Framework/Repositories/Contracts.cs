using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public class Token
    {
        public ObjectId _id;
        public string UserName;
        public string AccessToken;

        public Token(string userName, string accessToken)
        {
            UserName = userName;
            AccessToken = accessToken;
        }
    }
}