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

    

    public override string ToString()
    {
        return $"{Id} | {Name} | {PhoneNumber} | {Description} | {orderStatus} | {CreateAt}";
    }
}