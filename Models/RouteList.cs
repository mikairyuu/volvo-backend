using System.Collections.Generic;

namespace volvo_backend.Models
{
    public class RouteLists
    {
        public List<RouteModel> VolvoRoutes { get; set; }
        public List<RouteModel> CustomRoutes { get; set; }
    }

    public class RouteModel
    {
        public int Id  { get; set; }
        public string Img  { get; set; }
        public int Distance  { get; set; }
        public string Title  { get; set; }
        public string Description  { get; set; }
        public int UseCount  { get; set; }
        public string LastUsedAtTS  { get; set; }
    }

    public class PathModel
    {
        public string Path  { get; set; }
    }
}