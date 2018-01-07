using CalendarControl.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalendarControl
{
    public sealed partial class EventItem : Control
    {
        public EventItem()
        {
            this.DefaultStyleKey = typeof(EventItem);
            this.DataContextChanged += EventItem_DataContextChanged; ;
            this.Tapped += OnEventTapped;
            this.SizeChanged += OnEventSizeChanged;
            this.Unloaded += EventItem_Unloaded;
        }

        private void EventItem_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Tapped -= OnEventTapped;
            this.SizeChanged -= OnEventSizeChanged;
            this.DataContextChanged -= EventItem_DataContextChanged;
            this.Unloaded -= EventItem_Unloaded;
        }

        private void EventItem_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs e)
        {
            Event = e.NewValue as Event;
        }

        private void OnEventSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Width >= NormalMinWidth)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Narrow", true);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void OnEventTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Event.IsSelected = !Event.IsSelected;
            VisualStateManager.GoToState(this, Event.IsSelected ? "VisualStateSelected" : "VisualStateNormal", true);
        }

        private static void OnNormalMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventItem = d as EventItem;
            if (eventItem == null) return;
            eventItem.OnEventSizeChanged(eventItem, null);
        }
    }
}
