using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace CalendarControl
{
    public class Event : BindableBase, IComparable<Event>
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private DateTimeOffset _startDate;
        public DateTimeOffset StartDate
        {
            get { return _startDate; }
            set { Set(ref _startDate, value); }
        }

        private DateTimeOffset _endDate;
        public DateTimeOffset EndDate
        {
            get { return _endDate; }
            set { Set(ref _endDate, value); }
        }

        public Brush Color
        {
            get
            {
                return new SolidColorBrush((Color)Application.Current.Resources["EventBackgroundColor"]);
            }
        }

        public IList<Event> Concurrent { get; set; }
        public ISet<Event> GetConcurrent(ISet<Event> set = null)
        {
            if (set == null)
            {
                set = new SortedSet<Event>();
            }

            foreach (var ev in Concurrent)
            {
                if (!set.Contains(ev))
                {
                    set.Add(ev);
                    set.UnionWith(ev.GetConcurrent(set));
                }
            }
            return set;
        }

        [JsonConstructor]
        public Event() { }

        public Event(DateTimeOffset start, DateTimeOffset end, string title)
        {
            this.StartDate = start;
            this.EndDate = end;
            this.Title = title;
            this.ID = Guid.NewGuid().ToString("N");
        }

        public override bool Equals(object obj)
        {
            var ev = obj as Event;
            if (ev == null)
            {
                return false;
            }

            return ev.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        int IComparable<Event>.CompareTo(Event other)
        {
            var compare = StartDate.CompareTo(other.StartDate);
            if (compare == 0)
                compare = EndDate.CompareTo(other.EndDate);
            if (compare == 0)
                compare = ID.CompareTo(other.ID);
            return compare;
        }
    }
}