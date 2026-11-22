using System.Data;
using System.Text.RegularExpressions;

public class RepairRequest
{
    public string Name {get; set;}
    public string PhoneNumber {get; set;} = "";
    public int Id {get; set;}
    public long ChatId {get; set;}
    public string Description {get; set;}
    public OrderStatusEnum orderStatus {get; set;}
    public DateTime CreateAt {get; set;}

    public bool IsValidPhoneNumber(string PhoneNumber)
    {
        string pattern = @"\D";
        string target = "";
        Regex regex = new Regex(pattern);
        string checkPhoneNumber = regex.Replace(PhoneNumber, target);

        return checkPhoneNumber.Length >= 9 && checkPhoneNumber.Length <= 12;
    }

    public override string ToString()
    {
        return $"{Id} | {Name} | {PhoneNumber} | {Description} | {orderStatus} | {CreateAt}";
    }
}