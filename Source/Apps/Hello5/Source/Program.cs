using System;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Sockets;

namespace Hello5
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                using var connection = new ConnectionFactory()
                    .CreateConnection<PlainTcp4Socket>();

                connection.Host = "127.0.0.1";
                connection.Username = "librarian";
                connection.Password = "secret";

                var success = await connection.ConnectAsync();
                if (!success)
                {
                    Console.WriteLine("Can't connect");
                    return 1;
                }

                Console.WriteLine("Successfully connected");

                var maxMfn = await connection.GetMaxMfnAsync();
                Console.WriteLine($"Max MFN={maxMfn}");

                await connection.DisconnectAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 1;
            }

            return 0;
        }
    }
}
