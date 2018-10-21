using BOL;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class LogicUser
    {
        //curl -X GET -v http://localhost:60828/api/User

        //Login - get email and password, check if the user exists.
        //If exists - return the user,
        //Else return null.
        public static Worker Login(string eMail, string password)
        {
            string query = $"SELECT * FROM projects_managment.worker WHERE eMail = '{eMail}' AND password = '{password}' ";

            Func<MySqlDataReader, List<Worker>> func = (reader) =>
            {
                List<Worker> workers = new List<Worker>();
                while (reader.Read())
                {
                    workers.Add(new Worker
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4),
                        EMail = reader.GetString(5),
                        Phone = reader.GetString(6),
                        StatusId = reader.GetInt32(7),
                        ManagerId = reader[8] as int?,
                        TotalHours = reader[9] as int?
                    });
                }
                return workers;
            };

            List<Worker> worker = DBAccess.RunReader(query, func);
            if (worker != null && worker.Count > 0)
                return worker[0];
            return null;
        }
    }
}
