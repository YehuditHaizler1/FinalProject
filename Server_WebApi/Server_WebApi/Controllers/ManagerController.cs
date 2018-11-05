using BLL;
using BOL;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Server_WebApi.Controllers
{
    public class ManagerController : ApiController
    {
        //curl -X GET -v http://localhost:60828/api/Manager/GetAllStatus
        //GetAllStatus - Return all statusses
        [HttpGet]
        [Route("api/Manager/GetAllStatus")]
        public HttpResponseMessage GetAllStatus()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Status>>(LogicManager.GetAllStatus(), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/Manager/GetAllCompanyWorkers
        //GetAllCompanyWorkers - Return all company workers
        [HttpGet]
        [Route("api/Manager/GetAllCompanyWorkers")]
        public HttpResponseMessage GetAllCompanyWorkers()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Worker>>(LogicManager.GetAllCompanyWorkers(), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/Manager/GetAllWorkers
        //GetAllWorkers - Return all workers (program, UI/UX, QA)
        [HttpGet]
        [Route("api/Manager/GetAllWorkers")]
        public HttpResponseMessage GetAllWorkers()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Worker>>(LogicManager.GetAllWorkers(), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/Manager/GetAllTeamLeaders
        //GetAllManagers - Return all teamLeaders
        [HttpGet]
        [Route("api/Manager/GetAllTeamLeaders")]
        public HttpResponseMessage GetAllTeamLeaders()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Worker>>(LogicManager.GetAllTeamLeaders(), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/Manager/GetAllProjects
        //GetAllProjects - Return all proects
        [HttpGet]
        [Route("api/Manager/GetAllProjects")]
        public HttpResponseMessage GetAllProjects()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Project>>(LogicManager.GetAllProjects(), new JsonMediaTypeFormatter())
            };
        }

        //AddWorker - Return match message
        [Route("api/Manager/AddWorker")]
        [HttpPost]
        public HttpResponseMessage AddWorker([FromBody]Worker newWorker)
        {
            if (ModelState.IsValid)
            {
                return (LogicManager.AddWorker(newWorker)) ?
                   new HttpResponseMessage(HttpStatusCode.Created) :
                   new HttpResponseMessage(HttpStatusCode.BadRequest)
                   {
                       Content = new ObjectContent<String>("Can not add to DB", new JsonMediaTypeFormatter())
                   };
            };

            List<string> ErrorList = new List<string>();

            //if the code reached this part - the user is not valid
            foreach (var item in ModelState.Values)
                foreach (var err in item.Errors)
                    ErrorList.Add(err.ErrorMessage);

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent<List<string>>(ErrorList, new JsonMediaTypeFormatter())
            };
        }

        //curl -v -X POST -H "Content-type: application/json" -d "{\"name\":\"project-name\",\"customerName\":\"customer-name\",\"projectManagerId\":\"2\",\"programHours\":\"50\",\"QAHours\":\"30\",\"UIUXHours\":\"25\",\"startDate\":\"2018-09-09 03:44:00\",\"endDate\":\"2018-09-29 03:44:00\"}"  http://localhost:60828/api/Manager/AddProject
        //1
        //AddProject - with all parameter and manageId (without workers)
        //Return current projectId
        [HttpPost]
        [Route("api/Manager/AddProject")]
        public HttpResponseMessage AddProject([FromBody]Project newProject)
        {
            List<string> ErrorList = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    //2
                    bool created = LogicManager.AddProject(newProject);
                    if (created)
                    //if AddProject is success, get return it.
                    {
                        //3
                        //return current project to know current project id 
                        int currentProjectId = LogicManager.GetCurrentProjectId(newProject.Name);
                        //4
                        //return all the staff of the manager to add them to workerProject table.
                        List<Worker> workerToCurrentManager = LogicManager.WorkersToCurrentManager(newProject.ProjectManagerId);
                        //add staff to workerProject
                        foreach (Worker worker in workerToCurrentManager)
                        {
                            try
                            {
                                LogicManager.AddWorkerToProject(currentProjectId, worker.Id);
                            }
                            catch (Exception e)
                            {
                                ErrorList.Add("Worker :" + worker.FirstName + " " + worker.LastName + "not added to workerProject" + e.Message);
                            }
                        }
                        //return the id of the new project
                        return Request.CreateResponse(HttpStatusCode.Created, currentProjectId);
                    }
                }
                foreach (var item in ModelState.Values)
                    foreach (var err in item.Errors)
                        ErrorList.Add(err.ErrorMessage);

                return Request.CreateResponse(HttpStatusCode.BadRequest, ErrorList);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //UpdateWorkerDetails - Update worker by id, Return match message
        [HttpPut]
        [Route("api/Manager/UpdateWorkerDetails/{workerId}")]
        public HttpResponseMessage UpdateWorkerDetails([FromBody]Worker worker)
        {

            if (ModelState.IsValid)
            {
                return (LogicManager.UpdateWorker(worker)) ?
                    new HttpResponseMessage(HttpStatusCode.OK) :
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<String>("Can not update in DB", new JsonMediaTypeFormatter())
                    };
            };

            List<string> ErrorList = new List<string>();

            //if the code reached this part - the user is not valid
            foreach (var item in ModelState.Values)
                foreach (var err in item.Errors)
                    ErrorList.Add(err.ErrorMessage);

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent<List<string>>(ErrorList, new JsonMediaTypeFormatter())
            };
        }

        //GetWorkerById
        [HttpGet]
        [Route("api/Manager/GetWorkerById/{workerId}")]
        public HttpResponseMessage GetWorkerById([FromUri]int workerId)
        {
            try
            {
                Worker worker = LogicManager.GetWorkerById(workerId);
                return Request.CreateResponse(HttpStatusCode.OK, worker);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        //DeleteWorker - Delete worker by id, Return match message
        [HttpDelete]
        [Route("api/Manager/DeleteWorker/{workerId}")]
        public HttpResponseMessage DeleteWorker([FromUri]int id)
        {
            return (LogicManager.RemoveWorker(id)) ?
                    new HttpResponseMessage(HttpStatusCode.OK) :
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<String>("Can not remove from DB", new JsonMediaTypeFormatter())
                    };
        }

        //AddOtherWorkersToProject - Add other workers to the correct project
        [HttpPost]
        [Route("api/Manager/AddOtherWorkerstoProject/{projectId}")]
        public HttpResponseMessage AddOtherWorkersToProject([FromBody]List<Worker> otherWorkers, [FromUri]int projectId)
        {
            try
            {
                List<ObjectContent<string>> content = new List<ObjectContent<string>>();
                foreach (Worker worker in otherWorkers)
                {
                    if (!AddWorkerToProject(projectId, worker))
                    {
                        content.Add(new ObjectContent<String>("Worker :" + worker.FirstName + " " + worker.LastName + "can not be added to workerProject", new JsonMediaTypeFormatter()));
                    }
                }
                if (content.Count > 0)
                    return new HttpResponseMessage(HttpStatusCode.Created) { Content = new ObjectContent<String>(content.ToString(), new JsonMediaTypeFormatter()) };
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<String>("can't add workers to project", new JsonMediaTypeFormatter())
                };
            }
        }

        //GetProjectsToWorker - Get workerId, Return Dictionary<int, string> where key is projectId and value is projectName
        [HttpGet]
        [Route("api/Manager/GetNotAllowedWorkersToProject/{projectId}")]
        public HttpResponseMessage GetNotAllowedWorkersToProject([FromUri]int projectId)
        {
            List<Worker> notAllowedWorkers;

            try
            {
                notAllowedWorkers = LogicManager.GetNotAllowedWorkersToProject(projectId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, notAllowedWorkers);
        }

        //AllowPremissionToWorker - Add worker to project, Return match message
        [HttpPost]
        [Route("api/Manager/AllowPremissionToWorker/{projectId}")]
        public HttpResponseMessage AllowPremissionToWorker([FromBody]Worker worker, [FromUri]int projectId)
        {
            return (AddWorkerToProject(projectId, worker)) ?
                    new HttpResponseMessage(HttpStatusCode.Created) :
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<String>("Worker :" + worker.FirstName + " " + worker.LastName + "can not be added to project: " + projectId, new JsonMediaTypeFormatter())
                    };
        }

        //AddWorkerToProject - Get projectId and worker, 
        //Return true: if succeeded adding that worker to the correct project, or false: if didn't succeed adding
        private bool AddWorkerToProject(int projectId, Worker worker)
        {
            return (LogicManager.AddWorkerToProject(projectId, worker.Id));
        }

        //UpdateTeamLeaderToWorker - Update teamLeaderId to the correct worker
        [HttpPut]
        [Route("api/Manager/UpdateTeamLeaderToWorker/{workerId}")]
        public HttpResponseMessage UpdateTeamLeaderToWorker([FromUri]int workerId, [FromBody]int teamLeaderId)
        {
            return (LogicManager.UpdateTeamLeaderToWorker(workerId, teamLeaderId)) ?
                new HttpResponseMessage(HttpStatusCode.OK) :
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<String>("Can not update in DB", new JsonMediaTypeFormatter())
                };
        }

    }
}