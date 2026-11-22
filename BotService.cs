using System.ComponentModel;
using System.Net;
using Telegram.Bot.Types;

class BotService
{
    public Dictionary<long, StageEnum> _stages = new Dictionary<long, StageEnum>();
    public Dictionary<long, RepairRequest> notFullInfo = new Dictionary<long, RepairRequest>();

    private int count {get; set;} = File.Exists("repair.txt") ? File.ReadAllLines("repair.txt").Length : 0;

    public string HandleMessage(long chatId, string text)
    {
        RepairRequest repair = new();
        if (text == "Отмена")
        {
            _stages[chatId] = StageEnum.Menu;
            notFullInfo.Remove(chatId);

            return "Возвращаю вас в главное меню";
        }

        if (!_stages.ContainsKey(chatId))
        {
            _stages[chatId] = StageEnum.Menu;
        }


        switch(_stages[chatId])
        {
            case StageEnum.Menu :
            notFullInfo[chatId] = new RepairRequest {ChatId = chatId};
            _stages[chatId] = StageEnum.Name;
            return "Здравствуйте!\nКак вас зовут?";

            case StageEnum.Name :
            notFullInfo[chatId].Name = text;
            _stages[chatId] = StageEnum.PhoneNumber;
            return "Введите ваш номер телефона";

            case StageEnum.PhoneNumber :
            if (repair.IsValidPhoneNumber(text))
            {
                 notFullInfo[chatId].PhoneNumber = text;
                _stages[chatId] = StageEnum.Description;
                return "Введите описание пробоемы";
            }
            else
            {
                _stages[chatId] = StageEnum.PhoneNumber;
                return "Повторите еще раз!";
            }

            case StageEnum.Description :
            repair = notFullInfo[chatId];
            repair.Description = text;
            repair.orderStatus = OrderStatusEnum.Wait;
            repair.CreateAt = DateTime.Now;
            repair.Id = ++count;
            _stages[chatId] = StageEnum.Menu;
            SaveRequest(notFullInfo[chatId]);
            notFullInfo.Remove(chatId);
            return "Заявка принята, менеджер свяжется с вами!";
            

            default : 
            return "Что-то пошло не так, напишите /start.";
        }
    }

    public void SaveRequest(RepairRequest repair)
    {
        File.AppendAllText("repair.txt", repair + Environment.NewLine);
    }
}   