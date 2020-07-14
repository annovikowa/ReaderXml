﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConverterXlsx.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("test")]
        public IHttpActionResult Get()
        {
            return Ok(nameof(TestController));
        }
    }
}
