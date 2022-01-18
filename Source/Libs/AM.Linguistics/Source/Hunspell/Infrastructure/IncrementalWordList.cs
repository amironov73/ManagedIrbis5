// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure
{
    class IncrementalWordList
    {
        public IncrementalWordList()
            : this(new List<WordEntryDetail>(), 0) { }

        public IncrementalWordList(List<WordEntryDetail> words, int wNum)
        {
#if DEBUG
            if (words == null) throw new ArgumentNullException(nameof(words));
            if (WNum < 0) throw new ArgumentOutOfRangeException(nameof(wNum));
#endif
            Words = words;
            WNum = wNum;
        }

        public List<WordEntryDetail> Words { get; }

        public int WNum { get; }

        public void SetCurrent(WordEntryDetail value)
        {
            if (WNum == Words.Count)
            {
                Words.Add(value);
            }
            else if (WNum < Words.Count)
            {
                Words[WNum] = value;
            }
            else
            {
                AppendForCurrent(value);
            }
        }

        private void AppendForCurrent(WordEntryDetail value)
        {
            var blanksToInsert = WNum - Words.Count;
            for (var i = WNum - Words.Count; i > 0; i--)
            {
                Words.Add(null);
            }

            Words.Add(value);
        }

        public void ClearCurrent()
        {
            if (WNum < Words.Count)
            {
                Words[WNum] = null;
            }
        }

        public bool CheckIfCurrentIsNotNull() =>
            CheckIfNotNull(WNum);

        public bool CheckIfNextIsNotNull() =>
            CheckIfNotNull(WNum + 1);

        private bool CheckIfNotNull(int index) =>
            (index < Words.Count) && Words[index] != null;

        public bool ContainsFlagAt(int wordIndex, FlagValue flag)
        {
#if DEBUG
            if (wordIndex < 0) throw new ArgumentOutOfRangeException(nameof(wordIndex));
#endif

            if (wordIndex < Words.Count)
            {
                var detail = Words[wordIndex];
                if (detail != null)
                {
                    return detail.ContainsFlag(flag);
                }
            }

            return false;
        }

        public IncrementalWordList CreateIncremented() =>
            new IncrementalWordList(Words, WNum + 1);
    }
}
