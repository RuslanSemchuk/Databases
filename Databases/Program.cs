using System;
using System.Data;
using System.Data.SQLite;
using Dapper;

class Program
{
    static void Main()
    {
        const string connectionString = "Data Source=sample.db;Version=3;";

        using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
        {
            dbConnection.Open();

            
            dbConnection.Execute("CREATE DATABASE IF NOT EXISTS mydb");

           
            dbConnection.Execute(@"
                CREATE TABLE IF NOT EXISTS MyTable (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Age INTEGER
                )");

           
            var data = new { Name = "John", Age = 30 };
            dbConnection.Execute("INSERT INTO MyTable (Name, Age) VALUES (@Name, @Age)", data);

           
            var updatedData = new { Name = "UpdatedName", Age = 31, ID = 1 };
            dbConnection.Execute("UPDATE MyTable SET Name = @Name, Age = @Age WHERE ID = @ID", updatedData);

           
            dbConnection.Execute("DELETE FROM MyTable WHERE ID = @ID", new { ID = 1 });

         
            var selectedData = dbConnection.Query("SELECT * FROM MyTable WHERE Age > @Age", new { Age = 25 });
            foreach (var item in selectedData)
            {
                Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Age: {item.Age}");
            }

           
            var allData = dbConnection.Query("SELECT * FROM MyTable");
            foreach (var item in allData)
            {
                Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Age: {item.Age}");
            }

            dbConnection.Close();
        }
    }
}
