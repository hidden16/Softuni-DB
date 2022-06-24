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
        private StringBuilder sb = new StringBuilder();
        public string VillainsNamesWithMoreThan3Minions(SqlConnection sqlConnection)
        {
            sb = new StringBuilder();
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
        public string VillainsNameAndMinions(SqlConnection sqlConnection, int id)
        {
            sb = new StringBuilder();
            query = @$"SELECT Name FROM Villains WHERE Id = {id}";
            SqlCommand cmd = new SqlCommand(query,sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sb.AppendLine($"{reader["Name"]}");
            }
            if (sb.ToString() == "")
            {
                return $"No villain with ID {id} exists in the database.";
            }
            reader.Close(); // closing first reader so the second can run

            int stringLengthBeforeMinions = sb.ToString().Length; // taking string length to check if there are minions
            query = $@"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = {id}
                                ORDER BY m.Name";
            cmd = new SqlCommand(query, sqlConnection);
            using SqlDataReader readerTwo = cmd.ExecuteReader();
            while (readerTwo.Read())
            {
                int i = 1;
                sb.AppendLine($"{i++}. {readerTwo["Name"]} {readerTwo["Age"]}");
            }
            int stringLengthAfterMinions = sb.ToString().Length;
            if (stringLengthBeforeMinions == stringLengthAfterMinions)
            {
                return $"(no minions)";
            }
            readerTwo.Close();
            return sb.ToString().TrimEnd();
        }
    }
}
