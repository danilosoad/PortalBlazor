using Microsoft.AspNetCore.Identity;
using PortalBlazor.Core.Domain.Entity;

namespace PortalBlazor.Infra.Data.Repository
{
    public interface IUserRepository
    {
        Task<bool> DeleteUser(Guid id);

        Task<User> GetUser();

        Task<IEnumerable<IdentityUser>> GetUsers();

        Task<bool> UpdateUserRole(Guid id, User user);
    }
}