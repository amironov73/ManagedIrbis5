// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RegexNode.cs -- регулярное выражение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Регулярное выражение
/// </summary>
internal sealed class RegexNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RegexNode
        (
            string value
        )
    {
        Sure.NotNullNorEmpty (value);

        _value = value;
    }

    #endregion

    #region Private members

    private readonly string _value;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        return new Regex (_value);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"regex '{_value}'";
    }

    #endregion
}
