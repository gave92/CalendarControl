using CalendarControl.Models;
using System;
using System.Threading.Tasks;

namespace CalendarControl.Interfaces
{
    public interface IEventManager
    {
        Task AddEventAsync(Event ev);
        Task RemoveEventAsync(Event ev);
        Task RemoveEventsAsync(Func<Event, bool> predicate);
        DailyEventManager ForDay(DateTimeOffset date);
    }    
}
