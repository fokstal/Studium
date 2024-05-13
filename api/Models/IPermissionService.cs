namespace api.Models
{
    public interface IPermissionService
    {
        Task<HashSet<Helpers.Enums.Permission>> GetPermissionListAsync(int userId);
    }
}