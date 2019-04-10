using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace simple_crud_2
{
    class bdManager
    {

        static string serverAdress = "127.0.0.1";
        static string serverPort = "5432";
        static string userName = "postgres";
        static string userPass = "password";
        static string database = "Cadastro";
        public string connString = String.Format("Host={0};Port={1};Username={2};Password={3};Database={4}",
                                                serverAdress, serverPort, userName, userPass, database);
        public NpgsqlConnection conn;
        public string cmdString;
        public NpgsqlCommand cmd;

        public bdManager()
        {
            conn = new NpgsqlConnection(connString);
        }
    }
}
