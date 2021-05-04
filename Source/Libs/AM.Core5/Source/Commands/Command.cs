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

#pragma warning disable 67

/* Command.cs -- некая команда (действие) приложения
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
    /// Некая команда (действие) приложения.
    /// </summary>
    public class Command
        : IDisposable
    {
        #region Events

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

        #endregion

        #region Properties

        /// <summary>
        /// Команда разрешена к выполнению?
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    PerformChange();
                }
            }
        }

        /// <summary>
        /// Заглавие команды (произвольное).
        /// </summary>
        public string? Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    PerformChange();
                }
            }
        }

        /// <summary>
        /// Описание команды в произвольной форме.
        /// </summary>
        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PerformChange();
                }
            }
        }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object? UserData
        {
            get => _userData;
            set
            {
                _userData = value;
                PerformChange();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Command()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="title">Название команды (произвольное).
        /// </param>
        /// <param name="description">Описание команды в произвольной
        /// форме.</param>
        /// <param name="enabled">Команда разрешена к выполнению?</param>
        public Command
            (
                string? title,
                string? description = default,
                bool enabled = true
            )
        {
            _enabled = enabled;
            _title = title;
            _description = description;
        } // constructor

        #endregion

        #region Private members

        private bool _enabled;
        private string? _title, _description;
        private object? _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Выполнение команды в синхронном режиме.
        /// (учитывается состояние флага <see cref="Enabled"/>).
        /// </summary>
        public void PerformExecute()
        {
            if (Enabled)
            {
                Execute?.Invoke(this, EventArgs.Empty);
            }
        } // method PerformExecute

        /// <summary>
        /// Выполнение команды в синхронном режиме
        /// (учитывается состояние флага <see cref="Enabled"/>).
        /// </summary>
        /// <param name="eventArgs">Аргументы для события.</param>
        public void PerformExecute
            (
                EventArgs eventArgs
            )
        {
            if (Enabled)
            {
                Execute?.Invoke(this, eventArgs);
            }
        } // method PerformExecute

        /// <summary>
        /// Выполнение команды в асинхронном режиме
        /// (учитывается состояние флага <see cref="Enabled"/>).
        /// </summary>
        public Task PerformExecuteAsync()
        {
            if (Enabled)
            {
                return Execute.RaiseAsync(this);
            }

            return Task.CompletedTask;
        } // method PerformExecuteAsync

        /// <summary>
        /// Выполнение команды в асинхронном режиме.
        /// </summary>
        /// <param name="eventArgs">Аргументы для события.</param>
        public Task PerformExecuteAsync
            (
                EventArgs eventArgs
            )
        {
            if (Enabled)
            {
                return Execute.RaiseAsync(this, eventArgs);
            }

            return Task.CompletedTask;
        } // method PerformExecuteAsync

        /// <summary>
        /// Команда должна обновить свое состояние.
        /// </summary>
        public void PerformUpdate()
        {
            Update?.Invoke(this, EventArgs.Empty);
        } // method PerformUpdate

        /// <summary>
        /// Команда должна обновить свое состояние.
        /// </summary>
        /// <param name="eventArgs">Аргументы для события.</param>
        public void PerformUpdate
            (
                EventArgs eventArgs
            )
        {
            Update?.Invoke(this, eventArgs);
        } // method PerformUpdate

        /// <summary>
        /// Сигнал, что в команде что-то поменялось.
        /// </summary>
        public void PerformChange()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        } // method PerformChange

        /// <summary>
        /// Сигнал, что в команде что-то поменялось.
        /// </summary>
        /// <param name="eventArgs">Аргументы для события.</param>
        public void PerformChange
            (
                EventArgs eventArgs
            )
        {
            Changed?.Invoke(this, eventArgs);
        } // method PerformChange

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Title}: {Description} [{Enabled}]";

        #endregion

    } // class Command

} // namespace AM.Commands
