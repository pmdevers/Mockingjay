using System.Collections.Generic;
using System.Diagnostics;

namespace Application.Common.DomainModel
{
    public abstract class AggregateRoot<T>
        where T : AggregateRoot<T>
    {
        protected AggregateRoot()
        {
            Dynamic = new DynamicEventDispatcher<T>((T)this);
        }
        protected static IReadOnlyCollection<object> Events => new object[0];

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual dynamic Dynamic { get; }

        protected abstract void AddEventsToBuffer(IEnumerable<object> events);

        protected T ApplyEvent(object @event) => ApplyEvents(@event);
        protected T ApplyEvents(params object[] events) => Apply(events);

        protected T Apply(IEnumerable<object> events)
        {
            Guard.HasAny(events, nameof(events));

            lock (_locker)
            {
                foreach (var @event in events)
                {
                    Dynamic.When(@event);
                }

                AddEventsToBuffer(events);

                return (T)this;
            }
        }

        protected void Replay(IEnumerable<object> events)
        {
            Guard.HasAny(events, nameof(events));

            lock (_locker)
            {
                foreach (var @event in events)
                {
                    Dynamic.When(@event);
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly object _locker = new object();
    }
}
