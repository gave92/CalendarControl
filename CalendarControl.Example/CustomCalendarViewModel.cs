using CalendarControl.Models;
using CalendarControl.Mvvm;
using CalendarControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CalendarControl.Example
{
    public class CustomCalendarModel : CalendarViewModel
    {
        public CustomCalendarModel() : base()
        {

        }

        // Override the AddEventCommand command, show a dialog to create an event
        // Called when the user clicks the "plus" button on the calendar header
        private DelegateCommand _addEventCommand;
        public override DelegateCommand AddEventCommand => _addEventCommand ?? (_addEventCommand = new DelegateCommand(AddEvent));

        private async void AddEvent()
        {
            // Find the range of selected hours
            var day = Days.FirstOrDefault(d => d.Hours.Any(h => h.IsSelected));
            if (day == null) return;
            var begin = day.Hours.First(h => h.IsSelected);
            var end = day.Hours.Last(h => h.IsSelected);

            // Custom ContentDialog with a textbox for setting the event name
            DialogEvent dialog = new DialogEvent()
            {
                Event = new Event(begin.Time, end.Time.AddHours(1), "")
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Add event to the calendar               
                await EventManager?.AddEventAsync(dialog.Event);
                // Deselect the time range
                ((List<Hour>)day.Hours).ForEach(h => h.IsSelected = false);
            }
        }
    }
}
