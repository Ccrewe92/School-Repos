using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
    public class DBConnect
    {
        private readonly MySqlConnection connection;
        private readonly string server;
        private readonly string database;
        private readonly string uid;
        private readonly string password;

        public DBConnect()
        {
            // Initialize database connection values.
            Initialize();
        }

        private void Initialize()
        {
            // Database connection details
            server = "localhost";
            database = "connectcsharptomysql";
            uid = "root";
            password = "password";

            // Create the connection string using a formatted string for clarity.
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                // Display friendly error messages
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server. Contact administrator.");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again.");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Insert()
        {
            // Sample method to demonstrate opening and closing a connection.
            // Implementation details would go here.
            OpenConnection();
            CloseConnection();
        }

        public void Update(string query)
        {
            if (OpenConnection())
            {
                // Use MySqlCommand to execute a given SQL query.
                using MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Delete(string query)
        {
            if (OpenConnection())
            {
                using MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public List<string>[] Select()
        {
            string query = "SELECT * FROM tableinfo";

            List<string>[] list = new List<string>[3]
            {
                new List<string>(),
                new List<string>(),
                new List<string>()
            };

            if (OpenConnection())
            {
                using MySqlCommand cmd = new MySqlCommand(query, connection);
                using MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"].ToString());
                    list[1].Add(dataReader["name"].ToString());
                    list[2].Add(dataReader["age"].ToString());
                }

                dataReader.Close();
                CloseConnection();
            }

            return list;
        }

        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            if (OpenConnection())
            {
                using MySqlCommand cmd = new MySqlCommand(query, connection);
                Count = int.Parse(cmd.ExecuteScalar().ToString());
                CloseConnection();
            }

            return Count;
        }

        public void Backup()
        {
            try
            {
                DateTime now = DateTime.Now;
                // Create a formatted filename for the backup file.
                string path = $"C:\\MySqlBackup{now:yyyy-MM-dd-HH-mm-ss-fff}.sql";

                using StreamWriter file = new StreamWriter(path);
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "mysqldump",
                    RedirectStandardOutput = true,
                    Arguments = $"-u{uid} -p{password} -h{server} {database}",
                    UseShellExecute = false
                };

                using Process process = Process.Start(psi);
                file.WriteLine(process.StandardOutput.ReadToEnd());
                process.WaitForExit();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error, unable to backup!");
            }
        }

        public void Restore()
        {
            try
            {
                string path = "C:\\MySqlBackup.sql";
                using StreamReader file = new StreamReader(path);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "mysql",
                    RedirectStandardInput = true,
                    Arguments = $"-u{uid} -p{password} -h{server} {database}",
                    UseShellExecute = false
                };

                using Process process = Process.Start(psi);
                process.StandardInput.WriteLine(file.ReadToEnd());
                process.WaitForExit();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error, unable to Restore!");
            }
        }
    }
}

