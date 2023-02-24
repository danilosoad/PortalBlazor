namespace PortalBlazor.Core.Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
    }
}