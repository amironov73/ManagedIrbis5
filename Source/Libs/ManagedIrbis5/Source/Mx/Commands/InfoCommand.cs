// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InfoCommand.cs -- получение информации о сервере: нагрузка и прочее
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Получение информации о сервере: нагрузка и прочее.
/// </summary>
public sealed class InfoCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InfoCommand()
        : base ("info")
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

        var serverStat = provider.GetServerStat();
        if (serverStat is null)
        {
            executive.WriteError ("Can't get server info");

            return false;
        }

        var clients = serverStat.RunningClients;
        if (clients is not null)
        {
            var tablefier = new Tablefier();
            string[] properties =
            {
                "IPAddress", "Name", "ID", "Workstation", "Registered",
                "Acknowledged", "LastCommand", "CommandNumber"
            };
            var output = tablefier.Print (clients, properties)
                .TrimEnd();
            executive.WriteOutput (output);
        }

        return true;
    }

    #endregion
}
