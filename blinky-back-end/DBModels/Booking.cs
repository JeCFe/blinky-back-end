using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blinky_Back_End.DbModels;


public class Booking
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string AssignedName { get; set; }
    [Required]
    public DateOnly Date { get; set; } = new DateOnly();
    [Required]
    public Desk Desk { get; set; } = new Desk();
    public Guid DeskId { get; set; }


}