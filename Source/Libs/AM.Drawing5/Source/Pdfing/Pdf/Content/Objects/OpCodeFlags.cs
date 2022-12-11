// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* OpCodeFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Pdf.Content.Objects;

/// <summary>
/// Specifies the group of operations the op-code belongs to.
/// </summary>
[Flags]
public enum OpCodeFlags
{
    /// <summary>
    ///
    /// </summary>
    None,

    /// <summary>
    ///
    /// </summary>
    TextOut = 0x0001,

    //Color, Pattern, Images,...
}
