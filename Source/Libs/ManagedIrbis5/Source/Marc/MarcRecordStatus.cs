// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* RecordStatus.cs -- код статуса записи в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Код статуса записи в формате ISO 2709.
    /// </summary>
    public enum MarcRecordStatus
    {
        /// <summary>
        /// При изменении записи увеличивается ее уровень.
        /// </summary>
        LevelUp = 'a',

        /// <summary>
        /// При изменении записи уровень остался прежним.
        /// </summary>
        Changed = 'c',

        /// <summary>
        /// Запись удалена, но информация о ней сохраняется.
        /// </summary>
        Deleted = 'd',

        /// <summary>
        /// Новая запись.
        /// </summary>
        New = 'n',

        /// <summary>
        /// Запись изменилась после того, как была введена с плана издательства.
        /// </summary>
        Publisher = 'p'

    } // enum MarcRecordStatus

} // namespace ManagedIrbis.Marc
