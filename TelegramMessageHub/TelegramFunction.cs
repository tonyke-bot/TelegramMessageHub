using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramMessageHub
{
    public static class TelegramFunction
    {
        static readonly TelegramBotClient botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("BOT_TOKEN"));

        class SendMessageResult : JsonSerializable
        {
            public bool Success { get; set; }
            public int MessageId { get; set; }
            public string ChatId { get; set; }
        }

        [FunctionName("SendMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "SendMessage/{chatId}/text")] HttpRequest req,
            string chatId,
            ILogger log)
        {
            var result = new SendMessageResult();
            var textToSend = await new StreamReader(req.Body).ReadToEndAsync();

            var messageDeliveryResult = await botClient.SendTextMessageAsync(chatId, textToSend, ParseMode.Markdown);
            result.Success = true;
            result.MessageId = messageDeliveryResult.MessageId;
            result.ChatId = chatId;

            return (ActionResult)new OkObjectResult(result.ToJsonString());
        }
    }
}
