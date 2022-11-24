// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DbCommand.cs -- переключение на другую базу данных
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System.Linq;

using AM;

using ManagedIrbis.Providers;

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Команда прееключения на другую базу данных.
/// </summary>
public sealed class DbCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DbCommand()
        : base ("db")
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

        var saveDatabase = provider.EnsureDatabase();
        var dbName = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        if (!string.IsNullOrEmpty (dbName))
        {
            var aliases = executive.Aliases;
            if (aliases.ContainsKey (dbName))
            {
                var expanded = aliases[dbName];
                if (expanded.Contains ('='))
                {
                    executive.Provider.ParseConnectionString (expanded);
                }
                else
                {
                    executive.Provider.Database = expanded;
                }

            }
            else
            {
                executive.Provider.Database = dbName;
            }
        }

        try
        {
            var maxMfn = executive.Provider.GetMaxMfn() - 1;
            executive.WriteMessage ($"DB={executive.Provider.Database} max MFN={maxMfn}");
        }
        catch
        {
            executive.WriteError ($"Error changing DB, restoring to {saveDatabase}");
            executive.Provider.Database = saveDatabase;
        }

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string? GetShortHelp()
    {
        return "Switch the current database";
    }

    #endregion
}
