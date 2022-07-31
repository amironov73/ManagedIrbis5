// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SyntaxDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public class SyntaxDescriptor
    : IDisposable
{
    /// <summary>
    ///
    /// </summary>
    public char leftBracket = '(';

    /// <summary>
    ///
    /// </summary>
    public char rightBracket = ')';

    /// <summary>
    ///
    /// </summary>
    public char leftBracket2 = '{';

    /// <summary>
    ///
    /// </summary>
    public char rightBracket2 = '}';

    /// <summary>
    ///
    /// </summary>
    public BracketsHighlightStrategy bracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;

    /// <summary>
    ///
    /// </summary>
    public readonly List<Style> styles = new ();

    /// <summary>
    ///
    /// </summary>
    public readonly List<RuleDesc> rules = new ();

    /// <summary>
    ///
    /// </summary>
    public readonly List<FoldingDesc> foldings = new ();

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var style in styles)
            style.Dispose();
    }
}

/// <summary>
///
/// </summary>
public class RuleDesc
{
    /// <summary>
    ///
    /// </summary>
    Regex regex;

    /// <summary>
    ///
    /// </summary>
    public string pattern;

    /// <summary>
    ///
    /// </summary>
    public RegexOptions options = RegexOptions.None;

    /// <summary>
    ///
    /// </summary>
    public Style style;

    /// <summary>
    ///
    /// </summary>
    public Regex Regex
    {
        get
        {
            if (regex == null)
            {
                regex = new Regex (pattern, SyntaxHighlighter.RegexCompiledOption | options);
            }

            return regex;
        }
    }
}

/// <summary>
///
/// </summary>
public class FoldingDesc
{
    /// <summary>
    ///
    /// </summary>
    public string startMarkerRegex;

    /// <summary>
    ///
    /// </summary>
    public string finishMarkerRegex;

    /// <summary>
    ///
    /// </summary>
    public RegexOptions options = RegexOptions.None;
}
