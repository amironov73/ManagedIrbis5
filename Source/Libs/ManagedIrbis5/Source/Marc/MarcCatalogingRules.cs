// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MarcCatalogingRules.cs -- код правил описания записи в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Код правил описания записи в формате ISO 2709.
    /// </summary>
    public enum MarcCatalogingRules
    {
        /// <summary>
        /// Не соответствуте ISBD, AACR2
        /// </summary>
        NotConforming = ' ',

        /// <summary>
        /// Соответствует AACR2.
        /// </summary>
        AACR2 = 'a',

        /// <summary>
        /// Соответствует ISBD.
        /// </summary>
        ISBD = 'i'
    }
}
