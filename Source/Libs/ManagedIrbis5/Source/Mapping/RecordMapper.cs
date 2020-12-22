// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordMapper.cs -- абстрактный маппер для библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Абстрактный маппер для библиографической записи.
    /// </summary>
    public abstract class RecordMapper
    {
        #region Public methods

        /// <summary>
        /// Переносит данные из записи в объект.
        /// </summary>
        public abstract void FromField<T>
            (
                Record record,
                T target
            );

        /// <summary>
        /// Переносит данные из объекта в запись.
        /// </summary>
        public abstract void ToField<T>
            (
                Record record,
                T source
            );

        #endregion

    } // class RecordMapper

} // namespace ManagedIrbis.Mapping
