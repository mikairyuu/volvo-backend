using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using volvo_backend.Models;
using volvo_backend.Utils;

namespace volvo_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteController : ControllerBase
    {
        [HttpGet]
        public ActionResult<RouteLists> GetAllRoutes()
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand($"select * from routetable");
            var reader = dbase.GetReader(cmd);
            var Routes = new RouteLists();
            while (reader.Read())
            {
                if (reader.GetBoolean("isVolvo"))
                    Routes.VolvoRoutes.Add(
                        new Route()
                        {
                            Id = reader.GetInt32("route_id"),
                            Img = reader.GetString("route_img"),
                            Distance = reader.GetInt32("route_distance"),
                            Title = reader.GetString("route_name"),
                            Description = reader.GetString("route_description"),
                            LastUsedDate = reader.GetString("route_last_date")
                        });
                else
                    Routes.CustomRoutes.Add(
                        new Route()
                        {
                            Id = reader.GetInt32("route_id"),
                            Img = reader.GetString("route_img"),
                            Distance = reader.GetInt32("route_distance"),
                            Title = reader.GetString("route_name"),
                            Description = reader.GetString("route_description"),
                            LastUsedDate = reader.GetString("route_last_date")
                        });
            }
            return Routes;
        }
        [HttpGet]
        public ActionResult<Route> GetRouteById([FromQuery(Name = "RouteId")] int RouteId)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand($"select * from routetable where route_id = @id");
            cmd.Parameters.AddWithValue("@id", RouteId);
            var reader = dbase.GetReader(cmd);
            if (reader.Read())
                return new Route()
                {
                    Id = reader.GetInt32("route_id"),
                    Img = reader.GetString("route_img"),
                    Distance = reader.GetInt32("route_distance"),
                    Title = reader.GetString("route_name"),
                    Description = reader.GetString("route_description"),
                    LastUsedDate = reader.GetString("route_last_date")
                };
            return null;

        }
    }
}
