/*
 * Простая утилита, позволяющая вызвать метод из .NET-сборки.
 * Командная строка:
 *
 * UniCall <assembly> <type> <method>
 *
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace UniCall
{
    static class Program
    {
        static int Main
            (
                string[] args
            )
        {
            if (args.Length != 3)
            {
                Console.Error.WriteLine
                    (
                        "Usage: UniCall <assembly> <type> <method>"
                    );

                return 1;

            } // if

            try
            {

                var assemblyPath = args[0];
                var assembly = Assembly.LoadFile (assemblyPath);

                var typeName = args[1];
                var type = assembly.GetType (typeName, true);

                var methodName = args[2];
                var method = type?.GetMethod
                    (
                        methodName,
                        BindingFlags.Instance
                        | BindingFlags.Static
                        | BindingFlags.Public
                        | BindingFlags.NonPublic
                    );
                if (ReferenceEquals(method, null))
                {
                    Console.Error.WriteLine
                        (
                            "Can't find method {0}",
                            methodName
                        );

                    return 1;
                }

                object? result;

                if (method.IsStatic)
                {
                    result = method.Invoke (null, null);
                }
                else
                {
                    var instance = Activator.CreateInstance (type!);
                    if (ReferenceEquals (instance, null))
                    {
                        Console.Error.WriteLine
                            (
                                "Can't create instance of {0}",
                                type
                            );

                        return 1;
                    }

                    result = method.Invoke (instance, null);

                } // else

                result ??= string.Empty;

                Console.Out.WriteLine (result);

            } // try

            catch (Exception exception)
            {
                Console.Error.WriteLine (exception);
                return 1;
            }

            return 0;

        } // method Main

    } // class Program

} // namespace UniCall
