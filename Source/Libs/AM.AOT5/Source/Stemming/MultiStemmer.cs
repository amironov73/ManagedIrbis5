// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MultiStemmer.cs -- комбинированный стеммер
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AM.AOT.Stemming;

/// <summary>
/// Комбинированный стеммер.
/// </summary>
public sealed class MultiStemmer
    : IStemmer
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MultiStemmer
        (
            params IStemmer[] stemmers
        )
    {
        _stemmers = stemmers;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MultiStemmer
        (
            IReadOnlyList<IStemmer> stemmers
        )
    {
        _stemmers = stemmers;
    }

    #endregion

    #region Private members

    private readonly IReadOnlyList<IStemmer> _stemmers;

    #endregion

    #region IStemmer members

    /// <inheritdoc cref="IStemmer.Stem"/>
    public string Stem
        (
            string word
        )
    {
        if (string.IsNullOrEmpty (word))
        {
            return word;
        }

        foreach (var stemmer in _stemmers)
        {
            try
            {
                var candidate = stemmer.Stem (word);
                if (string.Compare (word, candidate, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return candidate;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception);
            }
        }

        return word;
    }

    #endregion
}
