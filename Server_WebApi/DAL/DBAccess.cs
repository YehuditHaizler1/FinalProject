using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DAL
{
    public class DBAccess
    {
        static MySqlConnection Connection = new MySqlConnection
            (@"SERVER=127.0.0.1;PORT=3306;UID=root;PWD=1234;persistsecurityinfo=True;DATABASE=projects_managment;SslMode=none");
 
        public static int? RunNonQuery(string query)
        {
            try
            {
                Connection.Open();
                MySqlCommand command = new MySqlCommand(query, Connection);
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }

        }

        public static object RunScalar(string query)
        {
            try
            {
                Connection.Open();
                MySqlCommand command = new MySqlCommand(query, Connection);
                return command.ExecuteScalar();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }
        }

        public static List<T> RunReader<T>(string query, Func<MySqlDataReader, List<T>> func)
        {
            try
            {
                Connection.Open();
                MySqlCommand command = new MySqlCommand(query, Connection);
                MySqlDataReader reader = command.ExecuteReader();
                return func(reader);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                {
                    Connection.Close();
                }
            }
        }
    }
}