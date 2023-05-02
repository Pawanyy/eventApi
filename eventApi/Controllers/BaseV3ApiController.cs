using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eventApi.Controllers
{
    [ApiController]
    [ApiVersion("3.0")]  
    [Route("api/v{v:apiVersion}/[controller]")]
    public class BaseV3ApiController : ControllerBase
    {
        
    }
}