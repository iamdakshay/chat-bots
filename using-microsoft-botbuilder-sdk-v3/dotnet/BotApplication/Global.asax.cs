using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System.Web.Http;

namespace BotApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InMemoryDataStore store = new InMemoryDataStore();

            Conversation.UpdateContainer(
                       builder =>
                       {
                           builder.Register(c => store)
                                     .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                                     .AsSelf()
                                     .SingleInstance();

                           builder.Register(c => new CachingBotDataStore(store,
                                      CachingBotDataStoreConsistencyPolicy
                                      .ETagBasedConsistency))
                                      .As<IBotDataStore<BotData>>()
                                      .AsSelf()
                                      .InstancePerLifetimeScope();


                       });
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
