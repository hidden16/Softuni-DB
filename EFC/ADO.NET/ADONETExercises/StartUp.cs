using System;
using System.Data.SqlClient;

namespace ADONETExercises
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            //Problem.02
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            SqlCommands commands = new SqlCommands();
            sqlConnection.Open();
            var result = commands.VillainsNamesWithMoreThan3Minions(sqlConnection);
            Console.WriteLine(result);

            //Problem.03
            int id = int.Parse(Console.ReadLine());
            result = commands.VillainsNameAndMinions(sqlConnection, id);
            Console.WriteLine(result);

            //Problem.04
            var minion = Console.ReadLine();
            var villain = Console.ReadLine();
            result = commands.AddMinionToVillain(sqlConnection, minion, villain);
            Console.WriteLine(result);

            //Problem.05


            sqlConnection.Close();
        }
    }
}
