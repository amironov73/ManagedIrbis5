// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SimplifyConditionalTernaryExpression
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MapperCache.cs -- кэш мапперов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Кэш мапперов.
    /// </summary>
    public static class MapperCache
    {
        #region Private members

        private static readonly ConcurrentDictionary<Type, FieldMapper> _fieldMappers = new();

        private static readonly ConcurrentDictionary<Type, RecordMapper> _recordMappers = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление маппера в кэш.
        /// </summary>
        public static void Add
            (
                Type type,
                FieldMapper mapper
            )
        {
            _fieldMappers[type] = mapper;
        }

        /// <summary>
        /// Добавление маппера в кэш.
        /// </summary>
        public static void Add
            (
                Type type,
                RecordMapper mapper
            )
        {
            _recordMappers[type] = mapper;
        }

        /// <summary>
        /// Очистка кэша.
        /// </summary>
        public static void Clear()
        {
            _fieldMappers.Clear();
            _recordMappers.Clear();
        }

        /// <summary>
        /// Получение маппера для указанного типа.
        /// </summary>
        public static FieldMapper GetFieldMapper
            (
                Type type
            )
        {
            var result = _fieldMappers.GetOrAdd
                (
                    type,
                    FieldMapper.Create
                );

            return result;
        } // method GetFieldMapper

        /// <summary>
        /// Получение маппера для указанного типа.
        /// </summary>
        public static FieldMapper GetFieldMapper<T>()
            where T : class, new()
            => GetFieldMapper(typeof(T));

        /// <summary>
        /// Получение маппера для указанного типа.
        /// </summary>
        public static RecordMapper GetRecordMapper
            (
                Type type
            )
        {
            var result = _recordMappers.GetOrAdd
                (
                    type,
                    RecordMapper.Create
                );

            return result;
        } // method GetRecordMapper

        /// <summary>
        /// Получение маппера для указанного типа.
        /// </summary>
        public static RecordMapper GetRecordMapper<T>()
            where T : class, new()
            => GetRecordMapper(typeof(T));

        #endregion

    } // class MapperCache

} // namespace ManagedIrbisMapping
