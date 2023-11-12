// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CommandManager.cs -- менеджер команд
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

namespace AM.Commands;

/// <summary>
/// Менеджер команд.
/// </summary>
[PublicAPI]
public class CommandManager
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Словарь команд.
    /// </summary>
    public CommandDictionary Commands { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск команды по ее названию.
    /// </summary>
    /// <param name="name">Искомое название команды
    /// (регистр символов учитывается).</param>
    /// <returns>Найденная команда либо <c>null</c></returns>
    public ICommand? FindCommand
        (
            string? name
        )
    {
        if (string.IsNullOrEmpty (name))
        {
            return default;
        }

        Commands.TryGetValue (name, out var result);

        return result;
    }

    /// <summary>
    /// Выполнение команды в синхронном режиме.
    /// (учитывается состояние флага <see cref="ICommand.Enabled"/>).
    /// </summary>
    public void PerformExecute
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        FindCommand (name)?.PerformExecute();
    }

    /// <summary>
    /// Выполнение команды в асинхронном режиме.
    /// (учитывается состояние флага <see cref="ICommand.Enabled"/>).
    /// </summary>
    public Task PerformExecuteAsync
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var command = FindCommand (name);

        return command is null ? Task.CompletedTask : command.PerformExecuteAsync();
    }

    /// <summary>
    /// Команда должна обновить свое состояние в синхронном режиме.
    /// </summary>
    public void PerformUpdate
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        FindCommand (name)?.PerformUpdate();
    }

    /// <summary>
    /// Команда должна обновить свое состояние в асинхронном режиме.
    /// </summary>
    public Task PerformUpdateAsync
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var command = FindCommand (name);

        return command is null ? Task.CompletedTask : command.PerformUpdateAsync();
    }

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (синхронный режим).
    /// </summary>
    public void PerformChange
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        FindCommand (name)?.PerformChange();
    }

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (асинхронный режим).
    /// </summary>
    public Task PerformChangeAsync
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var command = FindCommand (name);

        return command is null ? Task.CompletedTask : command.PerformChangeAsync();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Commands.Dispose();

    #endregion
}
