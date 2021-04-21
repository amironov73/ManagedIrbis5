// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsnClass.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Asn1
{
    /// <summary>
    /// Enumeration of possible classes in
    /// Identifier octet of the ASN.1 node.
    /// </summary>
    [Flags]
    public enum AsnClass
    {
        /// <summary>
        /// The type is native to ASN.1.
        /// </summary>
        Universal = 0,

        /// <summary>
        /// The type is only valid for one specific application
        /// </summary>
        Application = 64,

        /// <summary>
        /// Meaning of this type depends on the context
        /// (such as within a sequence, set or choice)
        /// </summary>
        ContextSpecific = 128,

        /// <summary>
        /// Defined in private specifications
        /// </summary>
        Private = 192
    }
}
