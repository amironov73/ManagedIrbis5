// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CancelableEventArgs.cs -- Event arguments for cancelable handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// <see cref="T:System.EventArgs"/> for cancelable
    /// handling.
    /// </summary>
    public class CancelableEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether
        /// event handling must be canceled.
        /// </summary>
        /// <value><c>true</c> if cancel;
        /// otherwise, <c>false</c>.</value>
        public bool Cancel { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the event with specified sender and handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        /// <returns><c>true</c> if handling was canceled,
        /// <c>false</c> otherwise.</returns>
        public bool Handle
            (
                object sender,
                CancelableEventHandler? handler
            )
        {
            if (handler == null)
            {
                return false;
            }

            var list = handler.GetInvocationList();
            foreach (var eventHandler in list.OfType<CancelableEventHandler>())
            {
                eventHandler(sender, this);
                if (Cancel)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
