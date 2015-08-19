using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Who.Models;

namespace Who.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        public IEnumerable<User> GetUsers(string searchText)
        {
            Users u = new Users(searchText);
            return u.UserList;
        }
    }
}
