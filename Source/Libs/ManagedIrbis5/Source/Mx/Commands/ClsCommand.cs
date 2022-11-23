// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ClsCommand.cs -- очистка консоли
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Очистка консоли.
/// </summary>
public sealed class ClsCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ClsCommand()
        : base ("cls")
    {
        // пустое тело конструктора
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

        executive.MxConsole.Clear();

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp" />
    public override string GetShortHelp()
    {
        return "Clear the console";
    }

    #endregion
}
