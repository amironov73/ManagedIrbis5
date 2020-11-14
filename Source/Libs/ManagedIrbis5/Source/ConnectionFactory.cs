using ManagedIrbis.Infrastructure.Sockets;

namespace ManagedIrbis
{
    public class ConnectionFactory
    {
        public virtual Connection CreateConnection<T>()
            where T : ClientSocket, new()
        {
            var socket = new T();
            var result = new Connection(socket);

            return result;
        }
    }
}