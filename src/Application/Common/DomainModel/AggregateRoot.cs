namespace Mockingjay.Common.DomainModel
{
    public static class AggregateRoot
    {
        public static T FromStorage<T, TId>(EventBuffer<TId> buffer)
            where T : AggregateRoot<T, TId>, new()
        {
            Guard.HasAny(buffer, nameof(buffer));

            var aggregate = new T();

            aggregate.Replay(buffer);
            return aggregate;
        }
    }
}
