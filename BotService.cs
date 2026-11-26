using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;

class BotService
{
    public Dictionary<long, StageEnum> _stages = new Dictionary<long, StageEnum>();
    public Dictionary<long, RepairRequest> notFullInfo = new Dictionary<long, RepairRequest>();

    private int count {get; set;} = File.Exists("repair.txt") ? File.ReadAllLines("repair.txt").Length : 0;
    private readonly string[] _menuButtons = {"Наши услуги", "Цены", "Оставить заявку", "Контакты"};

    public string HandleMessage(long chatId, string text)
    {
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
            switch (text)
            {
                case "Оставить заявку" :
                notFullInfo[chatId] = new RepairRequest {ChatId = chatId};
                _stages[chatId] = StageEnum.Name;
                return "Как вас зовут?";

                case "Наши услуги" : 
                return "Ремонт стиральной машины\nРемонт холодильника\nРемонт посудомоечной машины\nРемонт духовых шкафов";

                case "Цены" : 
                return "Диагностика: от 44 byn\nРемонт: от 60 byn\nТочную цену назовет мастер после диагностики";

                case "Контакты" :
                return "Телефон: +375 29 000-00-00\nАдрес: г. Минск, ул. Примерная, 1";
                default :
                return "Здравствуйте! Выберите пункт меню.";
            }
            case StageEnum.Name :
            if (_menuButtons.Contains(text))
            {
                return "Сейчас заполнение заявки. Введите данные или нажмиите «Отмена»";
            }
            notFullInfo[chatId].Name = text;
            _stages[chatId] = StageEnum.PhoneNumber;
            return "Введите ваш номер телефона";

            case StageEnum.PhoneNumber :
            if (IsValidPhoneNumber(text))
            {
                 notFullInfo[chatId].PhoneNumber = text;
                _stages[chatId] = StageEnum.Description;
                return "Введите описание проблемы";
            }
            else
            {
                _stages[chatId] = StageEnum.PhoneNumber;
                return "Повторите еще раз!";
            }

            case StageEnum.Description :
            if (_menuButtons.Contains(text))
                {
                   return "Сейчас заполнение заявки. еедите данные или нажмиите «Отмена»";
                }
            notFullInfo[chatId].Description = text;
            _stages[chatId] = StageEnum.Confirmation;
            var r = notFullInfo[chatId]; 
            return $"Проверьте заявку:\nИмя: {r.Name}\nТелефон: {r.PhoneNumber}\nПроблема: {r.Description}\n\nВсё верно? (Да / Нет)";
            

            case StageEnum.Confirmation :
            switch (text)
            {
                case "Да" :
                var repair = notFullInfo[chatId];
                repair.orderStatus = OrderStatusEnum.Wait;
                repair.CreateAt = DateTime.Now;
                repair.Id = ++count;
                _stages[chatId] = StageEnum.Menu;
                SaveRequest(notFullInfo[chatId]);
                RequestCompleted?.Invoke(repair);
                notFullInfo.Remove(chatId);
                _stages[chatId] = StageEnum.Menu;
                return "Заявка принята, менеджер свяжется с вами!";

                case "Нет" :
                notFullInfo[chatId] = new RepairRequest {ChatId = chatId};
                _stages[chatId] = StageEnum.Name; 
                return "Давайте заполним заявку заново\nКак вас зовут?";
                
                default :
                return "Выберете пожалуйста Да / Нет";
            }

            default : 
            return "Что-то пошло не так, напишите /start.";
        }
    }

    public void SaveRequest(RepairRequest repair)
    {
        File.AppendAllText("repair.txt", repair + Environment.NewLine);
    }
    
    public event Action<RepairRequest>? RequestCompleted;

    public bool IsInMenu(long chatId)
    {
        return !_stages.ContainsKey(chatId) || _stages[chatId] == StageEnum.Menu;
    }

    public StageEnum GetStage (long chatId)
    {
        if(_stages.ContainsKey(chatId))
        {
            return _stages[chatId];
        }

        return StageEnum.Menu;
    }

    private bool IsValidPhoneNumber(string PhoneNumber)
    {
        string pattern = @"\D";
        string target = "";
        Regex regex = new Regex(pattern);
        string checkPhoneNumber = regex.Replace(PhoneNumber, target);

        return checkPhoneNumber.Length >= 9 && checkPhoneNumber.Length <= 12;
    }
}   