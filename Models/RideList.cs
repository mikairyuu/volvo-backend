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
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatorToken { get; set; }
    }
}