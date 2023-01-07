// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AuthorUtility.cs -- полезные методы для обработки ФИО автора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM.Linq;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Полезные методы для обработки ФИО автора.
/// </summary>
public static class AuthorUtility
{
    #region Public methods

    /// <summary>
    /// Представление в виде "Иванов, Иван Иванович".
    /// </summary>
    public static string? WithComma
        (
            string? rawText
        )
    {
        if (string.IsNullOrEmpty (rawText))
        {
            return rawText;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (rawText.Length);
        foreach (var c in rawText)
        {
            if (char.IsLetter (c) || c is ' ' or '-')
            {
                builder.Append (c);
            }
        }

        var text = new Sparcer().SparceText (builder.ToString());
        if (!text.Contains (' '))
        {
            return text;
        }

        var navigator = new ValueTextNavigator (text);
        builder.Clear();
        while (true)
        {
            var chr = navigator.ReadChar();
            if (char.IsLetter (chr) || chr == '-')
            {
                builder.Append (chr);
            }
            else
            {
                break;
            }
        }

        if (builder.Length != 0)
        {
            builder.Append (',');
            builder.Append (' ');
        }

        navigator.SkipWhitespace();
        builder.Append (navigator.GetRemainingText().ToString());

        return builder.ReturnShared();
    }

    /// <summary>
    /// Представление в виде "Иванов Иван Иванович".
    /// </summary>
    public static string? WithoutComma
        (
            string? rawText
        )
    {
        if (string.IsNullOrEmpty (rawText))
        {
            return rawText;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (rawText.Length);
        foreach (var c in rawText)
        {
            if (char.IsLetter (c) || c is ' ' or '-')
            {
                builder.Append (c);
            }
        }

        var text = new Sparcer().SparceText (builder.ToString());
        if (!text.Contains (' '))
        {
            return text;
        }

        var navigator = new ValueTextNavigator (text);
        builder.Clear();
        while (true)
        {
            var chr = navigator.ReadChar();
            if (char.IsLetter (chr) || chr == '-')
            {
                builder.Append (chr);
            }
            else
            {
                break;
            }
        }

        if (builder.Length != 0)
        {
            builder.Append (' ');
        }

        navigator.SkipWhitespace();
        builder.Append (navigator.GetRemainingText().ToString());

        return builder.ReturnShared();
    }

    /// <summary>
    /// Получение массива, в первом элементе которого "Иванов, Иван Иванович",
    /// а во втором - "Иванов Иван Иванович".
    /// Если такое представление невозможно, возвращается массив с одним
    /// элементом, либо вообще <c>null</c>.
    /// </summary>
    public static string[]? WithAndWithoutComma
        (
            string? rawText
        )
    {
        if (string.IsNullOrEmpty (rawText))
        {
            return null;
        }

        var withComma = WithComma (rawText);
        var withoutComma = WithoutComma (rawText);
        var result = Sequence.FromItems (withComma, withoutComma)
            .NonEmptyLines()
            .ToArray();
        if (result.Length == 2 && result[0] == result[1])
        {
            result = new[] { result[0] };
        }

        return result;
    }

    #endregion
}
