using System.Collections.ObjectModel;
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

        public ObservableCollection<Event> Events
        {
            get { return (ObservableCollection<Event>)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Events.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventsProperty =
            DependencyProperty.Register("Events", typeof(ObservableCollection<Event>), typeof(EventView), new PropertyMetadata(null, OnEventsChanged));

        private static void OnEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventview = d as EventView;
            if (eventview == null) return;
            if (e.OldValue != null)
                (e.OldValue as ObservableCollection<Event>).CollectionChanged -= eventview.OnEventsCollectionChanged;
            if (e.NewValue != null)
                (e.NewValue as ObservableCollection<Event>).CollectionChanged += eventview.OnEventsCollectionChanged;
        }
    }
}
