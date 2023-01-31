namespace Blinky_Back_End.DbModels;

public class ViewDesksResponse
{
    public List<DeskAvailability> DesksAvailability { get; }

    public ViewDesksResponse(List<Booking> bookings, List<Desk> desks)
    {
        DesksAvailability = desks.Select((desk) =>
        {
            DeskAvailability deskAvailability = new DeskAvailability();
            deskAvailability.Desk = desk;
            deskAvailability.AssignedName = bookings.FirstOrDefault((booking) => booking.Desk == desk)?.AssignedName;
            return deskAvailability;
        }).ToList();
    }
}