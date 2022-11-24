// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ListUsersCommand.cs -- получение списка пользователей с сервера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM.Collections;
using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Получение списка пользователей с серввера ИРБИС64.
/// </summary>
public sealed class ListUsersCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ListUsersCommand()
        : base ("listusers")
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

        string? pattern = null;
        if (arguments.Length != 0)
        {
            pattern = arguments[0].Text?.Trim();
        }

        if (string.IsNullOrWhiteSpace (pattern))
        {
            pattern = null;
        }

        var users = provider.ListUsers();
        if (users.IsNullOrEmpty())
        {
            executive.WriteError ("Can't get list of users");
            return false;
        }

        users = users.OrderBy (u => u.Name).ToArray();
        var list = new List<UserInfo>();
        foreach (var user in users)
        {
            if (!string.IsNullOrEmpty (pattern)
                && !string.IsNullOrEmpty (user.Name)
                && !Regex.IsMatch (user.Name, pattern, RegexOptions.IgnoreCase))
            {
                continue;
            }

            list.Add (user);
        }

        var tablefier = new Tablefier();
        string[] properties =
        {
            "Name", "Password", "Cataloger", "Circulation",
            "Administrator"
        };
        var output = tablefier.Print (list, properties).TrimEnd();
        executive.WriteOutput (output);

        OnAfterExecute();

        return true;
    }

    #endregion
}
