// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* StringifierStrategy.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Formatting;

/// <summary>
///
/// </summary>
public class StringifierStrategy
{
    /// <summary>
    ///
    /// </summary>
    public Func<GetStringRequest, bool>? Matches;

    /// <summary>
    ///
    /// </summary>
    public Func<GetStringRequest, string>? StringFunction;
}
