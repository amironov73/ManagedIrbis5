// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DictionaryUtility.cs -- вспомогательные методы для работы со словарями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// <see cref="Dictionary{Key,Value}" /> manipulation
    /// helper methods.
    /// </summary>
    public static class DictionaryUtility
    {
        #region Public methods

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeWithConflicts<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
            where TKey: notnull
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Magna.Error
                        (
                            nameof(DictionaryUtility) + "::" + nameof(MergeWithConflicts)
                            + ": "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException(nameof(dictionaries));
                }
            }

            Dictionary<TKey, TValue> result = new ();
            for (var i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (var pair in dic)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeFirstValues<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
            where TKey: notnull
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Magna.Error
                        (
                            nameof(DictionaryUtility) + "::" + nameof(MergeFirstValues)
                            + ": "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException(nameof(dictionaries));
                }
            }

            Dictionary<TKey, TValue> result = new ();
            for (var i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (var pair in dic)
                {
                    if (!result.ContainsKey(pair.Key))
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Merges the specified dictionaries.
        /// </summary>
        /// <param name="dictionaries">Dictionaries to merge.</param>
        /// <returns>Merged dictionary.</returns>
        /// <exception cref="ArgumentNullException">
        /// One or more dictionaries is <c>null</c>.
        /// </exception>
        public static Dictionary<TKey, TValue> MergeLastValues<TKey, TValue>
            (
                params Dictionary<TKey, TValue>[] dictionaries
            )
            where TKey: notnull
        {
            foreach (Dictionary<TKey, TValue> dictionary in dictionaries)
            {
                if (ReferenceEquals(dictionary, null))
                {
                    Magna.Error
                        (
                            nameof(DictionaryUtility) + "::" + nameof(MergeLastValues)
                            + ": "
                            + "dictionary is null"
                        );

                    throw new ArgumentNullException(nameof(dictionaries));
                }
            }

            Dictionary<TKey, TValue> result = new ();
            for (var i = 0; i < dictionaries.Length; i++)
            {
                Dictionary<TKey, TValue> dic = dictionaries[i];
                foreach (var pair in dic)
                {
                    result[pair.Key] = pair.Value;
                }
            }
            return result;
        }

        #endregion
    }
}
