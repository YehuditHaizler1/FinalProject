using BOL;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLL
{
    public static class LogicManager
    {
        //GetAllStatus - Return the statusses
        public static List<Status> GetAllStatus()
        {
            string query = $"SELECT * FROM projects_managment.status";

            Func<MySqlDataReader, List<Status>> func = (reader) =>
            {
                List<Status> statusList = new List<Status>();
                while (reader.Read())
                {
                    statusList.Add(new Status
                    {
                        Id = reader.GetInt32(0),
                        StatusName = reader.GetString(1)
                    });
                }
                return statusList;
            };

            return DBAccess.RunReader(query, func);
        }

        //GetAllCompanyWorkers - Return the company-workers
        public static List<Worker> GetAllCompanyWorkers()
        {
            string query = $"SELECT * FROM projects_managment.workers";

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

        //GetAllWorkers - Return all workers (program, QA, UI/UX)
        public static List<Worker> GetAllWorkers()
        {
            string query = $"SELECT * FROM projects_managment.workers w JOIN projects_managment.status s ON w.statusId = s.id " +
                $"WHERE s.statusName in ('Programmer', 'QA', 'UI/UX') ; ";

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

        //GetAllTeamLeaders - Return all team-leaders
        public static List<Worker> GetAllTeamLeaders()
        {
            string query = $"SELECT * FROM projects_managment.workers w JOIN projects_managment.status s ON w.statusId = s.id " +
                $" WHERE s.statusName = 'ProjectManager'";

            Func<MySqlDataReader, List<Worker>> func = (reader) =>
            {
                List<Worker> ProjectManager = new List<Worker>();
                while (reader.Read())
                {
                    ProjectManager.Add(new Worker
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
                return ProjectManager;
            };

            return DBAccess.RunReader(query, func);
        }

        //GetAllProjects - Return the projects
        public static List<Project> GetAllProjects()
        {
            string query = $"SELECT * FROM projects_managment.projects";

            Func<MySqlDataReader, List<Project>> func = (reader) =>
            {
                List<Project> projectiLst = new List<Project>();
                while (reader.Read())
                {
                    while (reader.Read())
                    {
                        projectiLst.Add(new Project
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
                }
                return projectiLst;
            };

            return DBAccess.RunReader(query, func);
        }

        //AddProject
        public static bool AddProject(Project project)
        {

            // INSERT INTO projects_managment.projects(name, customerName, projectManagerId, programHours, QAHours, UIUXHours, startDate, endDate)
            //VALUES('MemoryGame', 'Seldat', 2, 30, 15, 10, '2018-09-09', '2018-09-29');
            string query = $"INSERT INTO projects_managment.projects(name, customerName, projectManagerId, programHours, QAHours, UIUXHours, startDate, endDate)" +
                $" VALUES ( '{project.Name}','{project.CustomerName}',{project.ProjectManagerId},{project.ProgramHours},{project.QAHours},{project.UIUXHours},'{project.StartDate.ToString("yyyy-MM-dd")}','{project.EndDate.ToString("yyyy-MM-dd")}' ) ";
            return DBAccess.RunNonQuery(query) == 1;
        }

        //GetCurrentProjectId - Return projectId by its name
        public static int GetCurrentProjectId(string projectName)
        {
            string query = $"SELECT id FROM projects_managment.projects WHERE name = '{projectName}'";

            return (Int32)DBAccess.RunScalar(query);
        }

        //GetWorkersToManagers - Get team-leader-id, Return the list of his staff
        public static List<Worker> WorkersToCurrentManager(int managerId)
        {
            string query = $"SELECT * FROM projects_managment.workers WHERE managerId = { managerId }";

            Func<MySqlDataReader, List<Worker>> func = (reader) =>
            {
                List<Worker> workersToManager = new List<Worker>();
                while (reader.Read())
                {
                    workersToManager.Add(new Worker
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
                return workersToManager;
            };

            return DBAccess.RunReader(query, func);
        }

        //GetNotAllowedWorkersToProject - Get projectId, Return the workers are not allowed to work on that project
        public static List<Worker> GetNotAllowedWorkersToProject(int projectId)
        {
            string query = $"SELECT * FROM projects_managment.workers WHERE id NOT IN " +
                $"( SELECT workerId FROM projects_managment.projectsworker WHERE projectId = {projectId}) ";

            Func<MySqlDataReader, List<Worker>> func = (reader) =>
            {
                List<Worker> notAllowedWorkers = new List<Worker>();
                while (reader.Read())
                {
                    notAllowedWorkers.Add(new Worker
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
                return notAllowedWorkers;
            };

            return DBAccess.RunReader(query, func);
        }

        //AddWorkerToProject - Get projectId and workerId, Add the correct worker to the correct project's staff
        public static bool AddWorkerToProject(int projectId, int workerId)
        {
            string query = $" INSERT INTO projects_managment.projectsworker (projectId,totalHours,workerId) VALUES ({projectId},0,{workerId})";
            return DBAccess.RunNonQuery(query) == 1;
        }

        //AddWorker
        public static bool AddWorker(Worker worker)
        {
            string query = $"INSERT INTO projects_managment.workers (userName, password, firstName, lastName, eMail, phone, statusId, managerId, totalHours)" +
                $" VALUES ('{worker.UserName}','{worker.Password}','{worker.FirstName}','{worker.LastName}','{worker.EMail}','{worker.Phone}',{worker.StatusId},'{worker.ManagerId}','{worker.TotalHours}')";
            return DBAccess.RunNonQuery(query) == 1;
        }

        //GetWorkerById
        public static Worker GetWorkerById(int workerId)
        {
            string query = $"SELECT * FROM projects_managment.workers WHERE Id = { workerId} ";
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

        //UpdateWorker - Update all properties, without the password
        public static bool UpdateWorker(Worker worker)
        {
            string query = $"UPDATE projects_managment.workers SET " +
                $"userName = '{worker.UserName}', firstName = '{worker.FirstName}', lastName = '{worker.LastName}', eMail = '{worker.EMail}', phone = '{worker.Phone}'," +
                $" statusId = '{worker.StatusId}', managerId = '{worker.ManagerId}', totalHours = '{worker.TotalHours}' WHERE id = { worker.Id }";
            return DBAccess.RunNonQuery(query) == 1;
        }

        //RemoveWorker
        public static bool RemoveWorker(int workerId)
        {
            string query;
            try
            {
                query = $" DELETE FROM projects_managment.projectsworker WHERE workerId = {workerId}";
                if (DBAccess.RunNonQuery(query) != 1)
                    throw new Exception();

                query = $" DELETE FROM projects_managment.presences WHERE workerId = {workerId}";
                if (DBAccess.RunNonQuery(query) != 1)
                    throw new Exception();

                query = $"DELETE FROM projects_managment.workers WHERE Id = {workerId}";
                if (DBAccess.RunNonQuery(query) != 1)
                    throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //UpdateTeamLeaderToWorker - Update teamLeaderId to the correct worker
        public static bool UpdateTeamLeaderToWorker(int workerId, int teamLeaderId)
        {
            string query = $"UPDATE projects_managment.workers SET managerId= {teamLeaderId} WHERE id= {workerId}";
            return DBAccess.RunNonQuery(query) == 1;
        }
    }
}

