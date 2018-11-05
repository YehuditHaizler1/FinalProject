using BOL;
using BOL.Help;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class LogicTeamLeader
    {

        //GetPresencesToTeamLeader - Get team-leader-id, Return the presences of the workers are working in that team-leader's staff 
        public static List<Presence> GetPresencesToTeamLeader(int projectManagerId)
        {
            string query = $"SELECT* FROM projects_managment.presences WHERE projectWorkerId in" +
                $"(SELECT id FROM projects_managment.projectsworker WHERE projectId in" +
                $"(SELECT id FROM projects_managment.projects WHERE projectManagerId = {projectManagerId}))";

            Func<MySqlDataReader, List<Presence>> func = (reader) =>
            {
                List<Presence> presences = new List<Presence>();
                while (reader.Read())
                {
                    presences.Add(new Presence
                    {
                        Id = reader.GetInt32(0),
                        WorkerId = reader.GetInt32(1),
                        ProjectId = reader.GetInt32(2),
                        BeginningTime = reader.GetDateTime(3),
                        EndTime = reader.GetDateTime(4)
                    });
                }
                return presences;
            };

            return DBAccess.RunReader(query, func);
        }

        //GetProjectsToTeamLeader - Get team-leader-id, Return the projects he manages
        public static List<Project> GetProjectsToTeamLeader(int teamLeaderId)
        {
            string query = $"SELECT * FROM projects_managment.projects WHERE projectManagerId = {teamLeaderId} ";

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

            return DBAccess.RunReader(query, func);
        }

        //GetWorkersToTeamLeader - Get team-leader-id, Return the workers he manages
        public static List<Worker> GetWorkersToTeamLeader(int teamLeaderId)
        {
            string query = $"SELECT * FROM projects_managment.workers WHERE managerId = {teamLeaderId}";

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

        //GetHoursStatusToProject - Get projectId, Return its hoursStatus
        public static List<HoursStatus> GetHoursStatusToProject(int projectId)
        {
            string query = $"SELECT pr.workerId, pr.projecId, pw.totalHours*360 AS 'Required Hours', SUM(TIMESTAMPDIFF(Second,beginningTime,endTime)) AS 'Actual Hours' " +
                $"FROM projects_managment.presences pr JOIN projects_managment.projectsworker pw " +
                $"ON pr.projecId = pw.projectId " +
                $"WHERE pr.projecId = {projectId} AND pr.endTime IS NOT NULL " +
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
