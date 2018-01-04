using System;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CalendarControl
{
    public sealed partial class EventView : Control
    {
        public ICollectionView Events { get; private set; }

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
            var e = new CollectionView.VectorChangedEventArgs(CollectionChange.Reset);
            this.OnEventsCollectionChanged(null, e);
        }

        private void OnEventsCollectionChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;

            var e = args as CollectionView.VectorChangedEventArgs;

            if (e.CollectionChange == CollectionChange.Reset)
            {
                canvas.Children.Clear();
                foreach (Event ev in Events)
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
                var ev = Events.Cast<Event>().Single(e => e == (item as EventItem).Event);
                ev.Concurrent = EventManager.Instance.GetConcurrentEvents(ev);
            }

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = Events.Cast<Event>().Single(e => e == (item as EventItem).Event);
                var concurrent = ev.GetConcurrent().ToList();
                item.Width = Math.Max(0, (canvas.ActualWidth - 20) / (double)concurrent.Count);
                Canvas.SetLeft(item, concurrent.IndexOf(ev) * item.Width);
            }
        }

        private void UpdateEventsHeight()
        {
            var canvas = GetTemplateChild("RootCanvas") as Canvas;
            if (canvas == null) return;
            if (canvas.ActualHeight == 0) return;

            foreach (var item in canvas.Children.Cast<FrameworkElement>())
            {
                var ev = Events.Cast<Event>().Single(e => e == (item as EventItem).Event);
                item.Height = (ev.EndDate - ev.StartDate).Hours * canvas.ActualHeight / (double)ListItemCount;
                Canvas.SetTop(item, ev.StartDate.Hour * canvas.ActualHeight / (double)ListItemCount);
            }
        }

        private void OnDayChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Events != null)
            {
                this.Events.VectorChanged -= OnEventsCollectionChanged;
            }

            var nday = (Day)e.NewValue;

            if (nday != null)
            {
                this.Events = EventManager.Instance.ForDay(nday.Date);
                this.Events.VectorChanged += OnEventsCollectionChanged ;
            }
        }
    }
}
