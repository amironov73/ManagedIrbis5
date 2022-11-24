// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ListDbCommand.cs -- получение списка баз данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM.Collections;
using AM.Reflection;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Получение списка баз данных.
/// </summary>
public sealed class ListDbCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ListDbCommand()
        : base ("listdb")
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

        // TODO брать имя списка из серверного INI-файла

        const string defaultMenu = "dbnam1.mnu";
        var menuName = defaultMenu;
        if (arguments.Length != 0)
        {
            menuName = arguments[0].Text;
        }

        if (string.IsNullOrWhiteSpace (menuName))
        {
            menuName = defaultMenu;
        }

        var databases = provider.ListDatabases (menuName);
        if (databases.IsNullOrEmpty())
        {
            executive.WriteError ("Can't get list of databases");
            return false;
        }

        databases = databases.OrderBy (db => db.Name).ToArray();
        var tablefier = new Tablefier();
        var output = tablefier.Print (databases, "Name", "Description")
                .TrimEnd();
        executive.WriteOutput (output);

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string? GetShortHelp()
    {
        return "List databases";
    }

    #endregion
}
