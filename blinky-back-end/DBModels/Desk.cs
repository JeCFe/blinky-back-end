using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blinky_Back_End.DbModels;


public class Desk
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public int posX { get; set; }
    [Required]
    public int posY { get; set; } = 0;
    [Required]
    [JsonIgnore]
    public Room Room { get; set; } = new Room();

}


