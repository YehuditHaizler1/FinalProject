using BOL;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Server_WebApi.Controllers
{
    public class ManagerController : Controller
    {
        [HttpPost]
        [Route("api/manager/AddProject")]

        public HttpResponseMessage AddUser(Project newProject)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    LogicManager.AddWorker()
                }
                else
                {
                    List<string> ErrorList = new List<string>();

                }


            }
            catch (Exception)
            {

                return Response.create
            }
        }
    }
}