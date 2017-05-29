using System;

namespace WebSite.Services
{
    public interface IJobService
    {
        void ScheduleAuctionEnd(DateTime dueDate, Guid itemId);
    }
}
