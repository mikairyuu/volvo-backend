using System.Collections.Generic;

namespace volvo_backend.Models
{
    public class Ride
    {
        public  List<UserInfo> userList { get; set; }
        public RouteModel route { get; set; }
    }
}
