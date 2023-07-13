using LearnSmartCoding.CosmosDb.Linq.API.Core;
using LearnSmartCoding.CosmosDb.Linq.API.Data;
using LearnSmartCoding.CosmosDb.Linq.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace LearnSmartCoding.CosmosDb.Linq.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TasksDocument>> GetTask(string taskId, [FromQuery] string userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpGet("user/{userId}/tasks")]
        public async Task<ActionResult<IEnumerable<TasksDocument>>> GetAllTasksForUser(string userId)
        {
            var tasks = await _taskRepository.GetAllTasksForUserAsync(userId);
            if (tasks == null)// || !tasks.Any())
            {
                return NotFound();
            }

            return Ok(tasks);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TasksDocument>>> GetAllTasks(string userId)
        {
            var tasks = await _taskRepository.GetAllTasksAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TasksDocument>> CreateTask(TasksDocument task)
        {
            // Set any additional properties if required
            task.Id = Guid.NewGuid().ToString();
            task.TaskId = task.Id;

            task.Subtasks.ForEach(s =>
            {
                if (string.IsNullOrEmpty(s.Id))
                {
                    s.Id = Guid.NewGuid().ToString();
                }
            });
            var createdTask = await _taskRepository.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { taskId = createdTask.Id, userId = createdTask.UserId }, createdTask);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<TasksDocument>> UpdateTask(string taskId, TasksDocument task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, task.UserId);
            if (existingTask == null)
            {
                return NotFound();
            }

            task.Id = existingTask.Id; // Preserve the original ID
            task.TaskId = existingTask.TaskId;

          var updatedTask = await _taskRepository.UpdateTaskAsync(task);
            return Ok(updatedTask);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId, [FromQuery] string userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                return NotFound();
            }

            await _taskRepository.DeleteTaskAsync(taskId, userId);
            return NoContent();
        }

        [HttpPut("{taskId}/subtasks/{subtaskId}/status")]
        public async Task<ActionResult<TasksDocument>> UpdateSubtaskStatus(string taskId, string subtaskId, 
            [FromBody] SubtaskUpdateRequest request)
        {
            var updatedTask = await _taskRepository.UpdateSubtaskStatusAsync(taskId, subtaskId, request.Status);
            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }

        [HttpPut("{taskId}/attachments")]
        public async Task<ActionResult<TasksDocument>> UpdateAttachments(string taskId, [FromBody] AttachmentsUpdateRequest request)
        {
            var updatedTask = await _taskRepository.UpdateAttachmentsAsync(taskId, request.AttachmentsToAdd, request.AttachmentsToDelete);
            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }

    }
}
