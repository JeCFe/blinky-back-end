namespace Blinky_Back_End.DbModels;


public class DeskAvailability
{

    public Desk Desk { get; set; }
    public bool Assigned => this.AssignedName != null;
    public string? AssignedName { get; set; }

}