// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

/// <summary>
/// Specifies the text writing direction for text.
/// </summary>
public enum TextDirection
{
    /// <summary>
    /// Left to right.
    /// </summary>
    LTR,

    /// <summary>
    /// Right to left.
    /// </summary>
    RTL,

    /// <summary>
    /// Automatic
    /// </summary>
    Auto,
}
