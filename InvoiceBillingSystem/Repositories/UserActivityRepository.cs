using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class UserActivityRepository:IUserActivityRepository
    {
        private readonly ApplicationDbContext _Context;
        public UserActivityRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task LogActivityAsync(UserActivity activity)
        {
            await _Context.UserActivities.AddAsync(activity);
            await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserActivity>> GetUserActivitiesAsync(Guid userId)
        {
            return await _Context.UserActivities
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.ActivityTime)
                .ToListAsync();
        }

        public async Task<UserActivity?> GetLastActivityByUserAsync(Guid userId)
        {
            return await _Context.UserActivities
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ActivityTime)
                .FirstOrDefaultAsync();
        }

        //public async Task UpdateActivityAsync(UserActivity activity)
        //{
        //    _Context.UserActivities.Update(activity);
        //    await _Context.SaveChangesAsync();
        //}

        public async Task UpdateActivityAsync(UserActivity activity)
        {
            activity.ActivityTime = DateTime.SpecifyKind(activity.ActivityTime, DateTimeKind.Utc);

            if (activity.ActivityEndTime.HasValue)
            {
                activity.ActivityEndTime = DateTime.SpecifyKind(activity.ActivityEndTime.Value, DateTimeKind.Utc);
            }

            _Context.UserActivities.Update(activity);
            await _Context.SaveChangesAsync();
        }



    }
}
