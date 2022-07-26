// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RussianStringComparer.cs -- сравнивает строки согласно русской локали
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Globalization;

/// <summary>
/// Сравнивает строки согласно русской локали.
/// </summary>
public class RussianStringComparer
    : StringComparer
{
    #region Properties

    ///<summary>
    /// Считать Ё отдельной буквой (иначе - считать ее равной Е)?
    ///</summary>
    public bool ConsiderYo { get; }

    ///<summary>
    /// Игнорировать регистр символов?
    ///</summary>
    public bool IgnoreCase { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="considerYo">Считать Ё отдельной буквой?</param>
    /// <param name="ignoreCase">Игнорировать регистр символов?</param>
    public RussianStringComparer
        (
            bool considerYo = false,
            bool ignoreCase = false
        )
    {
        ConsiderYo = considerYo;
        IgnoreCase = ignoreCase;

        var russianCulture = BuiltinCultures.Russian;

        var options = ignoreCase
            ? CompareOptions.IgnoreCase
            : CompareOptions.None;

        _innerComparer = (left, right)
            => russianCulture.CompareInfo.Compare
                (
                    left,
                    right,
                    options
                );
    }

    #endregion

    #region Private members

    private readonly Func<string?, string?, int> _innerComparer;

    private string? _Replace
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return null;
        }

        if (ConsiderYo)
        {
            text = text.Replace ('ё', 'е')
                .Replace ('Ё', 'Е');
        }

        return text;
    }

    #endregion

    #region StringComparer members

    /// <inheritdoc cref="StringComparer.Compare(string?,string?)" />
    public override int Compare
        (
            string? x,
            string? y
        )
    {
        var xCopy = _Replace (x);
        var yCopy = _Replace (y);

        return _innerComparer (xCopy, yCopy);
    }

    /// <inheritdoc cref="StringComparer.Equals(string?,string?)" />
    public override bool Equals
        (
            string? x,
            string? y
        )
    {
        var xCopy = _Replace (x);
        var yCopy = _Replace (y);

        return _innerComparer (xCopy, yCopy) == 0;
    }

    /// <inheritdoc cref="StringComparer.GetHashCode(string)"/>
    public override int GetHashCode
        (
            string obj
        )
    {
        var objCopy = _Replace (obj);

        if (IgnoreCase && objCopy is not null)
        {
            objCopy = objCopy.ToUpper();
        }

        return objCopy is null
            ? 0
            : objCopy.GetHashCode();
    }

    #endregion
}
