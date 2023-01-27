using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blinky_Back_End.DbModels;


public class Room
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public List<Desk> Desks { get; set; } = new List<Desk>();

}