using BE.Data.Contexts;
using BE.Data.Dtos;
using BE.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{


    [ApiController]
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        #region Property
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;
        #endregion

        #region Constructor
        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Tasks>>> getAllAsync()
        {
            try
            {
                var result = _context.tasks.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }

        [HttpGet("getAllTask/{idProject}")]
        public async Task<ActionResult<List<Tasks>>> getAllTaskByIdProject(int idProject)
        {
            try
            {
                var result = _context.tasks.Where(t => t.idProject == idProject);
                if(!result.ToList().Any())
                {
                    return NotFound("Don't found task in project!!!");
                }          
                return Ok(result);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getAllTaskAssignee/{idAssignee}")]
        public async Task<ActionResult<List<Tasks>>> getAllTaskByidAssignee(int idAssignee)
        {
            try
            {
                var result = _context.tasks.Where(t => t.assignee == idAssignee);
                if (!result.ToList().Any())
                {
                    return NotFound("Don't found task of Assignee!!!");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getAllTaskByTime")]
        public async Task<ActionResult<List<Tasks>>> getAllTaskByTime(DateTime time)
        {
            try
            {
                var result = _context.tasks.Where(t => t.startTaskDate <= time && t.endTaskDate >= time);
                if (!result.ToList().Any())
                {
                    return NotFound("Don't found task in date!!!");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getAllTaskExpired")]
        public async Task<ActionResult<List<Tasks>>> getAllTaskExpired()
        {
            try
            {
                var result = _context.tasks.Where(t => t.endTaskDate < DateTime.Now);
                if (!result.ToList().Any())
                {
                    return NotFound("Don't found task expired!!!");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getAllActive")]
        public async Task<ActionResult<List<Tasks>>> getAllActive()
        {
            try
            {
                var result = _context.tasks.Where(t => t.isDeleted == true);
                if (result.ToList().Any())
                {
                    return NotFound("Don't found task expired!!!");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpPost("addTask")]
        public async Task<ActionResult<List<TaskDto>>> addTaskAsync(TaskDto task)
        {
            try
            {
                var result = new Tasks
                {
                    idParent = task.idParent,
                    taskName = task.taskName,
                    description = task.description,
                    assignee = task.assignee,
                    milestone = task.milestone,
                    startTaskDate = task.startTaskDate,
                    endTaskDate = task.endTaskDate,
                    createUser = task.createUser,
                    idProject = task.idProject
                };
                _context.tasks.Add(result);
                await _context.SaveChangesAsync();
                return Ok("Create task success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpPut("editTask/{idTask}")]
        public async Task<ActionResult> editTask(int idTask, TaskDto task)
        {
            try
            {
                var result = _context.tasks.SingleOrDefault(h => h.idTask == idTask && h.isDeleted == false);
                if (result != null)
                {
                    result.idParent = task.idParent;
                    result.taskName = task.taskName;
                    result.description = task.description;
                    result.assignee = task.assignee;
                    result.milestone = task.milestone;
                    result.startTaskDate = task.startTaskDate;
                    result.endTaskDate = task.endTaskDate;
                    result.idProject = task.idProject;
                    await _context.SaveChangesAsync();
                    return Ok("Edit task success");
                }
                else
                {
                    return BadRequest("Task is not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpPut("deletedTask/{idTask}")] 
        public async Task<ActionResult> deletedTask(int idTask)
        {
            try
            {
                var result = _context.tasks.SingleOrDefault(h => h.idTask == idTask);
                if (result != null)
                {
                    result.isDeleted = true;
                }
                await _context.SaveChangesAsync();
                return Ok("Deleted task success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
    }
}
