// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldMapper.cs -- абстрактный маппер для поля записи
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Абстрактный маппер для поля записи.
    /// </summary>
    public abstract class FieldMapper
    {
        #region Public methods

        /// <summary>
        /// Переносит данные из поля в объект.
        /// </summary>
        public abstract void FromField<T>
            (
                Field field,
                T target
            );

        /// <summary>
        /// Переносит данные из объекта в поле.
        /// </summary>
        public abstract void ToField<T>
            (
                Field field,
                T source
            );

        #endregion

    } // class FieldMapper

} // namespace ManagedIrbis.Mapping
