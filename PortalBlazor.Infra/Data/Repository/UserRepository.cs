using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalBlazor.Core.Domain.Entity;

namespace PortalBlazor.Infra.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<bool> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            var result = await _dataContext.Users.ToListAsync();

            return result;
        }

        public Task<bool> UpdateUserRole(Guid id, User user)
        {
            throw new NotImplementedException();
        }
    }
}