using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockingjayApp.Controllers
{
    [Route("/hello")]
    public class GreetingController
    {
        [HttpGet]
        public IActionResult Get()
        {
            var greeting = "Hello ";
            return new JsonResult(greeting);
        }

        [HttpPost]
        public IActionResult Post([FromBody] string name)
        {
            //Program.Form.NameText = name;
            return new NoContentResult();
        }
    }
}
