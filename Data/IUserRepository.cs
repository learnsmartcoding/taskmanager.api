using LearnSmartCoding.CosmosDb.Linq.API.Core;

namespace LearnSmartCoding.CosmosDb.Linq.API.Data
{
    public interface IUserRepository
    {
        Task<UserDocument> CreateUserAsync(UserDocument user);
        Task DeleteUserAsync(string userId);
        Task<UserDocument> GetUserByIdAsync(string userId);
        Task<UserDocument> UpdateUserAsync(UserDocument user);
    }
}