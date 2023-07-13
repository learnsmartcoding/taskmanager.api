using LearnSmartCoding.CosmosDb.Linq.API.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace LearnSmartCoding.CosmosDb.Linq.API.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Container _taskContainer;
        private readonly IConfiguration configuration;

        public TaskRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;

            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var taskContainerName = "Tasks";
            _taskContainer = cosmosClient.GetContainer(databaseName, taskContainerName);
        }

        public async Task<IEnumerable<TasksDocument>> GetAllTasksForUserAsync(string userId)
        {
            var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
                .Where(t => t.UserId == userId)
                .ToFeedIterator();

            var tasks = new List<TasksDocument>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                tasks.AddRange(response);
            }

            return tasks;
        }


        public async Task<TasksDocument> GetTaskByIdAsync(string taskId, string userId)
        {
            //var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
            //    .Where(t => t.Id == taskId && t.UserId == userId)
            //    .Take(1)
            //    .ToFeedIterator();

            //var response = await query.ReadNextAsync();
            //return response.FirstOrDefault();

            var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
            .Where(t => t.Id == taskId && t.UserId == userId)
            .Take(1)
            .ToQueryDefinition();

            var sqlQuery = query.QueryText; // Retrieve the SQL query

            var response = await _taskContainer.GetItemQueryIterator<TasksDocument>(query).ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<IEnumerable<TasksDocument>> GetAllTasksAsync(string userId)
        {
            var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
                .Where(t => t.UserId == userId)
                .ToFeedIterator();

            var tasks = new List<TasksDocument>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                tasks.AddRange(response);
            }

            return tasks;
        }

        public async Task<TasksDocument> CreateTaskAsync(TasksDocument task)
        {
            var response = await _taskContainer.CreateItemAsync(task);
            return response.Resource;
        }

        public async Task<TasksDocument> UpdateTaskAsync(TasksDocument task)
        {
            var response = await _taskContainer.ReplaceItemAsync(task, task.Id);
            return response.Resource;
        }

        public async Task DeleteTaskAsync(string taskId, string userId)
        {
            await _taskContainer.DeleteItemAsync<TasksDocument>(taskId, new PartitionKey(userId));
        }

        public async Task<TasksDocument> UpdateSubtaskStatusAsync(string taskId, string subtaskId, string status)
        {
            var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
                .Where(t => t.Id == taskId)
                .Take(1)
                .ToFeedIterator();

            var response = await query.ReadNextAsync();
            var task = response.FirstOrDefault();

            if (task == null)
            {
                return null;
            }

            var subtask = task.Subtasks.FirstOrDefault(s => s.Id == subtaskId);
            if (subtask == null)
            {
                return null;
            }

            subtask.Status = status;

            await _taskContainer.ReplaceItemAsync(task, task.Id);

            return task;
        }

        public async Task<TasksDocument> UpdateAttachmentsAsync(string taskId, List<Attachment> attachmentsToAdd, List<string> attachmentIdsToDelete)
        {
            var query = _taskContainer.GetItemLinqQueryable<TasksDocument>()
                .Where(t => t.Id == taskId)
                .Take(1)
                .ToFeedIterator();

            var response = await query.ReadNextAsync();
            var task = response.FirstOrDefault();

            if (task == null)
            {
                return null;
            }

            // Add new attachments
            if (attachmentsToAdd != null)
            {
                task.Attachments.AddRange(attachmentsToAdd);
            }

            // Delete attachments
            if (attachmentIdsToDelete != null)
            {
                task.Attachments.RemoveAll(a => attachmentIdsToDelete.Contains(a.Id));
            }

            await _taskContainer.ReplaceItemAsync(task, task.Id);

            return task;
        }

    }
}
