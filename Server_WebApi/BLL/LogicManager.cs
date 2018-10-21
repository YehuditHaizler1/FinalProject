using BOL;
using DAL;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLL
{
    public static class LogicManager
    {
        //curl -X GET -v http://localhost:60828/api/User
        public static List<Worker> GetAllWorkers()
        {
            string query = $"SELECT * FROM projects_managment.worker";

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

            return DBAccess.RunReader(query, func);
        }

        //public static string GetUserName(int id)
        //{
        //    string query = $"SELECT Name FROM [dbo].[Users] WHERE Id={id}";
        //    return DBAccess.RunScalar(query).ToString();
        //}

        //public static bool RemoveUser(int id)
        //{
        //    string query = $"DELETE FROM [dbo].[Users] WHERE Id={id}";
        //    return DBAccess.RunNonQuery(query) == 1;
        //}

        //public static bool UpdateUser(User user)
        //{
        //    string query = $"UPDATE [dbo].[Users] SET Name='{user.UserName}', Age={user.Age}  WHERE Id={user.Id}";
        //    return DBAccess.RunNonQuery(query) == 1;
        //}
        //curl -v -X POST -H "Content-type: application/json" -d "{\"UserName\":\"Malki\",\"Password\":\"mmmmmm\" , \"FirstName\":\"jjj\", \"LastName\":\"hhh\",\"EMail\":\"sjafjkl@df.af\", \"Phone\":\"9999999988\", \"StatusId\":\"2\, \"ManagerId\":\"1\", \"TotalHours\":\"50\"}"  http://localhost:60828/api/AddWorker
        //curl -v -X POST -H "Content-type: application/json" -d "{\"UserName\":\"Test2\", \"Password\":\"88888\", \"FirstName\":\"jjj\", \"LastName\":\"hhh\", \"EMail\":\"ggg@gmail.com\", \"Phone\":\"888888888\", \"StatusId\":\"2\", \"ManagerId\":\"1\", \"TotalHours\":\"50\"}"  http://localhost:60828/api/User
        public static bool AddWorker(Worker worker)
        {
            string query = $"INSERT INTO worker (userName, password, firstName, lastName, eMail, phone, statusId, managerId, totalHours)" +
                $" VALUES ('{worker.UserName}','{worker.Password}','{worker.FirstName}','{worker.LastName}','{worker.EMail}','{worker.Phone}',{worker.StatusId},'{worker.ManagerId}','{worker.TotalHours}')";
            return DBAccess.RunNonQuery(query) == 1;
        }
    }
}
