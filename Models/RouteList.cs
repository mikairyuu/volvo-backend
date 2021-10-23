using System;
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
        public int Id { get; set; }
        public List<String> Img { get; set; }
        public int Distance { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UseCount { get; set; }
        public string LastUsedAtTS { get; set; }
    }

    public class PathModel
    {
        public string Path { get; set; }
        public List<PartnerOffer> partnerOffers { get; set; }
    }

    public class PartnerOffer
    {
        public string Title { get; set; }
        public string Discount { get; set; }
        public string Address { get; set; }
    }
}