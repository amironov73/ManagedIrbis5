// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExitCommand.cs -- завершение обработки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Завершение обработки.
/// </summary>
public sealed class ExitCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ExitCommand()
        : base ("exit")
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

        executive.WriteMessage ("Exiting");
        executive.StopFlag = true;

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string? GetShortHelp()
    {
        return "Exit from the interpreter";
    }

    #endregion
}
