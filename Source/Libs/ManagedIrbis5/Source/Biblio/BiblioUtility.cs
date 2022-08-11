// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BiblioUtility.cs -- полезные методы, применяемые при построении библиографического указателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Полезные методы, применяемые при построении библиографического указателя.
/// </summary>
public static class BiblioUtility
{
    #region Private members

    private static readonly char[] _delimiters = { '.', '!', '?', ')', ':', '}' };

    private static readonly Regex _commandRegex = new (@"\\[a-z]\d+$");

    private static void _AddDot
        (
            StringBuilder builder,
            string? line
        )
    {
        if (!string.IsNullOrEmpty (line))
        {
            line = line.TrimEnd();
            builder.Append (line);
            if (!string.IsNullOrEmpty (line))
            {
                var lastChar = line.LastChar();
                if (!lastChar.IsOneOf (_delimiters)
                    && !_commandRegex.IsMatch (line))
                {
                    builder.Append ('.');
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Add trailing dot to every line in the text.
    /// </summary>
    public static string AddTrailingDot
        (
            string text
        )
    {
        Sure.NotNull (text);

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length + 10);
        var navigator = new TextNavigator (text);
        string? line;
        while (!navigator.IsEOF)
        {
            line = navigator.ReadTo ("\\par").ToString();
            if (string.IsNullOrEmpty (line))
            {
                break;
            }

            var recent = navigator.RecentText (4).ToString();
            var par = false;
            if (recent == "\\par")
            {
                if (navigator.PeekChar() == 'd')
                {
                    builder.Append (line);
                    builder.Append ("\\par");
                    builder.Append (navigator.ReadChar());
                    continue;
                }

                par = true;
            }

            _AddDot (builder, line);

            if (par)
            {
                builder.Append ("\\par");
            }
        }

        line = navigator.GetRemainingText().ToString();
        _AddDot (builder, line);

        return builder.ReturnShared();
    }

    #endregion
}
