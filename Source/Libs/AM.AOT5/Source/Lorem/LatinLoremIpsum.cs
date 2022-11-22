// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LatinLoremIpsum.cs -- выдает Lorem Ipsum на псевдо-латыни
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Text.Lorem;

/// <summary>
/// Выдает Lorem Ipsum на псевдо-латыни.
/// </summary>
/// <remarks>
/// Based on Javascript Version of Marcus Campbell - Version 2.0 (Copyright 2003 - 2005)
/// Open-source code under the GNU GPL: http://www.gnu.org/licenses/gpl.txt
/// </remarks>
public sealed class LatinLoremIpsum
    : ILoremIpsum
{
    private static readonly Regex _wordSplitter = new (@"\w", RegexOptions.Compiled);

    private const string Original = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    private readonly List<string> _arrOriginal = new ();

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LatinLoremIpsum()
    {
        var matches = _wordSplitter.Matches (Original);
        foreach (var match in matches)
        {
            _arrOriginal.Add (match.ToString()!);
        }
    }

    /// <inheritdoc cref="ILoremIpsum.GetLetters"/>
    public string GetLetters (int length)
    {
        var output = new StringBuilder();

        if (length == 0)
        {
            length = Original.Length;
        }

        for (var i = 0; i < length; i++)
            output.Append (Original[i]);

        return output.ToString();
    }

    /// <inheritdoc cref="ILoremIpsum.GetWords"/>
    public string GetWords
        (
            int length
        )
    {
        var output = new StringBuilder();

        //var pattern = @"[\w\!\.\?\;\,\:]+/g";

        var arrCounter = 0;
        for (var i = 0; i <= length; i++)
        {
            output.Append (_arrOriginal[arrCounter]);
            if (++arrCounter > _arrOriginal.Count)
            {
                arrCounter = 0;
            }
        }

        return output.ToString();
    }

    /// <inheritdoc cref="ILoremIpsum.GetParagraphs"/>
    public string GetParagraphs
        (
            int count
        )
    {
        var output = new StringBuilder();
        for (var i = 0; i <= count; i++)
        {
            if (i == count)
            {
                output.Append (Original);
            }
            else
            {
                output.Append (Original + "\n\n");
            }
        }

        return output.ToString();
    }
}
