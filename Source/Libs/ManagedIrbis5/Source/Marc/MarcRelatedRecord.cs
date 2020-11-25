// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MarcRelatedRecord.cs -- код необходимости связанной записи в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Код необходимости связанной записи в формате ISO 2709.
    /// </summary>
    public enum MarcRelatedRecord
    {
        /// <summary>
        /// Связанная запись не требуется.
        /// </summary>
        NotRequired = ' ',

        /// <summary>
        /// Требуется связанная запись.
        /// </summary>
        Required = 'r'
    }
}
