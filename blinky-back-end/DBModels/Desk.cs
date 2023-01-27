using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blinky_Back_End.DbModels;


public class Desk
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public Room Room { get; set; } = new Room();
    public List<Booking> Bookings { get; set; } = new List<Booking>();

}


