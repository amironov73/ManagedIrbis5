// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MemberOrderAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Reflection
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MemberOrderAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MemberOrderAttribute
            (
                int index
            )
        {
            Index = index;
        }

        #endregion
    }
}
