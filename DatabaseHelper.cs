using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace UAS_RPL
{
    public static class DatabaseHelper
    {
        private static string connectionString = "server=localhost;user=root;database=uas_rpl;port=3306;password=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void OpenConnection(MySqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public static void CloseConnection(MySqlConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public static DataTable ExecuteQuery(string query)
        {
            using (MySqlConnection connection = GetConnection())
            {
                OpenConnection(connection);
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                CloseConnection(connection);
                return dataTable;
            }
        }
    }
}