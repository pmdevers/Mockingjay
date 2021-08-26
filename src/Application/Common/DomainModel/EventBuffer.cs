using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Application.Common.DomainModel
{
    public delegate object ConvertFromStoredEvent<in TStoredEvent>(TStoredEvent storedEvent);
    public delegate TStoredEvent ConvertToStoredEvent<in TId, out TStoredEvent>(TId aggregateId, int version, object @event);

    public class EventBuffer<TId> : IEnumerable<object>
    {
        private readonly int _offset;
        private readonly List<object> _buffer;

        public EventBuffer(TId aggregateId) : this(aggregateId, 0) { }
        public EventBuffer(TId aggregateId, int version)
            : this(aggregateId, version, committed: version, Array.Empty<object>()) { }

        private EventBuffer(TId aggregateId, int offset, int committed, IEnumerable<object> buffer)
        {
            AggregateId = aggregateId;
            CommittedVersion = committed;
            _offset = offset;
            _buffer = new List<object>();
            _buffer.AddRange(buffer);
        }

        public TId AggregateId { get; }
        public int Version => _buffer.Count + _offset;
        public int CommittedVersion { get; }
        public IEnumerable<object> Committed => _buffer.Take(CommittedVersion - _offset);
        public IEnumerable<object> Uncommitted => _buffer.Skip(CommittedVersion - _offset);
        public bool HasUncommited => Version != CommittedVersion;

        public bool IsEmpty => _buffer.Count == 0;

        public EventBuffer<TId> Add(object events)
        {
            _buffer.AddRange(events as IEnumerable<object>);
            return new (AggregateId, _offset, CommittedVersion, _buffer);
        }

        public EventBuffer<TId> MarkAllAsCommitted()
            => new (AggregateId, _offset, Version, _buffer);

        public EventBuffer<TId> ClearCommited()
            => new (AggregateId, CommittedVersion, CommittedVersion, Uncommitted);

        public IEnumerable<TStoredEvent> SelectUncommitted<TStoredEvent>(ConvertToStoredEvent<TId, TStoredEvent> convert)
        {
            Guard.NotNull(convert, nameof(convert));

            var version = CommittedVersion;
            foreach (var @event in Uncommitted)
            {
                yield return convert(AggregateId, ++version, @event);
            }
        }

        public IEnumerator<object> GetEnumerator() => _buffer.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string DebuggerDisplay
            => Version == CommittedVersion
                ? $"Version: {Version}, Aggregate: {AggregateId}"
                : $"Version: {Version} (Committed: {CommittedVersion}), Aggregate: {AggregateId}";

        public static EventBuffer<TId> FromStorage<TStoredEvent>(
            TId aggregateId,
            IEnumerable<TStoredEvent> storedEvents,
            ConvertFromStoredEvent<TStoredEvent> convert)
        {
            return FromStorage(aggregateId, 0, storedEvents, convert);
        }

        public static EventBuffer<TId> FromStorage<TStoredEvent>(
            TId aggregateId,
            int initialVersion,
            IEnumerable<TStoredEvent> storedEvents,
            ConvertFromStoredEvent<TStoredEvent> convert)
        {
            Guard.NotNegative(initialVersion, nameof(initialVersion));
            Guard.NotNull(storedEvents, nameof(storedEvents));
            Guard.NotNull(convert, nameof(convert));

            return new EventBuffer<TId>(aggregateId, initialVersion)
                   .Add(storedEvents.Select(storedEvent => convert(storedEvent)))
                   .MarkAllAsCommitted();
        }
    }
}
