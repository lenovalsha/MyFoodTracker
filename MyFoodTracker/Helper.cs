﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

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
                if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if (!File.Exists(filepath))
                {
                    SQLiteConnection.CreateFile(filepath);
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();

                        string createMealTableQry = @"CREATE TABLE IF NOT EXISTS Meals (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);";

                        string createFoodableQry = @"CREATE TABLE IF NOT EXISTS Foods (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL, MealId INTEGER, FoodDate Date NOT NULL, Foreign Key (MealId) References Meals(Id));";
                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = createFoodableQry;
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = createMealTableQry;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


    }
}