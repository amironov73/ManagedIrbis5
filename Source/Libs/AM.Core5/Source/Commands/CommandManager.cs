// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* CommandManager.cs -- менеджер команд
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Commands
{
    /// <summary>
    /// Менеджер команд.
    /// </summary>
    public class CommandManager
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Команды.
        /// </summary>
        public CommandDictionary Commands { get; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск команды по ее названию.
        /// </summary>
        /// <param name="name">Искомое название команды
        /// (регистр символов учитывается).</param>
        /// <returns></returns>
        public ICommand? FindCommand
            (
                string? name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return default;
            }

            Commands.TryGetValue(name, out var result);

            return result;
        } // method FindCommand

        /// <summary>
        /// Выполнение команды в синхронном режиме.
        /// (учитывается состояние флага <see cref="ICommand.Enabled"/>).
        /// </summary>
        public void PerformExecute(string name) => FindCommand(name)?.PerformExecute();

        /// <summary>
        /// Выполнение команды в асинхронном режиме.
        /// (учитывается состояние флага <see cref="ICommand.Enabled"/>).
        /// </summary>
        public Task PerformExecuteAsync
            (
                string name
            )
        {
            var command = FindCommand(name);

            return command is null ? Task.CompletedTask : command.PerformExecuteAsync();
        } // method PerformExecuteAsync

        /// <summary>
        /// Команда должна обновить свое состояние в синхронном режиме.
        /// </summary>
        public void PerformUpdate(string name) => FindCommand(name)?.PerformUpdate();

        /// <summary>
        /// Команда должна обновить свое состояние в асинхронном режиме.
        /// </summary>
        public Task PerformUpdateAsync(string name)
        {
            var command = FindCommand(name);

            return command is null ? Task.CompletedTask : command.PerformUpdateAsync();
        } // method PerformUpdateAsync

        /// <summary>
        /// Сигнал, что в команде что-то поменялось (синхронный режим).
        /// </summary>
        public void PerformChange(string name) => FindCommand(name)?.PerformChange();

        /// <summary>
        /// Сигнал, что в команде что-то поменялось (асинхронный режим).
        /// </summary>
        public Task PerformChangeAsync
            (
                string name
            )
        {
            var command = FindCommand(name);

            return command is null ? Task.CompletedTask : command.PerformChangeAsync();
        } // method PerformChangeAsync

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => Commands.Dispose();

        #endregion

    } // class CommandManager

} // namespace AM.Commands
