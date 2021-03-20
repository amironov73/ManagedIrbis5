// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSerializationException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftSerializationException
        : IrbisException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSerializationException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion
    }
}
