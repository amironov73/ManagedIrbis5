// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* MultiReplacement.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AM.Text;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public class MultiReplacementTable
    : IReadOnlyDictionary<string, MultiReplacementEntry>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly MultiReplacementTable Empty =
        TakeDictionary (new Dictionary<string, MultiReplacementEntry> (0));

    /// <summary>
    ///
    /// </summary>
    /// <param name="replacements"></param>
    /// <returns></returns>
    public static MultiReplacementTable Create
        (
            IEnumerable<KeyValuePair<string, MultiReplacementEntry>> replacements
        )
    {
        return replacements == null! ? Empty : TakeDictionary (replacements.ToDictionary (s => s.Key, s => s.Value));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="replacements"></param>
    /// <returns></returns>
    internal static MultiReplacementTable TakeDictionary (Dictionary<string, MultiReplacementEntry> replacements)
    {
        return replacements == null! ? Empty : new MultiReplacementTable (replacements);
    }

    private MultiReplacementTable (Dictionary<string, MultiReplacementEntry> replacements)
    {
        this.replacements = replacements;
    }

    private readonly Dictionary<string, MultiReplacementEntry> replacements;

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    public MultiReplacementEntry this [string key] => replacements[key];

    /// <summary>
    ///
    /// </summary>
    public int Count => replacements.Count;

    /// <summary>
    ///
    /// </summary>
    public bool HasReplacements => replacements.Count != 0;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<string> Keys => replacements.Keys;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<MultiReplacementEntry> Values => replacements.Values;

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey (string key)
    {
        return replacements.ContainsKey (key);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue (string key, out MultiReplacementEntry value)
    {
        return replacements.TryGetValue (key, out value!);
    }

    internal bool TryConvert (string text, out string converted)
    {
        Sure.NotNull (text);

        var appliedConversion = false;

        if (text.Length == 0)
        {
            converted = text;
        }
        else
        {
            var convertedBuilder = StringBuilderPool.Shared.Get();
            convertedBuilder.EnsureCapacity (text.Length);

            for (var i = 0; i < text.Length; i++)
            {
                var replacementEntry = FindLargestMatchingConversion (text.AsSpan (i));
                if (replacementEntry != null)
                {
                    var replacementText = replacementEntry.ExtractReplacementText (text.Length - i, i == 0);
                    if (!string.IsNullOrEmpty (replacementText))
                    {
                        convertedBuilder.Append (replacementText);
                        i += replacementEntry.Pattern.Length - 1;
                        appliedConversion = true;
                        continue;
                    }
                }

                convertedBuilder.Append (text[i]);
            }

            converted = convertedBuilder.ReturnShared();
        }

        return appliedConversion;
    }

    /// <summary>
    /// Finds a conversion matching the longest version of the given <paramref name="text"/> from the left.
    /// </summary>
    /// <param name="text">The text to find a matching input conversion for.</param>
    /// <returns>The best matching input conversion.</returns>
    /// <seealso cref="MultiReplacementEntry"/>
    internal MultiReplacementEntry? FindLargestMatchingConversion (ReadOnlySpan<char> text)
    {
        for (var searchLength = text.Length; searchLength > 0; searchLength--)
        {
            if (replacements.TryGetValue (text.Slice (0, searchLength).ToString(), out var entry))
            {
                return entry;
            }
        }

        return null;
    }

    internal Dictionary<string, MultiReplacementEntry>.Enumerator GetEnumerator()
    {
        return replacements.GetEnumerator();
    }

    IEnumerator<KeyValuePair<string, MultiReplacementEntry>>
        IEnumerable<KeyValuePair<string, MultiReplacementEntry>>.GetEnumerator()
    {
        return replacements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return replacements.GetEnumerator();
    }
}
