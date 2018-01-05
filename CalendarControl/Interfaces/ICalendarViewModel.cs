using CalendarControl.Models;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace CalendarControl.Interfaces
{
    public interface ICalendarViewModel
    {
        IEventManager EventManager { get; set; }
        ObservableCollection<Day> Days { get; set; }
        void SizeChanged(Size size);
    }
}
