// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RestartCommand.cs -- перезапуск сервера ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Перезапуск сервера ИРБИС64.
/// </summary>
public sealed class RestartCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RestartCommand()
        : base ("restart")
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
        if (!provider.IsConnected)
        {
            executive.WriteError ("Not connected");
            return false;
        }

        if (!provider.RestartServer())
        {
            executive.WriteError ("Can't restart the server");
            return false;
        }

        executive.WriteOutput ("Server restarted successfully");

        OnAfterExecute();

        return true;
    }

    #endregion
}
