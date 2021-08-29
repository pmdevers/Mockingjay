using System.Collections.Generic;

namespace Mockingjay.Common.DomainModel
{
    public class AggregateRoot<T, TId> : AggregateRoot<T>
        where T : AggregateRoot<T>
    {
        protected AggregateRoot(TId aggregateId)
        {
            Buffer = new EventBuffer<TId>(aggregateId);
        }

        public TId Id => Buffer.AggregateId;

        public int Version => Buffer.Version;

        internal EventBuffer<TId> Buffer { get; private set; }

        protected override void AddEventsToBuffer(IEnumerable<object> events)
            => Buffer = Buffer.Add(events);

        internal void Replay(EventBuffer<TId> eventBuffer)
        {
            Buffer = new EventBuffer<TId>(eventBuffer.AggregateId, eventBuffer.CommittedVersion);
            Replay(eventBuffer.Committed);
        }
    }
}
