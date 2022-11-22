// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EnglishLoremIpsum.cs -- выдает бред на английском языке
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
/// Выдает бред на английском языке.
/// </summary>
public sealed class EnglishLoremIpsum
    : ILoremIpsum
{
    private static readonly Regex _wordSplitter = new (@"\w", RegexOptions.Compiled);

    private const string Original = "Brock would have never dared to do it on his own he thought to himself. That is why Kenneth and he had become such good friends. Kenneth forced Brock out of his comfort zone and made him try new things he'd never imagine doing otherwise. Up to this point, this had been a good thing. It had expanded Brock's experiences and given him a new appreciation for life. Now that both of them were in the back of a police car, all Brock could think was that he would have never dared do it except for the influence of Kenneth.";

    private readonly List<string> _arrOriginal = new ();

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EnglishLoremIpsum()
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
