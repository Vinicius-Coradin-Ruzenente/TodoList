using TodoList.Model;

namespace TodoList.Services
{
    public interface IUserServices
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest request);
        Task<List<User>> GetUsers(bool? IsActive);
        Task<User?> GetUserById(int id);
        Task<User?> CreateUser(AddUpdateUser userObj);
        Task<User?> UpdateUser(int id, AddUpdateUser userObj);
        Task<bool> DeleteUserById(int id);
    }
}
