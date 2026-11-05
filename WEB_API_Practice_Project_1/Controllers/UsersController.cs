using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WEB_API_Practice_Project_1.Models.BEL;
using WEB_API_Practice_Project_1.Models.BLL;

namespace WEB_API_Practice_Project_1.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly QueryUserService _userService = new QueryUserService();

        // GET: /api/users/list
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(List<QueryUser>))]
        public IHttpActionResult GetUsers()
        {
            try
            {
                List<QueryUser> users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: /api/users/list/{id}
        [HttpGet]
        [Route("list/{id:int}")]
        [ResponseType(typeof(QueryUser))]
        public IHttpActionResult GetUserById(int id)
        {
            try
            {
                QueryUser user = _userService.GetUserById(id);
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = $"User with ID {id} not found." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: /api/users/create
        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(object))]
        public IHttpActionResult CreateUser([FromBody] QueryUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is required.");
                }

                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("UserName and Email are required fields.");
                }

                string result = _userService.InsertUser(user);
                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }

                return Content(HttpStatusCode.Created, new { message = result });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: /api/users/update/{id}
        [HttpPut]
        [Route("update/{id:int}")]
        [ResponseType(typeof(object))]
        public IHttpActionResult UpdateUser(int id, [FromBody] QueryUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is required.");
                }

                string result = _userService.UpdateUser(id, user);
                if (result.Contains("not found"))
                {
                    return Content(HttpStatusCode.NotFound, new { message = result });
                }
                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: /api/users/delete/{id}
        [HttpDelete]
        [Route("delete/{id:int}")]
        [ResponseType(typeof(object))]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                string result = _userService.DeleteUser(id);
                if (result.Contains("not found"))
                {
                    return Content(HttpStatusCode.NotFound, new { message = result });
                }
                if (result.StartsWith("Error"))
                {
                    return BadRequest(result);
                }

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
