using BLL;
using BOL;
using BOL.Help;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Web.Http;

namespace Server_WebApi.Controllers
{
    public class UserController : ApiController
    {
        //curl -v -X POST -H "Content-type: application/json" -d "{\"Password\":\"12345\",\"EMail\":\"esty@gmail.com\"}"  http://localhost:60828/api/User/Login

        // POST: api/Users
        [HttpPost]
        [Route("api/User/Login")]
        public HttpResponseMessage Login([FromBody]Login userLogin)
        {
            Worker worker;

            try
            {
                worker = LogicUser.Login(userLogin.EMail, userLogin.Password);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK, worker);
        }

        //???????????????????????????????????יש לתקן את ההערה!!!!!!!!!!!
        // GetProjectsToWorker - Get workerId, Return Dictionary<int, string> where key is projectId and value is projectName
        [HttpGet]
        [Route("api/User/GetProjectsToWorker/{workerId}")]
        public HttpResponseMessage GetProjectsToWorker([FromUri]int workerId)
        {
            List<Project> workerProjects;

            try
            {
                workerProjects = LogicUser.GetProjectsToWorker(workerId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, workerProjects);
        }

        //AddPresence - Get presence's details ,Add it to the D.B., Return the presenceId
        [HttpPost]
        [Route("api/User/AddPresence")]
        public HttpResponseMessage AddPresence([FromBody]Presence newPresence)
        {
            if (ModelState.IsValid)
            {
                int presenceId;
                try
                {
                    if (LogicUser.AddPresence(newPresence))
                    {
                        presenceId = LogicUser.GetCurrentPresenceId(newPresence);
                        return new HttpResponseMessage(HttpStatusCode.Created)
                        {
                            Content = new ObjectContent<Int32>(presenceId, new JsonMediaTypeFormatter())
                        };
                    }
                }
                catch (Exception e)
                {
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<String>($"Can not add to DB - {e.Message}", new JsonMediaTypeFormatter())
                    };
                }
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

        //UpdateEndTime - Get presenceId and endtime, Update in D.B. the endTime in the correct object (by the presenceId)
        [HttpPut]
        [Route("api/User/UpdateEndTime")]
        public HttpResponseMessage UpdateEndTime([FromBody]dynamic endTimeDetails)
        {
            int presenceId = endTimeDetails["presenceId"];
            DateTime endTime = endTimeDetails["endTime"];

            return (LogicUser.UpdateEndTime(presenceId, endTime)) ?
                new HttpResponseMessage(HttpStatusCode.OK) :
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<String>("Can not update in DB", new JsonMediaTypeFormatter())
                };
        }

        [HttpPost]
        [Route("api/User/SendEmailToManager")]
        public HttpResponseMessage SendEmailToManager([FromBody]dynamic data)
        {
            string subject = data["subject"];
            string content = data["cotent"];
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("shtilimrishum2018@gmail.com");
            msg.To.Add(new MailAddress("yehudit1419@gmail.com"));
            //mail subject
            msg.Subject = subject;
            //body
            msg.Body = content;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("shtilimrishum2018@gmail.com", "0504190762");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OK"); ;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message); ;
            }
            finally
            {
                msg.Dispose();
            }

        }

        //curl -X GET -v http://localhost:60828/api/User/GetHoursStatusToWorker/6
        //GetHoursStatusToWorker - Get workerId, Return his hoursStatus. Each record contains the following columns:
        //workerId, projectId, RequiredHours, ActualHours.
        [HttpGet]
        [Route("api/User/GetHoursStatusToWorker/{workerId}")]
        public HttpResponseMessage GetHoursStatusToWorker([FromUri]int workerId)
        {
            List<HoursStatus> workerProjects;

            try
            {
                workerProjects = LogicUser.GetHoursStatusToWorker(workerId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, workerProjects);
        }

    }
}