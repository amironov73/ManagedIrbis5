// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ReconnectCommand.cs -- переподключение к серверу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Переподключение к серверу.
/// </summary>
public sealed class ReconnectCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReconnectCommand()
        : base ("reconnect")
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

        var provider = executive.Provider;
        provider.Disconnect();

        executive.GetCommand<ConnectCommand>().Execute
            (
                executive,
                MxArgument.Empty
            );

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string GetShortHelp()
    {
        return "Reconnect to the server";
    }

    #endregion
}
