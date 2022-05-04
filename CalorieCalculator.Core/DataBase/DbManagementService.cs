using CalorieCalculator.Core.DataBase.Factories;
using CalorieCalculator.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CalorieCalculator.Core.DataBase
{
    public class DbManagementService : IDisposable, IDbManager
    {
        private readonly string ConnectionString = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=Calorie_calculator;Integrated Security=True";
        private readonly CalculatorFactory factory = new CalculatorFactory();

        public void AddProductToDb(Product product)
        {
            string commandText = "INSERT INTO Products VALUES (@name, @calories, @proteins, @carbs, @fats)";
            InsertToDbQuery(commandText, product);
        }
        public void DeleteProductFromDb(Product product)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;
            string commandText = "DELETE FROM Products WHERE Product_name = @name";

            SqlParameter param = new SqlParameter("@name", product.Name);
            bool result = ExecuteNonQueryCommand(connection, cmdType, commandText, param);
            if (result)
            {
                Console.WriteLine("Your product has been deleted");
            }
            else
            {
                Console.WriteLine("Your product has not been deleted");
            }
        }
        public List<Product> ReadProductListFromDb()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            List<Product> productList = new List<Product>();
            string commandText = "SELECT * FROM Products";
            DataTable table = ExecuteSelectCommand(connection, cmdType, commandText);

            foreach (DataRow item in table.Rows)
            {
                Product product = factory.CreateProduct(item);
                productList.Add(product);
            }
            return productList;
        }
        public Setting GetSettingFromDb(string key)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            string commandText = "SELECT * FROM Settings WHERE [Key] = @key";
            SqlParameter param = new SqlParameter("@key", key);
            DataTable table = ExecuteSelectCommand(connection, cmdType, commandText, param);
            if (table.Rows.Count > 0)
            {
                Setting setting = factory.CreateSetting(table.Rows[0]);
                return setting;
            }
            else
            {
                return null;
            }
        }
        public void UpdateSettingInDb(string key, string value)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            string commandText = "UPDATE Settings Set Value = @value WHERE [Key] = @key";
            SqlParameter[] paramList = new SqlParameter[2];
            paramList[0] = new SqlParameter("@key", key);
            paramList[1] = new SqlParameter("@value", value);
            ExecuteNonQueryCommand(connection, cmdType, commandText, paramList);
        }
        public DailySum ReadTodaysMacrosFromDb(string todaysDate)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            string commandText = "SELECT * FROM Daily_sum WHERE Date  = @today";
            SqlParameter param = new SqlParameter("@today", todaysDate);
            DataTable table = ExecuteSelectCommand(connection, cmdType, commandText, param);
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                DailySum todaysMacros = factory.CreateTodaysMacros(row);
                return todaysMacros;
            }
            else
                return null;
        }
        public void InsertNewDayToDb(DailySum todaysMacros)
        {
            string commandText = "INSERT INTO Daily_sum VALUES (@name, @calories, @proteins, @carbs, @fats)";
            InsertToDbQuery(commandText, todaysMacros);
        }
        public void UpdateTodaysMacros(DailySum todaysMacros)
        {
            string commandText = "UPDATE Daily_sum SET Calories = @calories, Proteins = @proteins, Carbohydrates = @carbs, Fats = @fats" +
                " WHERE Date = @name";
            InsertToDbQuery(commandText, todaysMacros);
        }

        private SqlParameter[] SetSqlParameters(Product product)
        {
            SqlParameter[] paramList = new SqlParameter[5];
            paramList[0] = new SqlParameter("@name", product.Name);
            paramList[1] = new SqlParameter("@calories", product.Calories);
            paramList[2] = new SqlParameter("@proteins", product.Proteins);
            paramList[3] = new SqlParameter("@carbs", product.Carbs);
            paramList[4] = new SqlParameter("@fats", product.Fats);           
            return paramList;
        }
        private SqlParameter[] SetSqlParameters(DailySum dailySum)
        {
            SqlParameter[] paramList = new SqlParameter[5];
            paramList[0] = new SqlParameter("@name", dailySum.Date);
            paramList[1] = new SqlParameter("@calories", dailySum.Calories);
            paramList[2] = new SqlParameter("@proteins", dailySum.Proteins);
            paramList[3] = new SqlParameter("@carbs", dailySum.Carbs);
            paramList[4] = new SqlParameter("@fats", dailySum.Fats);
            return paramList;
        }
        private void InsertToDbQuery(string commandText, Product product)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            SqlParameter[] paramList = SetSqlParameters(product);

            bool result = ExecuteNonQueryCommand(connection, cmdType, commandText, paramList);
            if (result)
                Console.WriteLine("Your product has been added");
            else
                Console.WriteLine("Your product has not been added");
        }
        private void InsertToDbQuery(string commandText, DailySum dailySum)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;

            SqlParameter[] paramList = SetSqlParameters(dailySum);

            bool result = ExecuteNonQueryCommand(connection, cmdType, commandText, paramList);
            if (result)
                Console.WriteLine("Your product has been added");
            else
                Console.WriteLine("Your product has not been added");
        }
        private bool ExecuteNonQueryCommand(SqlConnection con, CommandType cmdType, string commandText, SqlParameter[] paramList)
        {
            int result = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = commandText;
            cmd.Parameters.AddRange(paramList);
            if (con.State == ConnectionState.Closed)
                con.Open();
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                con.Close();
            }
            if (result >= 1)
                return true;
            else return false;

        }
        private bool ExecuteNonQueryCommand(SqlConnection con, CommandType cmdType, string commandText, SqlParameter param)
        {
            int result = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = commandText;
            cmd.Parameters.Add(param);
            if (con.State == ConnectionState.Closed)
                con.Open();
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                con.Close();
            }
            if (result >= 1)
                return true;
            else return false;

        }
        private DataTable ExecuteSelectCommand(SqlConnection connection, CommandType cmdType, string commandText)
        {
            SqlCommand cmd = null;
            DataTable table = new DataTable();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = commandText;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                using (da = new SqlDataAdapter(cmd))
                {
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                connection.Close();
            }
            return table;
        }
        private DataTable ExecuteSelectCommand(SqlConnection connection, CommandType cmdType, string commandText, SqlParameter param)
        {
            SqlCommand cmd = null;
            DataTable table = new DataTable();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = commandText;
            cmd.Parameters.Add(param);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                using (da = new SqlDataAdapter(cmd))
                {
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                connection.Close();
            }
            return table;
        }
        public void Dispose()
        {
            
        }
        [Obsolete]
        public bool CheckIfProductExists(string choice)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;
            string commandText = "SELECT * FROM Products WHERE Product_name  = @name";

            SqlParameter param = new SqlParameter("@name", choice);
            DataTable table = ExecuteSelectCommand(connection, cmdType, commandText, param);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        [Obsolete]
        public bool CheckIfTodaysMacrosExist(string todaysDate)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            CommandType cmdType = CommandType.Text;
            string commandText = "SELECT * FROM Daily_sum WHERE Date  = @today";

            SqlParameter param = new SqlParameter("@today", todaysDate);
            DataTable table = ExecuteSelectCommand(connection, cmdType, commandText, param);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }      
        
    }
}
