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

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Абстрактный маппер для поля записи.
    /// </summary>
    public sealed class FieldMapper<T>
    {
        #region Private members

        private Action<Field, T>? _fromField;

        private Action<Field, T>? _toField;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание маппера для указанного типа.
        /// </summary>
        public static FieldMapper<T> Create()
        {
            var result = new FieldMapper<T>
            {
                _fromField = MappingUtility.CreateForwardFieldMapper<T>()
                    .Compile(),

                _toField = MappingUtility.CreateBackwardFieldMapper<T>()
                    .Compile()
            };

            return result;
        } // method Create

        /// <summary>
        /// Переносит данные из поля в объект.
        /// </summary>
        public void FromField
            (
                Field field,
                T target
            )
        {
            _fromField!(field, target);
        }

        /// <summary>
        /// Переносит данные из объекта в поле.
        /// </summary>
        public void ToField
            (
                Field field,
                T source
            )
        {
            _toField!(field, source);
        }

        #endregion

    } // class FieldMapper

} // namespace ManagedIrbis.Mapping
