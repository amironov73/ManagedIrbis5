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

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Абстрактный маппер для библиографической записи.
    /// </summary>
    public sealed class RecordMapper
    {
        #region Private members

        private Action<Record, object>? _fromRecord;

        private Action<Record, object>? _toRecord;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание маппера для указанного типа.
        /// </summary>
        public static RecordMapper Create
            (
                Type type
            )
        {
            var result = new RecordMapper
            {
                _fromRecord = MappingUtility.CreateForwardRecordMapper(type)
                    .Compile(),

                _toRecord = MappingUtility.CreateBackwardRecordMapper(type)
                    .Compile()
            };

            return result;
        } // method Create

        /// <summary>
        /// Переносит данные из записи в объект.
        /// </summary>
        public void FromRecord
            (
                Record record,
                object target
            )
        {
            _fromRecord!(record, target);
        }

        /// <summary>
        /// Переносит данные из объекта в запись.
        /// </summary>
        public void ToRecord
            (
                Record record,
                object source
            )
        {
            _toRecord!(record, source);
        }

        #endregion

    } // class RecordMapper

} // namespace ManagedIrbis.Mapping
