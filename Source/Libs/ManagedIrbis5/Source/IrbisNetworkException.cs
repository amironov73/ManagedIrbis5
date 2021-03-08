// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IrbisNetworkException.cs -- exception during IRBIS network communication
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Exception during IRBIS network communication.
    /// </summary>
    public sealed class IrbisNetworkException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisNetworkException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisNetworkException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisNetworkException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion

    } // class IrbisNetworkException

} // namespace ManagedIrbis
