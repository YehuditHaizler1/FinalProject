using BLL;
using BOL;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Server_WebApi.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/Users
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<List<Worker>>(LogicManager.GetAllWorkers(), new JsonMediaTypeFormatter())
            };
        }

        // GET: api/Users/5
        //public HttpResponseMessage Get(int id)
        //{
        //    return new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent<String>(LogicManager.GetUserName(id), new JsonMediaTypeFormatter())
        //    };
        //}

        // POST: api/Users
        //public HttpResponseMessage Post([FromBody]User value)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return (LogicManager.AddUser(value)) ?
        //           new HttpResponseMessage(HttpStatusCode.Created) :
        //           new HttpResponseMessage(HttpStatusCode.BadRequest)
        //           {
        //               Content = new ObjectContent<String>("Can not add to DB", new JsonMediaTypeFormatter())
        //           };
        //    };

        //    List<string> ErrorList = new List<string>();

        //    //if the code reached this part - the user is not valid
        //    foreach (var item in ModelState.Values)
        //        foreach (var err in item.Errors)
        //            ErrorList.Add(err.ErrorMessage);

        //    return new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        Content = new ObjectContent<List<string>>(ErrorList, new JsonMediaTypeFormatter())
        //    };

        //}

        //// PUT: api/Users/5
        //public HttpResponseMessage Put([FromBody]User value)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        return (LogicManager.UpdateUser(value)) ?
        //            new HttpResponseMessage(HttpStatusCode.OK) :
        //            new HttpResponseMessage(HttpStatusCode.BadRequest)
        //            {
        //                Content = new ObjectContent<String>("Can not update in DB", new JsonMediaTypeFormatter())
        //            };
        //    };

        //    List<string> ErrorList = new List<string>();

        //    //if the code reached this part - the user is not valid
        //    foreach (var item in ModelState.Values)
        //        foreach (var err in item.Errors)
        //            ErrorList.Add(err.ErrorMessage);

        //    return new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        Content = new ObjectContent<List<string>>(ErrorList, new JsonMediaTypeFormatter())
        //    };
        //}

        //// DELETE: api/Users/5
        //public HttpResponseMessage Delete(int id)
        //{
        //    return (LogicManager.RemoveUser(id)) ?
        //            new HttpResponseMessage(HttpStatusCode.OK) :
        //            new HttpResponseMessage(HttpStatusCode.BadRequest)
        //            {
        //                Content = new ObjectContent<String>("Can not remove from DB", new JsonMediaTypeFormatter())
        //            };
        //}
    }
}