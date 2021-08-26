using System.Threading.Tasks;

namespace Mockingjay.Common.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class;
    }
}
