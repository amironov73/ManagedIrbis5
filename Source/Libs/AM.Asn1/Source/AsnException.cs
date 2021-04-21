// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsnException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    public class AsnException
        : Exception
    {
        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnException
            (
                string message,
                Exception innerException
            )
            : base
            (
                message,
                innerException
            )
        {
        }

        #endregion
    }
}
