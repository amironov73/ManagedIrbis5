// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CharComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Text.Searching;

internal sealed class OrdinalCharComparer
    : CharComparer
{
    private readonly bool _ignoreCase;

    public OrdinalCharComparer (bool ignoreCase = false)
    {
        _ignoreCase = ignoreCase;
    }

    public override bool Equals (char x, char y)
    {
        return _ignoreCase
            ? ((uint) char.ToUpperInvariant (x)).Equals (char.ToUpperInvariant (y))
            : ((uint) x).Equals (y);
    }

    public override int GetHashCode (char obj)
    {
        return _ignoreCase ? (int) char.ToUpperInvariant (obj) : (int)obj;
    }
}

class CultureCharComparer : CharComparer
{
    private readonly StringComparer _stringComparer;

    public CultureCharComparer (CultureInfo cultureInfo, bool ignoreCase = false)
    {
        _stringComparer = StringComparer.Create (cultureInfo, ignoreCase);
    }

    public override bool Equals (char x, char y)
    {
        return _stringComparer.Equals (x.ToString(), y.ToString());
    }

    public override int GetHashCode (char obj)
    {
        return _stringComparer.GetHashCode (obj.ToString());
    }
}

/// <summary>
/// Represents a char comparison operation that uses specific case and culture-based or ordinal comparison rules.
/// </summary>
public abstract class CharComparer
    : EqualityComparer<char>
{
    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-insensitive ordinal comparison.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer OrdinalIgnoreCase { get; } = new OrdinalCharComparer (ignoreCase: true);

    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-sensitive ordinal comparison.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer Ordinal { get; } = new OrdinalCharComparer (ignoreCase: false);

    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-insensitive comparison using the comparison rules of the invariant culture.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer InvariantCultureIgnoreCase { get; } = new CultureCharComparer (CultureInfo.InvariantCulture, ignoreCase: true);

    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-sensitive comparison using the comparison rules of the invariant culture.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer InvariantCulture { get; } = new CultureCharComparer (CultureInfo.InvariantCulture, ignoreCase: false);

    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-sensitive comparison using the comparison rules of the current culture.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer CurrentCulture => new CultureCharComparer (CultureInfo.CurrentCulture, ignoreCase: false);

    /// <summary>
    /// Gets a <see cref="CharComparer"/> object that performs a case-insensitive comparison using the comparison rules of the current culture.
    /// </summary>
    /// <value>
    /// A <see cref="CharComparer"/> object.
    /// </value>
    public static CharComparer CurrentCultureIgnoreCase => new CultureCharComparer (CultureInfo.CurrentCulture, ignoreCase: true);

    /// <summary>
    /// Creates a <see cref="CharComparer"/> object that compares characters according to the rules of a specified culture.
    /// </summary>
    /// <param name="cultureInfo">A culture whose linguistic rules are used to perform a string comparison.</param>
    /// <param name="ignoreCase">true to specify that comparison operations be case-insensitive; false to specify that comparison operations be case-sensitive.</param>
    /// <returns>A new <see cref="CharComparer"/> object that performs character comparisons according to the comparison rules used by the <paramref name="cultureInfo"/> parameter and the case rule specified by the <paramref name="ignoreCase"/> parameter.</returns>
    public static CharComparer Create (CultureInfo cultureInfo, bool ignoreCase)
    {
        return new CultureCharComparer (cultureInfo, ignoreCase);
    }
}
