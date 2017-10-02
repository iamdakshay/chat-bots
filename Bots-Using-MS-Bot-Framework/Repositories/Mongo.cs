using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Mongo
    {
        private IMongoDatabase _database;

        public Mongo()
        {
            string _connectionString = ConfigurationManager.AppSettings["MONGO_CONNECTIONSTRING"];
            MongoUrl mongoUrl = MongoUrl.Create(_connectionString);
            MongoClient _client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(mongoUrl.Server.Host, mongoUrl.Server.Port)
            });
            _database = _client.GetDatabase(MongoUrl.Create(_connectionString).DatabaseName, null);
        }

        public void Insert<T>(string collectionName, T document)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        public T Get<T>(string collectionName, string property, string value)
        {
            IMongoCollection<T> collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq<string>(property, value);
            return collection.Find(filter).SingleOrDefault();
        }
    }
}
