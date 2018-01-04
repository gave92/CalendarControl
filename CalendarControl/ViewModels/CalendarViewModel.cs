using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Mvvm;
using Windows.Foundation;

namespace CalendarControl
{
    public class CalendarViewModel : BindableBase, ICalendarViewModel
    {
        public string Year
        {
            get { return Days.FirstOrDefault()?.Year; }
        }

        public string Month
        {
            get { return Days.FirstOrDefault()?.Month; }
        }

        private ObservableCollection<Day> _days;
        public ObservableCollection<Day> Days
        {
            get { return _days; }
            set
            {
                if (_days != null)
                    _days.CollectionChanged -= DaysChanged;
                Set(ref _days, value);
                if (_days != null)
                    _days.CollectionChanged += DaysChanged;
            }
        }

        private int _numberOfDays;
        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                if (_numberOfDays != value)
                {
                    _numberOfDays = value;
                    base.RaisePropertyChanged();
                    UpdateNumberOfDays();
                }
            }
        }

        private void DaysChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.RaisePropertyChanged("Month");
            base.RaisePropertyChanged("Year");
        }

        public CalendarViewModel(DateTimeOffset? _startDate = null)
        {
            this.Days = new ObservableCollection<Day>();
            var startDate = _startDate?.Date ?? DateTimeOffset.Now.Date;
            this.Days.Add(new Day(startDate));
        }

        public void SizeChanged(Size size)
        {
            if (size.Width > 700)
            {
                NumberOfDays = 7;
            }
            else if (size.Width > 400)
            {
                NumberOfDays = 3;
            }
            else
            {
                NumberOfDays = 1;
            }
        }

        private void UpdateNumberOfDays()
        {
            var startDate = Days.Last().Date.Date;

            while (NumberOfDays > Days.Count)
            {
                startDate = startDate.AddDays(1);
                this.Days.Add(new Day(startDate));
            }
            while (NumberOfDays < Days.Count)
            {
                this.Days.RemoveAt(Days.Count - 1);
            }
        }

        private DelegateCommand _addEventCommand;
        public DelegateCommand AddEventCommand => _addEventCommand ?? (_addEventCommand = new DelegateCommand(AddEvent));

        private void AddEvent()
        {
            var day = Days.FirstOrDefault(d => d.Hours.Any(h => h.IsSelected));
            if (day == null) return;
            var begin = day.Hours.First(h => h.IsSelected);
            var end = day.Hours.Last(h => h.IsSelected);
            EventManager.Instance.AddEvent(new Event(begin.Time, end.Time.AddHours(1), "New event"));
            ((List<Hour>)day.Hours).ForEach(h => h.IsSelected = false);
        }

        private DelegateCommand _removeEventCommand;
        public DelegateCommand RemoveEventCommand => _removeEventCommand ?? (_removeEventCommand = new DelegateCommand(RemoveEvent));

        private void RemoveEvent()
        {
            EventManager.Instance.RemoveSelectedEvents();
        }

        private DelegateCommand<int> _shiftDaysCommand;
        public DelegateCommand<int> ShiftDaysCommand => _shiftDaysCommand ?? (_shiftDaysCommand = new DelegateCommand<int>(ShiftDays));

        private void ShiftDays(int offset)
        {
            while (offset > 0)
            {
                var day = new Day(Days.Last().Date.AddDays(1));
                this.Days.Add(day);
                this.Days.RemoveAt(0);
                offset--;
            }
            while (offset < 0)
            {
                var day = new Day(Days.First().Date.AddDays(-1));
                this.Days.Insert(0, day);
                this.Days.RemoveAt(Days.Count - 1);
                offset++;
            }
        }
    }
}
