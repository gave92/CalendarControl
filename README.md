# CalendarControl: Event calendar for UWP

![logo](https://img.shields.io/badge/license-MIT-blue.svg)

Calendar custom control for UWP platform. Supports week view and events.

## Installation

$ Install-Package Gave.Libs.CalendarControl

## Key features
* **Easy to use** (one XAML line for calendar and built-in event management)
* **Responsive** (the number of days shown is adjusted when resizing the window)
* **Extensible** (you can modify look and functionality)

## Quick guide

#### 1. Basic usage

```xml
<Page 
      xmlns:calendar="using:CalendarControl">
  
  <Grid>
    <calendar:Calendar Padding="16,0,16,16"
                       CanvasMinHeight="500"
                       CanvasMaxHeight="600" />
  </Grid>
</Page
```

The calendar default behaviour is to stretch to fill the available space both horizontally and vertically. The height can be limited using the ``CanvasMaxHeight`` property. If you set the ``CanvasMinHeight`` the control will use scrollbars when the available space is less than the value.

#### 2. Custom look

The control exposes a number of properties, e.g. ``HeaderTemplate`` and ``DayHeaderTemplate``, to customize the calendar appearance.
You can find the default styles [here](https://github.com/gave92/CalendarControl/tree/master/CalendarControl/Themes).
Also look [here](https://github.com/gave92/CalendarControl/tree/master/CalendarControl/Themes/Colors.xaml) for the x:Keys to override calendar colors and more.

#### 3. Custom behaviour

```xml
<calendar:Calendar IsHourSelectionEnabled="True"
                   ViewModel="{x:Bind CalendarModel}" />
```

You can set the Calendar's ``ViewModel`` property to change the behaviour of the control. The ``ViewModel`` property must implement ``CalendarControl.Interfaces.ICalendarViewModel``. You can also inherit from the default implementation ``CalendarControl.ViewModels.CalendarViewModel``.

###### Change header buttons behaviour

The control has embedded buttons for changing days and creating/deleting an event. By default you can click and drag to select a range of hours in a day and use the header buttons to add an event. You can override this behaviour, e.g. you can show a dialog to allow the user to specify the Event name.

```cs
using CalendarControl.ViewModels;
using CalendarControl.Models;
using CalendarControl.Mvvm;

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
```

###### Customize the event management

By default the events are saved in the "Events.txt" file in the LocalFolder as a JSON string. You can provide a custom implementation of the ``CalendarControl.Interfaces.IEventManager`` interface and pass it to the ``CalendarViewModel`` constructor.

```cs
public class CustomCalendarModel : CalendarViewModel
  {
        // CustomEventManager that implements the IEventManager interface
        public CustomCalendarModel() : base(eventManager: new CustomEventManager())
        {

        }
  }
```

You can look [here](https://github.com/gave92/CalendarControl/blob/master/CalendarControl/Models/LocalEventManager.cs) for the default implementation of the IEventManager interface.

```cs
public interface IEventManager
    {
        Task AddEventAsync(Event ev);
        Task RemoveEventAsync(Event ev);
        Task RemoveEventsAsync(Func<Event, bool> predicate);
        
        // Returns a DailyEventManager containing the event list for "date"
        DailyEventManager ForDay(DateTimeOffset date);
    }
```

## Supported SDKs
* Creators Update (15063)
* ...
* May 2020 Update (19041)

## Screenshots

<img src="https://github.com/gave92/CalendarControl/blob/master/Screenshots/Calendar_1.png?raw=true" width="750" />

© Copyright 2017 - 2021 by Marco Gavelli
