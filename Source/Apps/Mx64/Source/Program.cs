// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.AppServices;

using ManagedIrbis.Mx;

#endregion

#nullable enable

namespace Mx64;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal sealed class Program
    : MagnaApplication
{
    #region Construciton

    internal Program
        (
            string[] args
        )
        : base (args, turnOffLogging: true)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    public static void Main
        (
            string[] args
        )
    {
        var application = new Program (args);
        application.Run();

        var executive = new MxExecutive();
        executive.Banner();
        executive.ParseCommandLine (args);
        executive.Repl();

        application.Shutdown();
    }

    #endregion
}
