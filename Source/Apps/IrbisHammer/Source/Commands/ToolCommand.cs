// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ToolCommand.cs -- абстрактная команда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace IrbisHammer.Commands;

/// <summary>
/// Абстрактная команда.
/// </summary>
public abstract class ToolCommand
{
    #region Properties

    /// <summary>
    /// Имя команды.
    /// </summary>
    public abstract string? CommandName { get; }

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение команды.
    /// </summary>
    /// <param name="context">
    /// Контекст, в котором будет выполнена команда.
    /// </param>
    public abstract void ExecuteCommand
        (
            HammerContext context
        );

    /// <summary>
    /// Выполнение команды.
    /// </summary>
    public string Run
        (
            string workingDirectory,
            string command,
            string[] arguments
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return CommandName.ToVisibleString();
    }

    #endregion
}
