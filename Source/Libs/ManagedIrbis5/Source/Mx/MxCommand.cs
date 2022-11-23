// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MxCommand
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Fired before <see cref="Execute"/>.
        /// </summary>
        public event EventHandler? BeforeExecute;

        /// <summary>
        /// Fired after <see cref="Execute"/>.
        /// </summary>
        public event EventHandler? AfterExecute;

        #endregion

        #region Properties

        /// <summary>
        /// Main name of the command.
        /// </summary>
        public string Name { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MxCommand
            (
            )
            : this ("Unnamed")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MxCommand
            (
                string name
            )
        {
            Name = name;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Raises <see cref="BeforeExecute"/> event.
        /// </summary>
        protected virtual void OnBeforeExecute() =>
            BeforeExecute?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raises <see cref="AfterExecute"/> event.
        /// </summary>
        protected virtual void OnAfterExecute() =>
            AfterExecute?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize commands before using.
        /// </summary>
        public virtual void Initialize
            (
                MxExecutive executive
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <returns></returns>
        public virtual bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            executive.WriteLine("Connect");

            OnAfterExecute();

            return true;
        }

        /// <summary>
        /// Help message.
        /// </summary>
        public virtual string? GetShortHelp()
        {
            return null;
        }

        /// <summary>
        /// Help message.
        /// </summary>
        public virtual string? GetLongHelp()
        {
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool RecognizeLine
            (
                string line
            )
        {
            return false;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
