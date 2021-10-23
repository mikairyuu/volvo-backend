using System.Collections.Generic;

namespace volvo_backend.Models
{
    public class RouteLists
    {
        public List<Route> VolvoRoutes { get; set; }
        public List<Route> CustomRoutes { get; set; }
    }

    public class Route
    {
        public int Id  { get; set; }
        public string Img  { get; set; }
        public string Distance  { get; set; }
        public string Title  { get; set; }
        public string Description  { get; set; }
        public string LastUsedDate  { get; set; }
        public string Path  { get; set; }
    }
}