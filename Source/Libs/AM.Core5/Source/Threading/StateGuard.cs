// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StateGuard.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable disable

namespace AM.Threading
{
    /// <summary>
    ///
    /// </summary>
    public sealed class StateGuard<T>
        : IDisposable
        where T: IEquatable<T>
    {
        #region Properties

        /// <summary>
        /// Current value.
        /// </summary>
        public T CurrentValue => _state.Value;

        /// <summary>
        /// Saved value.
        /// </summary>
        public T SavedValue { get; }

        /// <summary>
        /// State.
        /// </summary>
        public StateHolder<T> State => _state;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StateGuard
            (
                StateHolder<T> state
            )
        {
            _state = state;
            SavedValue = state.Value;
        }

        #endregion

        #region Private members

        private readonly StateHolder<T> _state;

        private void _RestoreValue()
        {
            T currentValue = CurrentValue;
            T savedValue = SavedValue;

            bool null1 = ReferenceEquals(currentValue, null);
            bool null2 = ReferenceEquals(savedValue, null);

            bool restore = null1 != null2;

            if (!restore)
            {
                if (!null1)
                {
                    restore = !currentValue.Equals(savedValue);
                }
            }

            if (restore)
            {
                State.SetValue(savedValue);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _RestoreValue();
        }

        #endregion
    }
}
