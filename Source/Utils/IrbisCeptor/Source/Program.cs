// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Program.cs -- program entry point
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace IrbisCeptor;

internal static class Program
{
    private const string DataPath = "Capture";
    private const int BufferSize = 8_388_608;

    private static int _counter;

    private static int _localPort;
    private static int _remotePort;
    private static IPAddress _remoteIp = null!;

    private static byte[] ReadToEnd
        (
            Stream stream,
            int portion
        )
    {
        var result = new MemoryStream();
        var buffer = new byte[portion];
        while (true)
        {
            var read = stream.Read (buffer, 0, buffer.Length);
            if (read <= 0)
            {
                break;
            }
            result.Write(buffer, 0, read);
        }

        return result.ToArray();
    }

    private static void WriteBuffer
        (
            int counter,
            string suffix,
            byte[] buffer,
            int length
        )
    {
        var fileName = Path.Combine
            (
                DataPath,
                $"{counter:00000000}{suffix}.packet"
            );

        using Stream stream = File.Create (fileName);
        stream.Write (buffer, 0, length);
    }

    static void HandleClient
        (
            TcpClient downClient
        )
    {
        try
        {
            var counter = Interlocked.Increment (ref _counter);
            Console.WriteLine ($"Got downstream: {counter}");

            var downStream = downClient.GetStream();

            var address = _remoteIp;
            var endPoint = new IPEndPoint(address, _remotePort);
            var upClient = new TcpClient();
            upClient.Connect(endPoint);
            Console.WriteLine ("Connected to upstream");

            var buffer1 = new byte[BufferSize];
            var length = downStream.Read (buffer1, 0, buffer1.Length);
            Console.WriteLine ($"Readed downstream {length}");

            var upStream = upClient.GetStream();
            upStream.Write (buffer1, 0, length);
            Console.WriteLine("Written to upstream");

            WriteBuffer
                (
                    counter,
                    suffix: "dn",
                    buffer1,
                    length
                );

            var buffer2 = ReadToEnd (upStream, 100 * 1024);
            length = buffer2.Length;
            Console.WriteLine ($"Readed upstream {length}");
            downStream.Write (buffer2, 0, length);
            Console.WriteLine ("Written downstream");

            WriteBuffer
                (
                    counter,
                    suffix: "up",
                    buffer2,
                    length
                );

            upClient.Close();
            Console.WriteLine ("Closed upstream");

            downClient.Close();
            Console.WriteLine ("Closed downstream");

            Console.WriteLine();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
        }
    }

    public static int Main (string[] args)
    {
        if (args.Length is < 2 or > 3)
        {
            Console.Error.WriteLine ("Usage: IrbisCeptor <local-port> [remote-ip] <remote-port>");
            return 1;
        }

        const NumberStyles numberStyles = NumberStyles.Integer;
        var invariantCulture = CultureInfo.InvariantCulture;
        try
        {
            _localPort = int.Parse (args[0], numberStyles, invariantCulture);
            _remoteIp = args.Length == 2
                ? IPAddress.Loopback
                : IPAddress.Parse (args[1]);
            _remotePort = int.Parse
                (
                    args.Length == 2 ? args[1] : args[2],
                    numberStyles,
                    invariantCulture
                );

            Directory.CreateDirectory (DataPath);

            foreach (var fileName in Directory.EnumerateFiles (DataPath))
            {
                File.Delete (fileName);
            }

            var address = IPAddress.Any;
            var endPoint = new IPEndPoint (address, _localPort);
            var listener = new TcpListener (endPoint);
            listener.Start();

            Console.WriteLine ("Press Ctrl-Break to stop");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                Task.Factory.StartNew
                    (
                        () => HandleClient (client)
                    );
            }

        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        #pragma warning disable CS0162 // unreachable code

        return 0;
    }
}
