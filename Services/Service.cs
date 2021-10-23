using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using volvo_backend.Models;
using volvo_backend.Utils;

namespace volvo_backend.Services
{
    public class Service
    {
        public static List<UserInfo> GetUsersByRideId(int id)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand(
                $"SELECT * FROM ( SELECT user_id FROM eventuser WHERE event_id = @id ) AS fr JOIN usertable on usertable.user_id = fr.user_id");
            cmd.Parameters.AddWithValue("@id", id);
            var reader = dbase.GetReader(cmd);
            var userList = new List<UserInfo>();
            while (reader.Read())
            {
                userList.Add(new UserInfo()
                {
                    Id = reader.GetInt32("user_id"),
                    Name = reader.GetString("username"),
                    Img = reader.GetString("Img")
                });
            }
            dbase.CloseConnection();
            return userList;
        }


        public static RideCreation CreateRide(RideApplication ride)
        {
            var dbase = new DBManager();
            var cmd = new MySqlCommand(
                $"INSERT INTO eventtable(route_id, event_description, is_open) VALUES (@route_id,@event_description,true);" +
                $"SET @id=(SELECT LAST_INSERT_ID()); INSERT INTO eventuser(event_id,user_id) VALUES (@id,@user_id);SELECT @id");
            cmd.Parameters.AddWithValue("@route_id", ride.RouteId);
            cmd.Parameters.AddWithValue("@event_description", ride.Description);
            cmd.Parameters.AddWithValue("@user_id", ride.UserId);
            var reader = dbase.GetReader(cmd);
            reader.Read();
            var lobbyId = reader.GetInt32(0);
            dbase.CloseReader();


            cmd = new MySqlCommand($"select * from usertable where user_id = @id");
            cmd.Parameters.AddWithValue("@id", ride.UserId);
            reader = dbase.GetReader(cmd);
            reader.Read();
            var Ride = new RideCreation()
            {
                userList = new List<UserInfo>() {
                    new() {
                        Id = ride.UserId,
                        Name = reader.GetString("username"),
                        Img = reader.GetString("Img") } },
                RideId = lobbyId,
                RouteId = ride.RouteId
            };

            dbase.CloseConnection();
            return Ride;
        }


    }
}
