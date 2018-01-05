using CalendarControl.Mvvm;
using System;
using System.Globalization;
using Windows.System.UserProfile;

namespace CalendarControl.Models
{
    public class Hour : BindableBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        private DateTimeOffset _time;
        public DateTimeOffset Time
        {
            get { return _time; }
            set
            {
                if (_time.CompareTo(value) != 0)
                {
                    _time = value;
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged("IsNow");
                    base.RaisePropertyChanged("IsToday");
                    base.RaisePropertyChanged("IsFuture");
                    base.RaisePropertyChanged("Short");
                }                
            }
        }

        public string Short
        {
            get { return Time.ToString("HH:mm", Culture); }
        }

        public bool IsNow
        {
            get { return Time.Hour == DateTimeOffset.Now.Hour; }
        }

        public bool IsToday
        {
            get { return Time.Date == DateTimeOffset.Now.Date; }
        }

        public bool IsFuture
        {
            get { return Time.CompareTo(DateTimeOffset.Now) > 0; }
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        private readonly CultureInfo Culture;

        public Hour(DateTimeOffset date)
        {
            this.Time = date;
            this.Culture = new CultureInfo(GlobalizationPreferences.Languages[0]);
        }

        public override bool Equals(object obj)
        {
            switch (obj.GetType().FullName)
            {
                case "CalendarControl.Hour":
                    var hour = obj as Hour;
                    return this.Time.Equals(hour.Time);
                case "System.DateTimeOffset":
                    var dto = (DateTimeOffset)obj;
                    return this.Time.Equals(dto);
                default:
                    return false;
            }                       
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
