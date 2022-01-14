using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration; //for Config
using Tally.Models;

namespace Tally.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly IConfiguration _config;
        public APIController(IConfiguration config){
            _config = config;
        }

        [HttpPost]
        [Route("Data/test")]
        public string test(param data)
        {
            return data.msg;
        }
    }
}
