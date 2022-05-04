using CalorieCalculator.Core.DataBase.Models;
using System;

namespace CreateDb
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cs = $"Data Source=.;Initial Catalog=Nazwa bazy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (var db = new CalculatorDbContext(cs))
                {
                    db.Database.CreateIfNotExists();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

    }
}
