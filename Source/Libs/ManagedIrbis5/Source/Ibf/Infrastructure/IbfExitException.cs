// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IbfExitException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Ibf.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class IbfExitException
        : IrbisException
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                int returnCode
            )
            : base(returnCode)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbfExitException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
