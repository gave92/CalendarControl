using System.Collections.ObjectModel;
using Windows.Foundation;

namespace CalendarControl
{
    public interface ICalendarViewModel
    {
        ObservableCollection<Day> Days { get; }
        void SizeChanged(Size size);
    }
}
