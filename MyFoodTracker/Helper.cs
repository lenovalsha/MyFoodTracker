using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MyFoodTracker
{
    public static class Helper
    {
        private static string dir = "..\\..\\Files";
        private static string filepath = "..\\..\\Files\\myFoodTracker.db"; //In order for this file to be created make sure the Files folder exist
        private static string connString = $@"Data Source={filepath}; Version=3";

        public static void InitializeDatabase()
        {
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (!File.Exists(filepath))
                {
                    SQLiteConnection.CreateFile(filepath);
                    using (var conn = new SQLiteConnection(connString)) //create a connection
                    {
                        conn.Open(); //open the connection

                        //create tables qry
                        string createMealTableQry = @"CREATE TABLE IF NOT EXISTS Meals (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);";

                        string createFoodableQry = @"CREATE TABLE IF NOT EXISTS Foods (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL, MealId INTEGER, FoodDate Date NOT NULL, Foreign Key (MealId) References Meals(Id));";
                        using (var cmd = new SQLiteCommand(conn)) //create our command 
                        {
                            //create our actual table and put into our database
                            cmd.CommandText = createFoodableQry;
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = createMealTableQry;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    AddMealsAutomatically();
                    CreateFoodAutomatically();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        #region Automatic 
        public static void AddMealsAutomatically()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                string[] meals = { "Breakfast", "Lunch", "Supper", "Snack" };
                using (var cmd = new SQLiteCommand(conn))
                {

                    foreach (string meal in meals)
                    {
                        cmd.CommandText = @"INSERT INTO MEALS(Name) VALUES(@name)";
                        cmd.Parameters.AddWithValue("@name", meal);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }

            }
        }
        public static void CreateFoodAutomatically()
        {
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string[] foods = { "apples", "oranges", "sausage" };
                int[] mealIds = { 0, 2, 3 };
                int i = 0;
                using (var cmd = new SQLiteCommand(conn))
                {
                    foreach (string food in foods)
                    {
                        cmd.CommandText = @"INSERT INTO Foods(Name, MealId, FoodDate) VALUES(@food, @mealId, @foodDate)";
                        cmd.Parameters.AddWithValue("@food", food);
                        cmd.Parameters.AddWithValue("@mealId", mealIds[i]);
                        cmd.Parameters.AddWithValue("@foodDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        i++;
                    }

                }
            }

        }
        #endregion

        public static void CreateFoodEntry(string food, int mealId, DateTime foodDate)
        {
            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "INSERT INTO FOODS (Name, MealId, FoodDate) VALUES (@food, @mealId, @foodDate)";
            cmd.Parameters.AddWithValue("@food", food);
            cmd.Parameters.AddWithValue("@mealId", mealId);
            cmd.Parameters.AddWithValue("@foodDate", foodDate.ToLocalTime());

            cmd.ExecuteNonQuery();
            conn.Close();

        }
        

        #region READ
        public static void GetMealList(ComboBox comboBox)
        {
            comboBox.DisplayMember = "Name";
            var conn = new SQLiteConnection(connString);
            conn.Open();
            var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "SELECT NAME FROM MEALS";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //int id = reader.GetInt32(0); //assuming Id is of type integer
                string name = reader.GetString(0); //assyming bane us if tyoe string
                comboBox.Items.Add(new { Name = name });

            }
            conn.Close();
        }

        #endregion
    }
}
