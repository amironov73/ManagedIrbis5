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
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ICommand.cs -- интерфейс команды
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Commands;

/// <summary>
/// Интерфейс команды.
/// </summary>
public interface ICommand
    : IDisposable
{
    /// <summary>
    /// Вызывается при выполнении команды.
    /// </summary>
    public event EventHandler? Execute;

    /// <summary>
    /// Вызывается, когда команда должна обновить свое состояние.
    /// </summary>
    public event EventHandler? Update;

    /// <summary>
    /// Вызывается, когда в команде что-то поменялось
    /// (например, состояние <see cref="Enabled"/>).
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    /// Вызывается при очистке команды.
    /// </summary>
    public event EventHandler? Disposed;

    /// <summary>
    /// Команда разрешена к выполнению?
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Заглавие команды (произвольное).
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Описание команды в произвольной форме.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Выполнение команды в синхронном режиме.
    /// (учитывается состояние флага <see cref="Enabled"/>).
    /// </summary>
    public void PerformExecute();

    /// <summary>
    /// Выполнение команды в синхронном режиме
    /// (учитывается состояние флага <see cref="Enabled"/>).
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public void PerformExecute (EventArgs eventArgs);

    /// <summary>
    /// Выполнение команды в асинхронном режиме
    /// (учитывается состояние флага <see cref="Enabled"/>).
    /// </summary>
    public Task PerformExecuteAsync();

    /// <summary>
    /// Выполнение команды в асинхронном режиме.
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public Task PerformExecuteAsync (EventArgs eventArgs);

    /// <summary>
    /// Команда должна обновить свое состояние в синхронном режиме.
    /// </summary>
    public void PerformUpdate();

    /// <summary>
    /// Команда должна обновить свое состояние в синхронном режиме.
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public void PerformUpdate (EventArgs eventArgs);

    /// <summary>
    /// Команда должна обновить свое состояние в асинхронном режиме.
    /// </summary>
    public Task PerformUpdateAsync();

    /// <summary>
    /// Команда должна обновить свое состояние в асинхронном режиме.
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public Task PerformUpdateAsync (EventArgs eventArgs);

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (синхронный режим).
    /// </summary>
    public void PerformChange();

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (синхронный режим).
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public void PerformChange (EventArgs eventArgs);

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (асинхронный режим).
    /// </summary>
    public Task PerformChangeAsync();

    /// <summary>
    /// Сигнал, что в команде что-то поменялось (асинхронный режим).
    /// </summary>
    /// <param name="eventArgs">Аргументы для события.</param>
    public Task PerformChangeAsync (EventArgs eventArgs);
}
