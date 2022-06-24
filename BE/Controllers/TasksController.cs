using BE.Data.Dtos;
using BE.Services.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        #region Property
        private readonly ITasksManager _tasksManager;
        private readonly ILogger<TasksController> _logger;
        #endregion

        #region Constructor
        public TasksController(ITasksManager tasksManager, ILogger<TasksController> logger)
        {
            _tasksManager = tasksManager;
            _logger = logger;
        }
        #endregion

        [HttpGet("getAll")]
        public async Task<IActionResult> getAllAsync()
        {
            try
            {
                return Ok(await _tasksManager.GetAllTasksAsync());
            }
            catch(Exception ex)
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
                return Ok(await _tasksManager.getTaskByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpGet("getTaskChild")]
        public async Task<IActionResult> getTaskChild(int id)
        {
            try
            {
                return Ok(await _tasksManager.getTaskChild(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }

        [HttpPost("addTask")]
        public async Task<IActionResult> addTaskAsync(TaskDto task)
        {
            try
            {
                await _tasksManager.AddTask(task);
                return Ok();
            }
            catch(Exception ex)
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
                return Ok(await _tasksManager.getTaskParent(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
        [HttpPut("DeleteTaskById")]
        public async Task<IActionResult> DeleteTaskById(int id)
        {
            try
            {
                return Ok(await _tasksManager.getTaskParent(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        }
    }
}
