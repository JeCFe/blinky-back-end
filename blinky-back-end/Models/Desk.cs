namespace Blinky_Back_End.Model;

public class Desk
{
    public string Id { get; set; }
    public string DeskName { get; set; }
    public string? AssignedName { get; set; }
    public bool IsAvailable { get; set; }
}
