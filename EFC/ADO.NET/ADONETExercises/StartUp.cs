using System;
using System.Data.SqlClient;

namespace ADONETExercises
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            SqlCommands commands = new SqlCommands();
            sqlConnection.Open();
            var result = commands.VillainsNamesWithMoreThan3Minions(sqlConnection);
            Console.WriteLine(result);
            sqlConnection.Close();
        }
    }
}
