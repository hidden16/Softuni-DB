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

            ////////Problem.03
            int id = int.Parse(Console.ReadLine());
            result = commands.VillainsNameAndMinions(sqlConnection, id);
            Console.WriteLine(result);

            ////////Problem.04
            var minion = Console.ReadLine();
            var villain = Console.ReadLine();
            result = commands.AddMinionToVillain(sqlConnection, minion, villain);
            Console.WriteLine(result);

            //////Problem.05
            var countryName = Console.ReadLine();
            commands.ChangeTownNamesCasing(sqlConnection, countryName);

            //Problem.06
            int villainId = int.Parse(Console.ReadLine());
            result = commands.RemoveVillain(sqlConnection,villainId);
            Console.WriteLine(result);
            sqlConnection.Close();
        }
    }
}
