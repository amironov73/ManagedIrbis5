// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MarcBibliographicalLevel.cs -- уровень кодирования библиографического описания в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Уровень кодирования библиографического описания в формате ISO 2709.
    /// </summary>
    public enum MarcBibliographicalLevel
    {
        /// <summary>
        /// Полный уровень описания.
        /// </summary>
        Full = ' ',

        /// <summary>
        /// Полный уровень, но запись не была проверена.
        /// </summary>
        FullNotChecked = '1',

        /// <summary>
        /// Не окончательно созданная запись.
        /// </summary>
        NotComplete = '5',

        /// <summary>
        /// Минимальный уровень.
        /// </summary>
        Minimal = '7',

        /// <summary>
        /// Запись сделана по тематическому плану издательства.
        /// </summary>
        Publisher = '8',

        /// <summary>
        /// Уровень неизвестен.
        /// </summary>
        Unknown = 'u',

        /// <summary>
        /// Невозможно установить уровень записи.
        /// </summary>
        NotAvailable = 'z'
    }
}
