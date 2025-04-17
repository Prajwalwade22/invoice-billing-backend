using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IUserActivityRepository
    {
        Task LogActivityAsync(UserActivity activity);
        Task<IEnumerable<UserActivity>> GetUserActivitiesAsync(Guid userId);

        Task<UserActivity?> GetLastActivityByUserAsync(Guid userId);


        Task UpdateActivityAsync(UserActivity activity);
    }
}
