using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIPractice.Controllers
{
    public class ValuesController : ApiController
    {
        // Static in-memory list to persist CRUD data across HTTP requests
        public static List<string> _values = new List<string> { "Anubhaw", "Komal", "Ovijeet" };
        private static readonly object _lock = new object();

        // GET api/values
        public IEnumerable<string> Get()
        {
            lock (_lock)
            {
                return _values.ToList();
            }
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _values.Count)
                {
                    return NotFound();
                }

                return Ok(_values[id]);
            }
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Value cannot be null or empty.");
            }

            lock (_lock)
            {
                _values.Add(value);
                return Ok(_values.ToList());
            }                          
        }

        // PUT api/values/5
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Value cannot be null or empty.");
            }

            lock (_lock)
            {
                if (id < 0 || id >= _values.Count)
                {
                    return NotFound();
                }

                _values[id] = value;
                return Ok(_values.ToList());
            }
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _values.Count)
                {
                    return NotFound();
                }

                string removedValue = _values[id];
                _values.RemoveAt(id);
                return Ok(new { message = "Deleted successfully", value = removedValue, list = _values.ToList() });
            }
        }
    }
}
