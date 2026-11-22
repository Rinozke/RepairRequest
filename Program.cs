using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
var client = new TelegramBotClient(token);
var service = new BotService();

client.OnMessage += async(Message, type) =>
{
     if (Message.Text is null)
    {
        return;
    }  
    string reply = service.HandleMessage(Message.Chat.Id, Message.Text);
    await client.SendMessage(Message.Chat.Id, reply);
};

System.Console.WriteLine("Бот запущен");
Console.ReadKey();