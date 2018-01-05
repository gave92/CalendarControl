using System;
using System.Threading.Tasks;

namespace CalendarControl
{
    public interface IEventManager
    {
        Task AddEventAsync(Event ev);
        Task RemoveEventAsync(Event ev);
        Task RemoveEventsAsync(Func<Event, bool> predicate);
        DailyEventManager ForDay(DateTimeOffset date);
    }    
}
