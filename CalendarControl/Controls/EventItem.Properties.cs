using CalendarControl.Models;
using Windows.UI.Xaml;

namespace CalendarControl
{
    public partial class EventItem
    {
        public int NormalMinWidth
        {
            get { return (int)GetValue(NormalMinWidthProperty); }
            set { SetValue(NormalMinWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalMinWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalMinWidthProperty =
            DependencyProperty.Register("NormalMinWidth", typeof(int), typeof(EventItem), new PropertyMetadata(50, OnNormalMinWidthChanged));        

        public Event Event
        {
            get { return (Event)GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Event.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.Register("Event", typeof(Event), typeof(EventItem), new PropertyMetadata(null));
    }
}
