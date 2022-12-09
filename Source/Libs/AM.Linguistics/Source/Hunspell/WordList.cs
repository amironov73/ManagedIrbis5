// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global

/* WordList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed partial class WordList
{
    internal const int MaxWordLen = 100;

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryStream"></param>
    /// <param name="affixStream"></param>
    /// <returns></returns>
    public static WordList CreateFromStreams
        (
            Stream dictionaryStream,
            Stream affixStream
        )
    {
        return WordListReader.Read (dictionaryStream, affixStream);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryFilePath"></param>
    /// <returns></returns>
    public static WordList CreateFromFiles
        (
            string dictionaryFilePath
        )
    {
        return WordListReader.ReadFile (dictionaryFilePath);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryFilePath"></param>
    /// <param name="affixFilePath"></param>
    /// <returns></returns>
    public static WordList CreateFromFiles
        (
            string dictionaryFilePath,
            string affixFilePath
        )
    {
        return WordListReader.ReadFile (dictionaryFilePath, affixFilePath);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryStream"></param>
    /// <param name="affixStream"></param>
    /// <returns></returns>
    public static async Task<WordList> CreateFromStreamsAsync
        (
            Stream dictionaryStream,
            Stream affixStream
        )
    {
        return await WordListReader.ReadAsync (dictionaryStream, affixStream).ConfigureAwait (false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryFilePath"></param>
    /// <returns></returns>
    public static async Task<WordList> CreateFromFilesAsync
        (
            string dictionaryFilePath
        )
    {
        return await WordListReader.ReadFileAsync (dictionaryFilePath).ConfigureAwait (false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dictionaryFilePath"></param>
    /// <param name="affixFilePath"></param>
    /// <returns></returns>
    public static async Task<WordList> CreateFromFilesAsync
        (
            string dictionaryFilePath,
            string affixFilePath
        )
    {
        return await WordListReader.ReadFileAsync (dictionaryFilePath, affixFilePath).ConfigureAwait (false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    public static WordList CreateFromWords
        (
            IEnumerable<string> words
        )
    {
        return CreateFromWords (words, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="words"></param>
    /// <param name="affix"></param>
    /// <returns></returns>
    public static WordList CreateFromWords
        (
            IEnumerable<string>? words,
            AffixConfig? affix
        )
    {
        words ??= Enumerable.Empty<string>();

        var wordListBuilder = new Builder (affix ?? new AffixConfig.Builder().MoveToImmutable());

        if (words is IList<string> wordsAsList)
        {
            wordListBuilder.InitializeEntriesByRoot (wordsAsList.Count);
        }
        else
        {
            wordListBuilder.InitializeEntriesByRoot (-1);
        }

        var entryDetail = WordEntryDetail.Default;

        foreach (var word in words)
        {
            wordListBuilder.Add (word, entryDetail);
        }

        return wordListBuilder.MoveToImmutable();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="affix"></param>
    private WordList (AffixConfig affix)
    {
        NGramRestrictedDetails = null!;

        Affix = affix;
    }

    /// <summary>
    ///
    /// </summary>
    public AffixConfig Affix { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public SingleReplacementSet? AllReplacements { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<string> RootWords => EntriesByRoot!.Keys;

    /// <summary>
    ///
    /// </summary>
    public bool HasEntries => EntriesByRoot!.Count != 0;

    /// <summary>
    ///
    /// </summary>
    /// <param name="rootWord"></param>
    /// <returns></returns>
    public bool ContainsEntriesForRootWord
        (
            string? rootWord
        )
    {
        return rootWord != null && EntriesByRoot!.ContainsKey (rootWord);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rootWord"></param>
    public WordEntryDetail[] this [string? rootWord] =>
        rootWord != null
            ? (WordEntryDetail[])FindEntryDetailsByRootWord (rootWord).Clone()
            : Array.Empty<WordEntryDetail>();

    private Dictionary<string, WordEntryDetail[]>? EntriesByRoot { get; set; }

    /// <summary>
    ///
    /// </summary>
    private FlagSet? NGramRestrictedFlags { get; set; }

    private NGramAllowedEntries GetNGramAllowedDetails
        (
            Func<string, bool> rootKeyFilter
        )
    {
        return new (this, rootKeyFilter);
    }

    private Dictionary<string, WordEntryDetail[]> NGramRestrictedDetails { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public bool Check
        (
            string word
        )
    {
        return new QueryCheck (this).Check (word);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public SpellCheckResult CheckDetails
        (
            string word
        )
    {
        return new QueryCheck (this).CheckDetails (word);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public IEnumerable<string> Suggest
        (
            string word
        )
    {
        return new QuerySuggest (this).Suggest (word);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rootWord"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal WordEntry? FindFirstEntryByRootWord
        (
            string rootWord
        )
    {
#if DEBUG
        if (rootWord == null)
        {
            throw new ArgumentNullException (nameof (rootWord));
        }
#endif
        var details = FindEntryDetailsByRootWord (rootWord);
        return details.Length == 0
            ? null
            : new WordEntry (rootWord, details[0]);
    }

    internal WordEntryDetail[] FindEntryDetailsByRootWord
        (
            string? rootWord
        )
    {
#if DEBUG
        if (rootWord == null)
        {
            throw new ArgumentNullException (nameof (rootWord));
        }
#endif
        return rootWord == null! || !EntriesByRoot!.TryGetValue (rootWord, out var details)
            ? Array.Empty<WordEntryDetail>()
            : details;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rootWord"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal WordEntryDetail? FindFirstEntryDetailByRootWord
        (
            string rootWord
        )
    {
#if DEBUG
        if (rootWord == null)
        {
            throw new ArgumentNullException (nameof (rootWord));
        }
#endif

        return EntriesByRoot.TryGetValue (rootWord, out var details) && details.Length != 0
            ? details[0]
            : null;
    }

    private class NGramAllowedEntries
        : IEnumerable<KeyValuePair<string, WordEntryDetail[]>>
    {
        public NGramAllowedEntries
            (
                WordList wordList,
                Func<string, bool> rootKeyFilter
            )
        {
            this.wordList = wordList;
            this.rootKeyFilter = rootKeyFilter;
        }

        private readonly WordList wordList;

        private readonly Func<string, bool> rootKeyFilter;

        public Enumerator GetEnumerator()
        {
            return new (wordList.EntriesByRoot!, wordList.NGramRestrictedDetails, rootKeyFilter);
        }

        IEnumerator<KeyValuePair<string, WordEntryDetail[]>> IEnumerable<KeyValuePair<string, WordEntryDetail[]>>.
            GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator
            : IEnumerator<KeyValuePair<string, WordEntryDetail[]>>
        {
            public Enumerator (Dictionary<string, WordEntryDetail[]> entriesByRoot,
                Dictionary<string, WordEntryDetail[]> nGramRestrictedDetails, Func<string, bool> rootKeyFilter)
            {
                coreEnumerator = entriesByRoot.GetEnumerator();
                this.entriesByRoot = entriesByRoot;
                this.nGramRestrictedDetails = nGramRestrictedDetails;
                this.rootKeyFilter = rootKeyFilter;
                requiresNGramFiltering = nGramRestrictedDetails != null! && nGramRestrictedDetails.Count != 0;
            }

            private Dictionary<string, WordEntryDetail[]>.Enumerator coreEnumerator;
            private Dictionary<string, WordEntryDetail[]> entriesByRoot;
            private Dictionary<string, WordEntryDetail[]> nGramRestrictedDetails;
            private Func<string, bool> rootKeyFilter;
            private bool requiresNGramFiltering;

            public KeyValuePair<string, WordEntryDetail[]> Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (coreEnumerator.MoveNext())
                {
                    var rootPair = coreEnumerator.Current;
                    if (!rootKeyFilter (rootPair.Key))
                    {
                        continue;
                    }

                    if (requiresNGramFiltering)
                    {
                        if (nGramRestrictedDetails.TryGetValue (rootPair.Key.ToString(), out var restrictedDetails))
                        {
                            if (restrictedDetails.Length != 0)
                            {
                                var filteredValues = rootPair.Value;
                                if (restrictedDetails.Length == rootPair.Value.Length)
                                {
                                    continue;
                                }
                                else
                                {
                                    filteredValues = filteredValues.Where (d => !restrictedDetails.Contains (d))
                                        .ToArray();
                                }

                                rootPair = new KeyValuePair<string, WordEntryDetail[]> (rootPair.Key, filteredValues);
                            }
                        }
                    }

                    Current = rootPair;
                    return true;
                }

                Current = default;
                return false;
            }

            public void Reset()
            {
                ((IEnumerator)coreEnumerator).Reset();
            }

            public void Dispose()
            {
                coreEnumerator.Dispose();
            }
        }
    }
}
