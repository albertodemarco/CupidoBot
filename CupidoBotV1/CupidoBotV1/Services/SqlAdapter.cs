using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CupidoBotV1.Data;

namespace CupidoBotV1.Services
{
    public class SqlAdapter
    {
        public static async Task saveData(CupidoProfile i)
        {

            DateTime dt = DateTime.Now;
            string myConnStr = ConfigurationManager.ConnectionStrings["CupidoConn"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = myConnStr;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT TOP 1 Id ");
                sb.Append("FROM Cupido ");
                sb.Append("WHERE ");
                sb.Append("UserId='" + i.UserId + "' AND ChannelId='" + i.ChannelId + "';");
                String sql = sb.ToString();
                long Id = -1;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = reader.GetInt64(0);
                        }
                    }
                }
                if (Id != -1)
                {
                    string query = "UPDATE Cupido SET Gender=@Gender,Age=@Age,PicUrl=@PicUrl,NormalPoints=@NormalPoints,SuperModelPoints=@SuperModelPoints,HairBald=@HairBald,FirstName=@FirstName,LastName=@LastName,Updated=@Updated WHERE Id=" + Id.ToString();
                    SqlCommand myCommand = new SqlCommand(query, connection);
                    myCommand.Parameters.AddWithValue("@Gender", i.Gender);
                    myCommand.Parameters.AddWithValue("@Age", i.Age);
                    myCommand.Parameters.AddWithValue("@PicUrl", i.picUrl);
                    myCommand.Parameters.AddWithValue("@NormalPoints", i.NormalPoints);
                    myCommand.Parameters.AddWithValue("@SuperModelPoints", i.SuperModelPoints);
                    myCommand.Parameters.AddWithValue("@HairBald", i.HairBald);
                    myCommand.Parameters.AddWithValue("@FirstName", i.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", i.LastName);
                    myCommand.Parameters.AddWithValue("@Updated", dt);
                    // ... other parameters
                    await myCommand.ExecuteNonQueryAsync();
                }
                else
                {
                    string query = "INSERT INTO Cupido (UserId,ChannelId,Gender,Age,PicUrl,NormalPoints,SuperModelPoints,HairBald,FirstName,LastName,Created,Updated)";
                    query += " VALUES (@UserId, @ChannelId, @Gender, @Age,@PicUrl, @NormalPoints, @SuperModelPoints, @HairBald, @FirstName, @LastName, @Created,@Updated)";
                    SqlCommand myCommand = new SqlCommand(query, connection);
                    myCommand.Parameters.AddWithValue("@UserId", i.UserId);
                    myCommand.Parameters.AddWithValue("@ChannelId", i.ChannelId);
                    myCommand.Parameters.AddWithValue("@Gender", i.Gender);
                    myCommand.Parameters.AddWithValue("@Age", i.Age);
                    myCommand.Parameters.AddWithValue("@PicUrl", i.picUrl);
                    myCommand.Parameters.AddWithValue("@NormalPoints", i.NormalPoints);
                    myCommand.Parameters.AddWithValue("@SuperModelPoints", i.SuperModelPoints);
                    myCommand.Parameters.AddWithValue("@HairBald", i.HairBald);
                    myCommand.Parameters.AddWithValue("@FirstName", i.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", i.LastName);
                    myCommand.Parameters.AddWithValue("@Created", dt);
                    myCommand.Parameters.AddWithValue("@Updated", dt);
                    // ... other parameters
                    await myCommand.ExecuteNonQueryAsync();
                }
            }



        }

        public static List<CupidoProfile> getSimilarProfiles(CupidoProfile i)
        {

            List<CupidoProfile> result = new List<CupidoProfile>();
            int agemin = i.Age - 15;
            int agemax = i.Age + 15;
            string myConnStr = ConfigurationManager.ConnectionStrings["CupidoConn"].ConnectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = myConnStr;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT TOP 3 ChannelId,Gender,Age,PicUrl,NormalPoints,SuperModelPoints,HairBald,FirstName,LastName,UserId ");
                sb.Append("FROM Cupido ");
                sb.Append("WHERE ");
                sb.Append("Age >= " + agemin + " AND Age <= " + agemax + " ");
                sb.Append("AND Gender<>'" + i.Gender + "' ");
                sb.Append("Order By SuperModelPoints DESC;");
                String sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CupidoProfile cp = new CupidoProfile();
                            cp.ChannelId = reader.GetString(0);
                            cp.Gender = reader.GetString(1);
                            cp.Age = reader.GetInt32(2);
                            cp.picUrl = reader.GetString(3);
                            cp.NormalPoints = reader.GetInt32(4);
                            cp.SuperModelPoints = reader.GetInt32(5);
                            cp.HairBald = reader.GetBoolean(6) ? 1 : 0;
                            cp.FirstName = reader.GetString(7);
                            cp.LastName = reader.GetString(8);
                            cp.UserId = reader.GetString(9);
                            result.Add(cp);
                        }
                    }
                }
            }
            return result;
        }
    }
}