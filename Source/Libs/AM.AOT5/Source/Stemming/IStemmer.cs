// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IStemmer.cs -- общий интерфейс стеммера
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.AOT.Stemming
{
    /// <summary>
    /// Общий интерфейс стеммера.
    /// </summary>
    public interface IStemmer
    {
        /// <summary>
        /// Извлечение корня из указанного слова.
        /// </summary>
        string Stem (string word);

    } // interface IStemmer

} // namespace AM.AOT.Stemming
