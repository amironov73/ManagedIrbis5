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
    public sealed class FieldMapper
    {
        #region Private members

        private Action<Field, object>? _fromField;

        private Action<Field, object>? _toField;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание маппера для указанного типа.
        /// </summary>
        public static FieldMapper Create
            (
                Type type
            )
        {
            var result = new FieldMapper
            {
                _fromField = MappingUtility.CreateForwardFieldMapper(type)
                    .Compile(),

                _toField = MappingUtility.CreateBackwardFieldMapper(type)
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
                object target
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
                object source
            )
        {
            _toField!(field, source);
        }

        #endregion

    } // class FieldMapper

} // namespace ManagedIrbis.Mapping
