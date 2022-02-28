// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- стартовый класс сервиса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Server;

#endregion

#nullable enable

namespace IrbisCoreServer;

/// <summary>
/// Стартовый класс сервиса.
/// </summary>
static class Program
{
    public static ServerEngine Engine { get; private set; } = null!;

    /// <summary>
    /// Запуск и эксплуатация сервера.
    /// </summary>
    private static async Task RunServer
        (
            string[] args
        )
    {
        try
        {
            Magna.Info ("START");

            await using (Engine = ServerUtility.CreateEngine (args))
            {
                ServerUtility.DumpEngineSettings (Engine);
                Magna.Info ("Entering server main loop");
                await Engine.MainLoop();
                Magna.Info ("Leave server main loop");
            }

            Magna.Info ("STOP");
        }
        catch (Exception exception)
        {
            Magna.TraceException (nameof (Program) + "::" + nameof (RunServer), exception);
        }
    }

    /// <summary>
    /// Точка входа в сервис.
    /// </summary>
    public static async Task<int> Main
        (
            string[] args
        )
    {
        Magna.Initialize (args, _ => { Console.Out.WriteLine ("Initialized"); });

        await RunServer (args);

        return 0;
    }
}
