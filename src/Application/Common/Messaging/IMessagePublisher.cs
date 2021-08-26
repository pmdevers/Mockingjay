using System.Threading.Tasks;

namespace Application.Common.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : class;
    }
}
