// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* VerCommand.cs -- определение версии сервера
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM;
using AM.Reflection;

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Определение версии сервера ИРБИС64.
/// </summary>
public sealed class VerCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public VerCommand()
        : base ("ver")
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

        var version = provider.GetServerVersion();
        if (version is null)
        {
            executive.WriteError ("Can't determine version");
            return false;
        }

        var tablefier = new Tablefier();
        var items = new NameValue<string?>[]
        {
            new ("Version", version.Version),
            new ("Connected", version.ConnectedClients.ToInvariantString()),
            new ("Max clients", version.MaxClients.ToInvariantString()),
            new ("Organization", version.Organization)
        };
        var output = tablefier.Print (items, "Name", "Value");
        executive.WriteOutput (output);

        OnAfterExecute();

        return true;
    }

    #endregion
}
