using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONETExercises
{
    public class SqlCommands
    {
        private string query = string.Empty;

        public string VillainsNamesWithMoreThan3Minions(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();
            query = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                      FROM Villains AS v 
                      JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                      GROUP BY v.Id, v.Name 
                      HAVING COUNT(mv.VillainId) > 3 
                      ORDER BY COUNT(mv.VillainId)";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                sb.Append($"{reader["Name"]} - {reader["MinionsCount"]}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
