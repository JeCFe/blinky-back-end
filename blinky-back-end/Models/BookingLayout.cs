using System.ComponentModel.DataAnnotations;

namespace Blinky_Back_End.Model;

public class BookingLayout
{

    public string Id { get; set; }
    public string day { get; set; }
    public string month { get; set; }
    public string year { get; set; }
    public List<Desk> desks { get; set; } = new();
}