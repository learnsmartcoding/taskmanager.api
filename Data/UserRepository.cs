using LearnSmartCoding.CosmosDb.Linq.API.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace LearnSmartCoding.CosmosDb.Linq.API.Data
{

    public class UserRepository : IUserRepository
    {
        private readonly Container _userContainer;
        private readonly IConfiguration configuration;

        public UserRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var userContainerName = "Users";
            _userContainer = cosmosClient.GetContainer(databaseName, userContainerName);            
        }

        public async Task<UserDocument> GetUserByIdAsync(string userId)
        {
            var query = _userContainer.GetItemLinqQueryable<UserDocument>()
                .Where(u => u.Id == userId)
                .Take(1)
                .ToFeedIterator();

            var response = await query.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task<UserDocument> CreateUserAsync(UserDocument user)
        {
            var response = await _userContainer.CreateItemAsync(user);
            return response.Resource;
        }

        public async Task<UserDocument> UpdateUserAsync(UserDocument user)
        {
            var response = await _userContainer.ReplaceItemAsync(user, user.Id);
            return response.Resource;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userContainer.DeleteItemAsync<UserDocument>(userId, new PartitionKey(userId));
        }
    }
}
