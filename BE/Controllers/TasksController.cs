using BE.Data.Contexts;
using BE.Data.Dtos;
using BE.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BE.Data.Enums;
using System.Globalization;

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
                var result = await _context.tasks.ToListAsync();
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
                var result = await _context.tasks.Where(t => t.idProject == idProject).ToListAsync();
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
                var result = await _context.tasks.Where(t => t.assignee == idAssignee).ToListAsync();
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
                var result = await _context.tasks.Where(t => t.startTaskDate <= time && t.endTaskDate >= time).ToListAsync();
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
                var result = await _context.tasks.Where(t => t.endTaskDate < DateTime.Now).ToListAsync();
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
                var result = await _context.tasks.Where(t => t.isDeleted == true).ToListAsync();
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
        [HttpGet("getTaskById")]
        public async Task<IActionResult> getTaskById(int id)
        {
            try
            {
                var result = await _context.tasks.SingleOrDefaultAsync(t => t.idTask == id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getTaskChild")]
        public async Task<ActionResult<List<Tasks>>> getTaskChild(int id)
        {
            try
            {
                var result = await _context.tasks.Where(t => t.idParent == id).ToListAsync();
                return Ok(result.ToList());
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
                    status = task.status,
                    tag = task.tags,
                    assignee = task.assignee,
                    status = task.status,
                    tag = task.tags,
                    milestone = task.milestone,
                    startTaskDate = task.startTaskDate,
                    endTaskDate = task.endTaskDate,
                    createUser = task.createUser,
                    idProject = task.idProject
                };
                _context.tasks.Add(result);
                //await _context.SaveChangesAsync();
                return Ok("Create task success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpPut("editTask/{idTask}")]
        public async Task<ActionResult> editTask(int idTask, TaskEditDto task)
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
        public async Task<ActionResult> deletedTask( int id)
        {
            try
            {
                var resultChilds = await _context.tasks.Where(t => t.idParent == id).ToListAsync();
                if (!resultChilds.Any())
                {
                    var result = await _context.tasks.FindAsync(id);
                    result.isDeleted = true;
                    await _context.SaveChangesAsync();
                    return Ok("Remove task success");
                }
                return BadRequest("Task have child. Don't remove");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getTaskParent")]
        public async Task<IActionResult> getTaskParent(int id)
        {
            try
            {
                var task = await _context.tasks.SingleOrDefaultAsync(h => h.idTask == id);
                var resutl = await _context.tasks.FirstOrDefaultAsync(t => t.idTask == task.idParent);
                return Ok(resutl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getTasksByTag")]
        public async Task<ActionResult<List<Tasks>>> getTasksByTagAsync(Tags tag)
        {
            try
            {
                var resutl = await _context.tasks.Where(t => t.tag == tag).ToListAsync();
                return Ok(resutl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getDaysCompletedAsync")]
        public async Task<ActionResult> getDaysCompletedAsync(int id)
        {
            try
            {
                var task = await _context.tasks.SingleOrDefaultAsync(h => h.idTask == id);

                if (task != null)
                {

                    System.TimeSpan result = (TimeSpan)(task.endTaskDate - task.startTaskDate);
                    int day = result.Days;
                    return Ok(day);
                }
                return NotFound("Task empty!!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getTasksStillDealine")]
        public async Task<ActionResult<List<Tasks>>> getTasksStillDealine()
        {
            try
            {
                var result = await _context.tasks.Where(h => DateTime.Now <= h.endTaskDate).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
    }
}
