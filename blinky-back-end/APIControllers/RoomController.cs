using Models;


namespace RoomController
{
    public class RController
    {
        private Room[] rooms;

        public RController(Room[] rooms)
        {
            this.rooms = rooms;
            Console.WriteLine(this.rooms.Length);
        }

        public GetRoomIdsResponse GetAllRoomIds()
        {
            GetRoomIdsResponse response = new GetRoomIdsResponse();
            List<string> temp = new List<string>();
            foreach (Room room in this.rooms)
            {
                temp.Add(room.RoomId);
            }
            response.RoomIds = temp;
            return response;
        }

        public GetRoomDataResponse GetRoomData(GetRoomRequest request)
        {
            GetRoomDataResponse response = new GetRoomDataResponse();
            try
            {
                Room room = rooms[FindRoomById(request.roomId)];
                response.room = room;
                return response;
            }
            catch { return null; }
        }

        public bool BookDesk(BookDeskRequest request)
        {
            try
            {
                Room room = rooms[FindRoomById(request.roomId)];
                foreach (Desk desk in room.desks)
                {
                    if (desk.deskId == request.deskId && desk.assignedTo == null)
                    {
                        desk.assignedTo = request.assigneeName;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAllBookingsInRoom(GetRoomRequest request)
        {
            try
            {
                Room room = rooms[FindRoomById(request.roomId)];
                foreach (Desk desk in room.desks)
                {
                    desk.assignedTo = null;
                }
                return true;
            }
            catch { return false; }
        }


        private int FindRoomById(string roomId)
        {
            for (int i = 0; i < rooms.Count(); i++)
            {
                if (rooms[i].RoomId == roomId) { return i; }
            }
            return -1;
        }
    }
}