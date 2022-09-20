// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

/* ReadOnlyException.cs -- specific for IReadOnly interface
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM
{
    /// <summary>
    /// Specific for IReadOnly interface.
    /// </summary>
    public sealed class ReadOnlyException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException
            (
                string message
            )
            : base(message)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        } // constructor

        #endregion

    } // class ReadOnlyException

} // namespace AM
