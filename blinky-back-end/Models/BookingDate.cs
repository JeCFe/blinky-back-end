
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Blinky_Back_End.Model;

[Keyless]
public class BookingDate
{
    public string day { get; set; }
    public string month { get; set; }
    public string year { get; set; }
}