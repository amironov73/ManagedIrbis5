// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisBusyStripe.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Threading;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public class IrbisBusyStripe
        : BusyStripe
    {
        #region Private members

        private void Busy_StateChanged
            (
                object sender,
                EventArgs e
            )
        {
            var state = (BusyState)sender;

            this.InvokeIfRequired
                (
                    () =>
                    {
                        Moving = state;
                        Invalidate();
                    }
                );
        }

        /*

        private void Connection_Disposing
            (
                object sender,
                EventArgs e
            )
        {
            IrbisConnection connection = (IrbisConnection)sender;

            UnsubscribeFrom(connection);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Subscribe to the connection busy state.
        /// </summary>
        public void SubscribeTo
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            connection.Busy.StateChanged += Busy_StateChanged;
            connection.Disposing += Connection_Disposing;
        }

        /// <summary>
        /// Unsubscribe from the connection
        /// busy state.
        /// </summary>
        public void UnsubscribeFrom
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            connection.Busy.StateChanged -= Busy_StateChanged;
            connection.Disposing -= Connection_Disposing;
        }

        */

        #endregion
    }
}
