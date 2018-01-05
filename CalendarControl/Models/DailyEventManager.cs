using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace CalendarControl
{
    public class DailyEventManager
    {
        public ICollectionView View { get; private set; }

        private WeakReference<IEventManager> _eventManager;
        public IEventManager EventManager
        {
            get
            {
                IEventManager value;
                _eventManager.TryGetTarget(out value);
                return value;
            }
            set
            {
                _eventManager = new WeakReference<IEventManager>(value);
            }
        }

        public DailyEventManager(IEventManager manager, ICollectionView view)
        {
            this.View = view;
            this.EventManager = manager;
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
            foreach (var other in View.Cast<Event>())
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
    }
}
