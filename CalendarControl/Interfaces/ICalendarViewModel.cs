using CalendarControl.Models;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace CalendarControl.Interfaces
{
    public interface ICalendarViewModel
    {
        ObservableCollection<Day> Days { get; set; }
        void SizeChanged(Size size);
    }
}
