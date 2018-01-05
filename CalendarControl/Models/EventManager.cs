using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace CalendarControl
{
    public class EventManager
    {
        private static EventManager _manager;
        public static EventManager Instance
        {
            get
            {
                if (_manager == null)
                    _manager = new EventManager();
                return _manager;
            }
        }

        private SemaphoreSlim SemaphoreFile;
        private ObservableCollection<Event> Events { get; set; }

        private EventManager()
        {
            this.Events = new ObservableCollection<Event>();
            this.SemaphoreFile = new SemaphoreSlim(1, 1);
            this.LoadEvents();
        }

        public async void AddEvent(Event ev)
        {
            Events.Add(ev);
            await SaveEvents();
        }

        public async void RemoveEvent(Event ev)
        {
            Events.Remove(ev);
            await SaveEvents();
        }

        public async void RemoveSelectedEvents()
        {
            while (Events.Any(e => e.IsSelected))
                Events.Remove(Events.First(e => e.IsSelected));
            await SaveEvents();
        }

        public ISet<Event> GetConcurrentEvents(Event @event, ISet<Event> set = null)
        {
            if (set == null)
            {
                set = new SortedSet<Event>();
            }

            foreach (var ev in GetImmediateConcurrentEvents(@event))
            {
                if (!set.Contains(ev))
                {
                    set.Add(ev);
                    set.UnionWith(GetConcurrentEvents(ev, set));
                }
            }
            return set;
        }

        private List<Event> GetImmediateConcurrentEvents(Event ev)
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

        public async Task LoadEvents()
        {
            await SemaphoreFile.WaitAsync();

            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("Events.txt");
                var json = await FileIO.ReadTextAsync(file);
                var value = JsonConvert.DeserializeObject<List<Event>>(json);

                foreach (var ev in value)
                {
                    Events.Add(ev);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                SemaphoreFile.Release();
            }
        }

        public async Task SaveEvents()
        {
            await SemaphoreFile.WaitAsync();

            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Events.txt", CreationCollisionOption.ReplaceExisting);
                var json = JsonConvert.SerializeObject(Events.ToList());
                await FileIO.WriteTextAsync(file, json);
            }
            finally
            {
                SemaphoreFile.Release();
            }
        }

        public ICollectionView ForDay(DateTimeOffset date)
        {
            var view = new CollectionView.ListCollectionView();
            view.Source = Events;
            view.Filter = (ev) => (ev as Event).StartDate.Date == date.Date || (ev as Event).EndDate.Date == date.Date;
            return view;
        }
    }
}
