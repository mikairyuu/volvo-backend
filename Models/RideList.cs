using System.Collections.Generic;

namespace volvo_backend.Models
{
    public class Ride
    {
        public List<UserInfo> userList { get; set; }
        public RouteModel route { get; set; }
    }

    public class RideApplication
    {
        public int RouteId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
    public class RideCreation
    {
        public List<UserInfo> userList { get; set; }
        public int RideId { get; set; }
        public int RouteId { get; set; }
    }
}