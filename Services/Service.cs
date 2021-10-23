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
            while (reader.Read()) {
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
    }
}
