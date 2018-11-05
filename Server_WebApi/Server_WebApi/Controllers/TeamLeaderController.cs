using BLL;
using BOL;
using BOL.Help;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Server_WebApi.Controllers
{
    public class TeamLeaderController : ApiController
    {
        //curl -X GET -v http://localhost:60828/api/TeamLeader/GetPresencesToTeamLeader/2
        //GetPresencesToTeamLeader - get projectManagerId, return all the presences to workers that are working in his staff
        [HttpGet]
        [Route("api/TeamLeader/GetPresencesToTeamLeader/{projectManagerId}")]
        public HttpResponseMessage GetPresencesToTeamLeader([FromUri]int projectManagerId)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Presence>>(LogicTeamLeader.GetPresencesToTeamLeader(projectManagerId), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/TeamLeader/GetProjectsToTeamLeader/2
        //GetProjectsToTeamLeader - Get team-leader-id, Return the projects he manages
        [HttpGet]
        [Route("api/TeamLeader/GetProjectsToTeamLeader/{teamLeaderId}")]
        public HttpResponseMessage GetProjectsToTeamLeader([FromUri]int teamLeaderId)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Project>>(LogicTeamLeader.GetProjectsToTeamLeader(teamLeaderId), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/TeamLeader/GetWorkersToTeamLeader/2
        //GetWorkersToTeamLeader - Get team-leader-id, Return the workers he manages
        [HttpGet]
        [Route("api/TeamLeader/GetWorkersToTeamLeader/{teamLeaderId}")]
        public HttpResponseMessage GetWorkersToTeamLeader([FromUri]int teamLeaderId)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Worker>>(LogicTeamLeader.GetWorkersToTeamLeader(teamLeaderId), new JsonMediaTypeFormatter())
            };
        }

        //curl -X GET -v http://localhost:60828/api/User/GetHoursStatusToProject/13
        //GetHoursStatusToProject - Get projectId, Return its hoursStatus. Each record contains the following columns:
        //workerId, projectId, RequiredHours, ActualHours.
        [HttpGet]
        [Route("api/TeamLeader/GetHoursStatusToProject/{projectId}")]
        public HttpResponseMessage GetHoursStatusToProject([FromUri]int projectId)
        {
            List<HoursStatus> workerProjects;

            try
            {
                workerProjects = LogicTeamLeader.GetHoursStatusToProject(projectId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, workerProjects);
        }

    }
}