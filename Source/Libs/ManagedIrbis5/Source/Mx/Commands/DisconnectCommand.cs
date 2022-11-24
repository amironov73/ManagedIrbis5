// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DisconnectCommand.cs -- отключение от сервера
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Отключение от сервера.
/// </summary>
public sealed class DisconnectCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DisconnectCommand()
        : base ("disconnect")
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

        if (executive.Provider.IsConnected)
        {
            executive.Provider.Dispose();
            executive.WriteMessage ("disconnected");
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
