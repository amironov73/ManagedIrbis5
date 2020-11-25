// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MarcBibliographicalIndex.cs -- код библиографического указателя в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Код библиографического указателя в формате ISO 2709.
    /// </summary>
    public enum MarcBibliographicalIndex
    {
        /// <summary>
        /// Часть монографии.
        /// </summary>
        PartOfMonograph = 'a',

        /// <summary>
        /// Часть сериального издания.
        /// </summary>
        PartOfSerialPrinting = 'b',

        /// <summary>
        /// Коллекция.
        /// </summary>
        Collection = 'c',

        /// <summary>
        /// Часть коллекции.
        /// </summary>
        PartOfCollection = 'd',

        /// <summary>
        /// Монография (книга, рукопись, картина и т.д.).
        /// </summary>
        Monograph = 'm',

        /// <summary>
        /// Сериальное издание, периодика.
        /// </summary>
        SerialPrinting = 's'
    }
}
