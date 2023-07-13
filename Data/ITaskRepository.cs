using LearnSmartCoding.CosmosDb.Linq.API.Core;

namespace LearnSmartCoding.CosmosDb.Linq.API.Data
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TasksDocument>> GetAllTasksForUserAsync(string userId);
        Task<TasksDocument> CreateTaskAsync(TasksDocument task);
        Task DeleteTaskAsync(string taskId, string userId);
        Task<IEnumerable<TasksDocument>> GetAllTasksAsync(string userId);
        Task<TasksDocument> GetTaskByIdAsync(string taskId, string userId);
        Task<TasksDocument> UpdateAttachmentsAsync(string taskId, List<Attachment> attachmentsToAdd, List<string> attachmentIdsToDelete);
        Task<TasksDocument> UpdateSubtaskStatusAsync(string taskId, string subtaskId, string status);
        Task<TasksDocument> UpdateTaskAsync(TasksDocument task);
    }
}