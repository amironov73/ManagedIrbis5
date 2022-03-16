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
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace UniCall;

static class Program
{
    static int Main
        (
            string[] args
        )
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine ("USAGE: UniCall <assembly> <type> <method>");
            return 1;
        }

        try
        {
            var assemblyPath = args[0];

            // Load и LoadFile не работают
            var assembly = Assembly.LoadFrom (assemblyPath);

            var typeName = args[1];
            var type = assembly.GetType (typeName, true);
            if (type is null)
            {
                Console.Error.WriteLine ($"Can't find type {typeName}");
                return 2;
            }

            var methodName = args[2];
            var method = type.GetMethod
                (
                    methodName,
                    BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                );
            if (method is null)
            {
                Console.Error.WriteLine ($"Can't find method {methodName}");
                return 3;
            }

            RuntimeHelpers.PrepareMethod (method.MethodHandle);

            // Console.WriteLine("Now you can attach debugger");
            // Console.ReadLine();

            object? result;

            if (method.IsStatic)
            {
                result = method.Invoke (null, null);
            }
            else
            {
                var instance = Activator.CreateInstance (type);
                if (ReferenceEquals (instance, null))
                {
                    Console.Error.WriteLine ($"Can't create instance of {type}");
                    return 4;
                }

                result = method.Invoke (instance, null);
            }

            result ??= string.Empty;

            Console.Out.WriteLine (result);
        }

        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 5;
        }

        return 0;
    }
}
