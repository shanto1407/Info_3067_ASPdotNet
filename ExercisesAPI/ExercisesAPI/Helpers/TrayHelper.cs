using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExercisesAPI.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrayHelper
    {
        public string email { get; set; }
        public TraySelectionHelper[] selections { get; set; }
    }

}
