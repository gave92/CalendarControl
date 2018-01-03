using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CalendarControl
{
    public sealed partial class EventView : Control
    {
        public EventView()
        {
            this.DefaultStyleKey = typeof(EventView);
            this.SizeChanged += EventView_SizeChanged;
        }

        private void EventView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateEventsHeight();
            UpdateEventsWidth();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.OnEventsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnEventsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                canvas.Children.Clear();
                foreach (Event ev in Events)
                {
                    var rect = new EventItem() { Event = ev };
                    canvas.Children.Add(rect);
                }
            }
            else
            {
                if (e.NewItems != null)
                {
                    foreach (Event ev in e.NewItems.Cast<Event>())
                    {
                        var rect = new EventItem() { Event = ev };
                        canvas.Children.Add(rect);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (Event ev in e.OldItems.Cast<Event>())
                    {
                        canvas.Children.Remove(canvas.Children.Single(item => ev == (item as EventItem).Event));
                    }
                }
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
                var ev = Events.Single(e => e == (item as EventItem).Event);
                ev.Concurrent = GetConcurrentEvents(ev);
            }

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = Events.Single(e => e == (item as EventItem).Event);
                var concurrent = ev.GetConcurrent().ToList();
                item.Width = Math.Max(0, (canvas.ActualWidth - 20) / (double)concurrent.Count);
                Canvas.SetLeft(item, concurrent.IndexOf(ev) * item.Width);
            }
        }

        private List<Event> GetConcurrentEvents(Event ev)
        {
            var cevs = new List<Event>();
            foreach (var other in Events)
            {
                var endsBefore = (ev.EndDate <= other.StartDate);
                var startsAfter = (ev.StartDate >= other.EndDate);
                if (!(endsBefore || startsAfter))
                {
                    cevs.Add(other);
                }
            }
            return cevs;
        }

        private void UpdateEventsHeight()
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;
            if (canvas.ActualHeight == 0) return;

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = Events.Single(e => e == (item as EventItem).Event);
                item.Height = (ev.EndDate - ev.StartDate).Hours * canvas.ActualHeight / (double)ListItemCount;
                Canvas.SetTop(item, ev.StartDate.Hour * canvas.ActualHeight / (double)ListItemCount);
            }
        }
    }
}
