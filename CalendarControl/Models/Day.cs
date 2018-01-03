using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Template10.Mvvm;
using Windows.System.UserProfile;

namespace CalendarControl
{
    public class Day : BindableBase
    {
        public string DayOfWeek
        {
            get { return Date.ToString("ddd", Culture); }
        }

        public string DayOfYear
        {
            get { return Date.ToString("dd", Culture); }
        }

        public string Month
        {
            get { return Date.ToString("MMMM", Culture); }
        }

        public string Year
        {
            get { return Date.ToString("yyyy", Culture); }
        }

        public bool IsToday
        {
            get { return Date.Date == DateTimeOffset.Now.Date; }
        }

        private DateTimeOffset _date;
        public DateTimeOffset Date
        {
            get { return _date; }
            set
            {
                if (_date.CompareTo(value) != 0)
                {
                    _date = value;
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged("IsToday");
                    base.RaisePropertyChanged("DayOfWeek");
                    base.RaisePropertyChanged("DayOfYear");
                    base.RaisePropertyChanged("Month");
                    base.RaisePropertyChanged("Year");
                }                
            }
        }

        public IList<Hour> Hours { get; set; }

        public ObservableCollection<Event> Events { get; set; }

        private readonly CultureInfo Culture;

        public Day(DateTimeOffset date, int startH = 0, int endH = 23)
        {
            this.Date = date;
            this.Culture = new CultureInfo(GlobalizationPreferences.Languages[0]);

            this.Events = new ObservableCollection<Event>();

            this.Hours = new List<Hour>();
            for (var h = startH; h <= endH; h++)
            {
                Hours.Add(new Hour(date.Date.AddHours(h)));
            }
        }        
    }
}
