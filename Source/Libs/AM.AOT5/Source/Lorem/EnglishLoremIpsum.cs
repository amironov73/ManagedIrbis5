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
using System.Text.RegularExpressions;

#endregion

namespace AM.Text.Lorem;

/// <summary>
/// Выдает бред на английском языке.
/// </summary>
public sealed class EnglishLoremIpsum
    : ILoremIpsum
{
    private static readonly Regex _wordSplitter = new (@"\w", RegexOptions.Compiled);

    private const string Original = "Brock would have never dared to do it on his own he thought to himself. That is why Kenneth and he had become such good friends. Kenneth forced Brock out of his comfort zone and made him try new things he'd never imagine doing otherwise. Up to this point, this had been a good thing. It had expanded Brock's experiences and given him a new appreciation for life. Now that both of them were in the back of a police car, all Brock could think was that he would have never dared do it except for the influence of Kenneth.";

    private readonly List<string> _originalWords = new ();

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EnglishLoremIpsum()
    {
        var matches = _wordSplitter.Matches (Original);
        foreach (var match in matches)
        {
            _originalWords.Add (match.ToString()!);
        }
    }

    /// <inheritdoc cref="ILoremIpsum.GetLetters"/>
    public string GetLetters
        (
            int length
        )
    {
        Sure.NonNegative (length);

        if (length == 0)
        {
            length = Original.Length;
        }

        var result = StringBuilderPool.Shared.Get();
        while (result.Length < length)
        {
            var remaining = (length - result.Length) % Original.Length;
            for (var i = 0; i < remaining; i++)
            {
                result.Append (Original[i]);
            }

            if (result.Length < length)
            {
                result.Append (' ');
            }
        }

        return result.ReturnShared();
    }

    /// <inheritdoc cref="ILoremIpsum.GetWords"/>
    public string GetWords
        (
            int length
        )
    {
        Sure.NonNegative (length);

        var result = StringBuilderPool.Shared.Get();
        var counter = 0;
        for (var i = 0; i <= length; i++)
        {
            if (result.Length != 0)
            {
                result.Append (' ');
            }

            result.Append (_originalWords[counter]);
            if (++counter > _originalWords.Count)
            {
                counter = 0;
            }
        }

        return result.ReturnShared();
    }

    /// <inheritdoc cref="ILoremIpsum.GetParagraphs"/>
    public string GetParagraphs
        (
            int count
        )
    {
        Sure.NonNegative (count);

        var result = StringBuilderPool.Shared.Get();
        for (var i = 0; i <= count; i++)
        {
            if (i == count)
            {
                result.Append (Original);
            }
            else
            {
                result.Append (Original + "\n\n");
            }
        }

        return result.ReturnShared();
    }
}
