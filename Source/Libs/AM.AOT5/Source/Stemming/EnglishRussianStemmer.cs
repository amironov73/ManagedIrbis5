// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EnglishRussianStemmer.cs -- стеммер для английского и русского языков
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.AOT.Stemming;

/// <summary>
/// Стеммер для английского и русского языков.
/// </summary>
public sealed class EnglishRussianStemmer
    : MultiStemmer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public EnglishRussianStemmer()
        : base (new EnglishStemmer(), new RussianStemmer())
    {
        // пустое тело класса
    }

    #endregion
}
