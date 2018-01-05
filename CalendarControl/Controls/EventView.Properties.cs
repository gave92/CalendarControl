using CalendarControl.Models;
using Windows.UI.Xaml;

namespace CalendarControl
{
    public partial class EventView
    {
        public int ListItemCount
        {
            get { return (int)GetValue(ListItemCountProperty); }
            set { SetValue(ListItemCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ListItemCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListItemCountProperty =
            DependencyProperty.Register("ListItemCount", typeof(int), typeof(EventView), new PropertyMetadata(0, OnListItemCountChanged));

        private static void OnListItemCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventview = d as EventView;
            if (eventview == null) return;
            eventview.UpdateEventsHeight();
        }

        public Day Day
        {
            get { return (Day)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Day.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register("Day", typeof(Day), typeof(EventView), new PropertyMetadata(null, OnDayChanged));

        private static void OnDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventview = d as EventView;
            if (eventview == null) return;
            eventview.OnDayChanged(e);
        }        
    }
}
