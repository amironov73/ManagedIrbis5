// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EditMargins.cs -- specifies what margin of edit control to set
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Specifies what margin of edit control to set.
    /// </summary>
    [Flags]
    public enum EditMargins
    {
        /// <summary>
        /// Sets the left margin.
        /// </summary>
        EC_LEFTMARGIN = 1,

        /// <summary>
        /// Sets the right margin.
        /// </summary>
        EC_RIGHTMARGIN = 2,

        /// <summary>
        /// Sets the left and right margins to a narrow
        /// width calculated using the text metrics of the
        /// control's current font. If no font has been
        /// set for the control, the margins are set to
        /// zero.
        /// </summary>
        EC_USEFONTINFO = 0xFFFF

    } // enum EditMargins

} // namespace AM.Win32
