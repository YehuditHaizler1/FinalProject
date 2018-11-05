using BOL;
using BOL.Help;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class LogicUser
    {
        //Login - Get email and password, Check if the user exists.
        //If exists - return the user,
        //Else return null.
        public static Worker Login(string eMail, string password)
        {
            string query = $"SELECT* FROM projects_managment.workers WHERE eMail = '{eMail}' AND password = '{password}' ";

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

        public static List<Project> GetProjectsToWorker(int workerId)
        {
            string query = $"SELECT p.* FROM projects_managment.projects p " +
                $"JOIN projects_managment.projectsworker pw ON p.id = pw.projectId " +
                $"JOIN workers w ON pw.workerId = w.id WHERE w.id = {workerId} ";

            Func<MySqlDataReader, List<Project>> func = (reader) =>
            {
                List<Project> projects = new List<Project>();
                while (reader.Read())
                {
                    projects.Add(new Project
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        CustomerName = reader.GetString(2),
                        ProjectManagerId = reader.GetInt32(3),
                        ProgramHours = reader[4] as int?,
                        QAHours = reader[5] as int?,
                        UIUXHours = reader[6] as int?,
                        StartDate = reader.GetDateTime(7),
                        EndDate = reader.GetDateTime(8)
                    });
                }
                return projects;
            };

            List<Project> workerProjectsList = DBAccess.RunReader(query, func);
            return workerProjectsList;
        }

        public static bool AddPresence(Presence presence)
        {
            string query = $"INSERT INTO projects_managment.presences (workerId, projecId, beginningTime, endTime) VALUES ({presence.WorkerId},{presence.ProjectId},'{presence.BeginningTime.ToString("yyyy-MM-dd HH:mm:ss")}', NULL);";

            return DBAccess.RunNonQuery(query) == 1;
        }

        public static int GetCurrentPresenceId(Presence presence)
        {
            string query = $"SELECT id FROM projects_managment.presences WHERE " +
                $"workerId={presence.WorkerId} AND projecId={presence.ProjectId} AND beginningTime='{presence.BeginningTime.ToString("yyyy-MM-dd HH:mm:ss")}' ;";

            return (Int32)DBAccess.RunScalar(query);
        }

        //UpdateEndTime - Get presenceId and endtime, Update in D.B. the endTime in the correct object (by the presenceId)
        public static bool UpdateEndTime(int presenceId, DateTime endTime)
        {
            string query = $"UPDATE projects_managment.presences SET endTime = '{endTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {presenceId}; ";

            return DBAccess.RunNonQuery(query) == 1;
        }

        //GetHoursStatusToWorker - Get workerId, Return his hoursStatus
        public static List<HoursStatus> GetHoursStatusToWorker(int workerId)
        {
            string query = $"SELECT pr.workerId, pr.projecId, pw.totalHours*360 AS 'Required Hours', SUM(TIMESTAMPDIFF(Second,beginningTime,endTime)) AS 'Actual Hours' " +
                $"FROM projects_managment.presences pr JOIN projects_managment.projectsworker pw " +
                $"ON pr.projecId = pw.projectId " +
                $"WHERE pr.workerId = {workerId} AND pr.endTime IS NOT NULL " +
                $"GROUP BY projecId ";

            Func<MySqlDataReader, List<HoursStatus>> func = (reader) =>
            {
                List<HoursStatus> hoursStatus = new List<HoursStatus>();
                while (reader.Read())
                {
                    hoursStatus.Add(new HoursStatus
                    {
                        WorkerId = reader.GetInt32(0),
                        ProjectId = reader.GetInt32(1),
                        RequiredHours = reader.GetInt64(2) as long?,
                        ActualHours = reader.GetInt64(3) as long?
                    });
                }
                return hoursStatus;
            };

            List<HoursStatus> hoursStatusList = DBAccess.RunReader(query, func);
            return hoursStatusList;
        }

    }
}
