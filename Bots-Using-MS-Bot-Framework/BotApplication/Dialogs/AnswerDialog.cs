
namespace BotApplication.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json;
    using System.Net;
    using System.Configuration;
    using System.Globalization;
    using System.Collections.Generic;
    public class QnAMakerResult
    {
        [JsonProperty(PropertyName = "answers")]
        public List<Result> Answers { get; set; }
    }

    public class Result
    {
        /// <summary>
        /// The top answer found in the QnA Service.
        /// </summary>
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        [JsonProperty(PropertyName = "questions")]
        public List<string> Questions { get; set; }

        /// <summary>
        /// The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }

    [Serializable]
    public class AnswerDialog : IDialog<object>
    {
public Task StartAsync(IDialogContext context)
{
    context.Wait(QuestionReceivedAsync);

    return Task.CompletedTask;
}

private async Task QuestionReceivedAsync(IDialogContext context, IAwaitable<object> result)
{
    var activity = await result as Activity;

                await context.PostAsync(GetAnswer(activity.Text));
}

private string GetAnswer(string query)
{
    string responseString = string.Empty;

    var knowledgebaseId = Convert.ToString(ConfigurationManager.AppSettings["KNOWLEDGE_BASE_ID"], CultureInfo.InvariantCulture);

    //Build the URI
    var builder = new UriBuilder(string.Format(Convert.ToString(ConfigurationManager.AppSettings["QNA_SERVICE_URL"], CultureInfo.InvariantCulture), knowledgebaseId));

    //Add the question as part of the body
    var postBody = string.Format("{{\"question\": \"{0}\"}}", query);

    //Send the POST request
    using (WebClient client = new WebClient())
    {
        //Set the encoding to UTF8
        client.Encoding = System.Text.Encoding.UTF8;

        //Add the subscription key header
        var qnamakerSubscriptionKey = Convert.ToString(ConfigurationManager.AppSettings["SUBSCRIPTION_KEY"], CultureInfo.InvariantCulture);
        client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
        client.Headers.Add("Content-Type", "application/json");
        responseString = client.UploadString(builder.Uri, postBody);
    }
    QnAMakerResult result = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
    return result.Answers[0].Answer;
}
    }
}