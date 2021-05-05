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
        : ICommand
    {
        #region Events

        /// <inheritdoc cref="ICommand.Execute"/>
        public event EventHandler? Execute;

        /// <inheritdoc cref="ICommand.Update"/>
        public event EventHandler? Update;

        /// <inheritdoc cref="ICommand.Changed"/>
        public event EventHandler? Changed;

        /// <inheritdoc cref="ICommand.Disposed"/>
        public event EventHandler? Disposed;

        #endregion

        #region Properties

        /// <inheritdoc cref="ICommand.Enabled"/>
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

        /// <inheritdoc cref="ICommand.PerformExecute()"/>
        public virtual void PerformExecute()
        {
            if (Enabled)
            {
                Execute?.Invoke(this, EventArgs.Empty);
            }
        } // method PerformExecute

        /// <inheritdoc cref="ICommand.PerformExecute(System.EventArgs)"/>
        public virtual void PerformExecute
            (
                EventArgs eventArgs
            )
        {
            if (Enabled)
            {
                Execute?.Invoke(this, eventArgs);
            }
        } // method PerformExecute

        /// <inheritdoc cref="ICommand.PerformExecuteAsync()"/>
        public virtual Task PerformExecuteAsync()
        {
            if (Enabled)
            {
                return Execute.RaiseAsync(this);
            }

            return Task.CompletedTask;
        } // method PerformExecuteAsync

        /// <inheritdoc cref="ICommand.PerformExecuteAsync(System.EventArgs)"/>
        public virtual Task PerformExecuteAsync
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

        /// <inheritdoc cref="ICommand.PerformUpdate()"/>
        public virtual void PerformUpdate()
        {
            Update?.Invoke(this, EventArgs.Empty);
        } // method PerformUpdate

        /// <inheritdoc cref="ICommand.PerformUpdate(System.EventArgs)"/>
        public virtual void PerformUpdate
            (
                EventArgs eventArgs
            )
        {
            Update?.Invoke(this, eventArgs);
        } // method PerformUpdate

        /// <inheritdoc cref="ICommand.PerformUpdateAsync()"/>
        public virtual Task PerformUpdateAsync()
        {
            return Update.RaiseAsync(this);
        } // method PerformUpdateAsync

        /// <inheritdoc cref="ICommand.PerformUpdateAsync(System.EventArgs)"/>
        public virtual Task PerformUpdateAsync
            (
                EventArgs eventArgs
            )
        {
            return Update.RaiseAsync(this, eventArgs);
        } // method PerformUpdateAsync

        /// <inheritdoc cref="ICommand.PerformChange()"/>
        public virtual void PerformChange()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        } // method PerformChange

        /// <inheritdoc cref="ICommand.PerformChange(System.EventArgs)"/>
        public virtual void PerformChange
            (
                EventArgs eventArgs
            )
        {
            Changed?.Invoke(this, eventArgs);
        } // method PerformChange

        /// <inheritdoc cref="ICommand.PerformChangeAsync()"/>
        public virtual Task PerformChangeAsync()
        {
            return Changed.RaiseAsync(this);
        } // method PerformChangeAsync

        /// <inheritdoc cref="ICommand.PerformChangeAsync(System.EventArgs)"/>
        public virtual Task PerformChangeAsync
            (
                EventArgs eventArgs
            )
        {
            return Changed.RaiseAsync(this, eventArgs);
        } // method PerformChangeAsync

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
