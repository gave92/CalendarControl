using CalendarControl.Models;
using System;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CalendarControl
{
    public sealed partial class EventView : Control
    {
        private DailyEventManager EventManager { get; set; }

        public EventView()
        {
            this.DefaultStyleKey = typeof(EventView);
            this.SizeChanged += EventView_SizeChanged;
            this.Unloaded += EventView_Unloaded;
        }

        private void EventView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (EventManager != null)
            {
                this.EventManager.View.VectorChanged -= OnEventsCollectionChanged;
                this.EventManager.View.Source = null;
            }

            this.SizeChanged -= EventView_SizeChanged;
            this.Unloaded -= EventView_Unloaded;
        }

        private void EventView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateEventsHeight();
            UpdateEventsWidth();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var e = new CollectionView.VectorChangedEventArgs(CollectionChange.Reset);
            this.OnEventsCollectionChanged(null, e);
        }

        private void OnEventsCollectionChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;
            if (EventManager == null) return;

            var e = args as CollectionView.VectorChangedEventArgs;

            if (e.CollectionChange == CollectionChange.Reset)
            {
                canvas.Children.Clear();
                foreach (Event ev in EventManager.View)
                {
                    var rect = new EventItem() { DataContext = ev };
                    canvas.Children.Add(rect);
                }
            }
            else if (e.CollectionChange == CollectionChange.ItemInserted)
            {
                var rect = new EventItem() { DataContext = e.Item as Event };
                canvas.Children.Add(rect);
            }
            else if (e.CollectionChange == CollectionChange.ItemRemoved)
            {
                canvas.Children.Remove(canvas.Children.Single(item => e.Item as Event == (item as EventItem).Event));
            }

            UpdateEventsHeight();
            UpdateEventsWidth();
        }

        private void UpdateEventsWidth()
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;
            if (canvas.ActualWidth == 0) return;

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = EventManager.View.Cast<Event>().Single(e => e == (item as EventItem).Event);
                var concurrent = EventManager.GetConcurrentEvents(ev).ToList();
                var count = (double)(concurrent?.Count ?? 0);
                item.Width = count > 0 ? Math.Max(0, (canvas.ActualWidth - 20) / count) : 0;
                Canvas.SetLeft(item, (concurrent?.IndexOf(ev) ?? 0) * item.Width);
            }
        }

        private void UpdateEventsHeight()
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;
            if (canvas.ActualHeight == 0) return;

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = EventManager.View.Cast<Event>().Single(e => e == (item as EventItem).Event);
                item.Height = (ev.EndDate - ev.StartDate).TotalHours * canvas.ActualHeight / (double)Day.Hours.Count;
                Canvas.SetTop(item, (ev.StartDate.TimeOfDay.TotalHours - Day.Hours[0].Time.Hour) * canvas.ActualHeight / (double)Day.Hours.Count);
            }
        }

        private void OnDayChanged(DependencyPropertyChangedEventArgs e)
        {
            if (EventManager != null)
            {
                this.EventManager.View.VectorChanged -= OnEventsCollectionChanged;
            }

            var nday = (Day)e.NewValue;

            if (nday != null)
            {
                this.EventManager = nday.EventManager;
                if (EventManager != null)
                {
                    this.EventManager.View.VectorChanged += OnEventsCollectionChanged;
                }                    
                this.UpdateEventsHeight();
            }
        }
    }
}
