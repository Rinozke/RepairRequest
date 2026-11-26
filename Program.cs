using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
long adminChatId = long.Parse(Environment.GetEnvironmentVariable("ADMIN_CHAT_ID")); 

var client = new TelegramBotClient(token);
var service = new BotService();

var menu = new ReplyKeyboardMarkup(new[]
{
    new KeyboardButton[]{"Наши услуги", "Цены"},
    new KeyboardButton[] {"Оставить заявку", "Контакты"}
})
{
    ResizeKeyboard = true
};

var cancelMenu = new ReplyKeyboardMarkup(new[]
{
    new KeyboardButton[]{"Отмена"}
})
{
    ResizeKeyboard = true
};

var yesNoMenu = new ReplyKeyboardMarkup(new[]
{
     new KeyboardButton[]{"Да", "Нет"}
})
{
    ResizeKeyboard = true
};

client.OnMessage += async(Message, type) =>
{
     if (Message.Text is null)
    {
        return;
    }  
    string reply = service.HandleMessage(Message.Chat.Id, Message.Text);
    var stage = service.GetStage(Message.Chat.Id);
    var keyboard = stage switch
    {
        StageEnum.Menu => menu,
        StageEnum.Confirmation => yesNoMenu,
        _=>cancelMenu
    };
        
    await client.SendMessage(Message.Chat.Id, reply, replyMarkup: keyboard);
};

service.RequestCompleted += async repair =>
{   
     string text = $"Новая заявка № {repair.Id}\n" + 
     $"Имя: {repair.Name}\n" + 
     $"Телефон: {repair.PhoneNumber}\n" + 
     $"Проблема: {repair.Description}\n";
     await client.SendMessage(adminChatId, text);
};


System.Console.WriteLine("Бот запущен");
Console.ReadKey();