// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FindTextArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

namespace AM.Reporting.Code;

/// <summary>
/// This class is used to pass find arguments to some methods of the <b>CodeUtils</b> class.
/// </summary>
public class FindTextArgs
{
    /// <summary>
    /// The start position of the search. After the search, this property points to
    /// the begin of an expression.
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// After the search, this property points to the end of an expression.
    /// </summary>
    public int EndIndex { get; set; }

    /// <summary>
    /// The char sequence used to find the expression's begin.
    /// </summary>
    public string? OpenBracket { get; set; }

    /// <summary>
    /// The char sequence used to find the expression's end.
    /// </summary>
    public string? CloseBracket { get; set; }

    /// <summary>
    /// The text with embedded expressions.
    /// </summary>
    public FastString? Text { get; set; }

    /// <summary>
    /// The last found expression.
    /// </summary>
    public string? FoundText { get; set; }
}
