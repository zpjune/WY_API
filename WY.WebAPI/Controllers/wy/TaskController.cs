using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UIDP.BIZModule.wy;

namespace WY.WebAPI.Controllers.wy
{
    [Produces("application/json")]
    [Route("Task")]
    public class TaskController : Controller
    {
        TaskModule TM = new TaskModule();
        [HttpGet("GetTaskInfo")]
        public IActionResult GetTaskInfo(string RWBH, string RWMC, int page, int limit) => Ok(TM.GetTaskInfo(RWBH, RWMC, page, limit));
        [HttpGet("GetPlanCheckAndDetail")]
        public IActionResult GetPlanCheckAndDetail(string TASK_ID) => Ok(TM.GetPlanCheckAndDetail(TASK_ID));

        [HttpPost("CreateTask")]
        public IActionResult CreateTask([FromBody]JObject value) => Ok(TM.CreateTask(value.ToObject<Dictionary<string, object>>()));

        
        [HttpPost("UpdateTask")]
        public IActionResult UpdateTask([FromBody]JObject value) => Ok(TM.UpdateTask(value.ToObject<Dictionary<string, object>>()));

        [HttpGet("DeleteTask")]
        public IActionResult DeleteTask(string TASK_ID) => Ok(TM.DeleteTask(TASK_ID));
    }
}