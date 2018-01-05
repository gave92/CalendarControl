using System.Collections.ObjectModel;
using Windows.Foundation;

namespace CalendarControl
{
    public interface ICalendarViewModel
    {
        IEventManager EventManager { get; set; }
        ObservableCollection<Day> Days { get; set; }
        void SizeChanged(Size size);
    }
}
