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

        private static readonly ConcurrentDictionary<Type, object> _fieldMappers = new();

        private static readonly ConcurrentDictionary<Type, object> _recordMappers = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление маппера в кэш.
        /// </summary>
        public static void Add<T>
            (
                FieldMapper<T> mapper
            )
        {
            _fieldMappers[typeof(T)] = mapper;
        }

        /// <summary>
        /// Добавление маппера в кэш.
        /// </summary>
        public static void Add<T>
            (
                RecordMapper<T> mapper
            )
        {
            _recordMappers[typeof(T)] = mapper;
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
        public static FieldMapper<T> GetFieldMapper<T>()
        {
            var result = _fieldMappers.GetOrAdd
                (
                    typeof(T),
                    type => FieldMapper<T>.Create()
                );

            return (FieldMapper<T>) result;
        } // method GetFieldMapper

        /// <summary>
        /// Получение маппера для указанного типа.
        /// </summary>
        public static RecordMapper<T> GetRecordMapper<T>()
        {
            var result = _recordMappers.GetOrAdd
                (
                    typeof(T),
                    type => RecordMapper<T>.Create()
                );

            return (RecordMapper<T>) result;
        } // method GetRecordMapper

        #endregion

    } // class MapperCache

} // namespace ManagedIrbisMapping
