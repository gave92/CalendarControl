using CalendarControl.CollectionView;
using CalendarControl.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace CalendarControl.Models
{
    public class LocalEventManager : IEventManager
    {
        private SemaphoreSlim SemaphoreFile;
        private ObservableCollection<Event> Events { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Await.Warning", "CS4014:Await.Warning")]
        public LocalEventManager()
        {
            this.Events = new ObservableCollection<Event>();
            this.SemaphoreFile = new SemaphoreSlim(1, 1);
            this.LoadEvents();
        }

        public async Task AddEventAsync(Event ev)
        {
            Events.Add(ev);
            await SaveEvents();
        }

        public async Task RemoveEventAsync(Event ev)
        {
            Events.Remove(ev);
            await SaveEvents();
        }

        public async Task RemoveEventsAsync(Func<Event, bool> predicate)
        {
            while (Events.Any(predicate))
                Events.Remove(Events.First(predicate));
            await SaveEvents();
        }        

        public virtual async Task LoadEvents()
        {
            await SemaphoreFile.WaitAsync();

            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("Events.txt");
                var json = await FileIO.ReadTextAsync(file);
                var value = JsonConvert.DeserializeObject<List<Event>>(json, settings);

                foreach (var ev in value)
                {
                    Events.Add(ev);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                SemaphoreFile.Release();
            }
        }

        public virtual async Task SaveEvents()
        {
            await SemaphoreFile.WaitAsync();

            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Events.txt", CreationCollisionOption.ReplaceExisting);
                var json = JsonConvert.SerializeObject(Events.ToList(), settings);
                await FileIO.WriteTextAsync(file, json);
            }
            finally
            {
                SemaphoreFile.Release();
            }
        }

        public DailyEventManager ForDay(DateTimeOffset date)
        {
            var view = new ListCollectionView();
            view.Source = Events;
            view.Filter = (ev) => (ev as Event).StartDate.Date == date.Date || (ev as Event).EndDate.Date == date.Date;
            return new DailyEventManager(this, view);
        }
    }
}
