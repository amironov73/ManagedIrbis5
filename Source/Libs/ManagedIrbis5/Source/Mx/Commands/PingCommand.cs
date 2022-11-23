// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PingCommand.cs -- пингование сервера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Пингование сервера ИРБИС64.
/// </summary>
public sealed class PingCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PingCommand()
        : base ("ping")
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Do one ping operation.
    /// </summary>
    public long DoPing
        (
            int number,
            MxExecutive executive
        )
    {
        long result = 0;

        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            executive.Provider.NoOperation();
            stopwatch.Stop();

            result = stopwatch.ElapsedMilliseconds;

            executive.WriteMessage ($"{number}: {result} ms");
        }
        catch
        {
            executive.WriteError ("ERROR");
        }

        return result;
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute" />
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        if (!executive.Provider.IsConnected)
        {
            executive.WriteLine ("Not connected");
            return false;
        }

        long sum = 0;
        var ntries = 4;
        if (arguments.Length != 0)
        {
            var n = arguments[0].Text.SafeToInt32();
            if (n > 1)
            {
                ntries = n;
            }
        }

        for (var i = 0; i < ntries; i++)
        {
            sum += DoPing (i + 1, executive);
        }

        executive.WriteMessage ($"average = {sum / ntries}");

        OnAfterExecute();

        return true;
    }

    #endregion
}
