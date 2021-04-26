// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CounterDatabase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// База данных глобальных счётчиков.
    /// </summary>
    public sealed class CounterDatabase
    {
        #region Constants

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public const string DefaultName = "COUNT";

        /// <summary>
        /// Префикс для поиска по индексу счётчика.
        /// </summary>
        public const string IndexPrefix = "I=";

        /// <summary>
        /// Префикс для поиска по шаблону счётчика.
        /// </summary>
        public const string TemplatePrefix = "S=";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CounterDatabase
            (
                ISyncProvider provider
            )
            : this(provider, DefaultName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CounterDatabase
            (
                ISyncProvider provider,
                string name
            )
        {
            Provider = provider;
            Name = name;
        }

        #endregion

        #region Public methods

        /// <summary>
        ///
        /// </summary>
        public GlobalCounter CreateCounter
            (
                string index,
                string template
            )
        {
            var saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                var expression = string.Format
                    (
                        "\"{0}{1}\"",
                        IndexPrefix,
                        index
                    );
                var found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    throw new IrbisException();
                }

                var result = new GlobalCounter
                {
                    Index = index,
                    Template = template,
                    NumericValue = 0
                };
                var record = result.ToRecord();
                record.Database = Name;
                result.Record = record;
                Provider.WriteRecord(record);

                return result;
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        /// <summary>
        /// Get the <see cref="GlobalCounter"/> by its index.
        /// </summary>
        public GlobalCounter? GetCounter
            (
                string index
            )
        {
            var saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                var expression = string.Format
                    (
                        "\"{0}{1}\"",
                        IndexPrefix,
                        index
                    );
                var found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return null;
                }

                var record = Provider.ReadRecord(found[0]);
                if (ReferenceEquals(record, null))
                {
                    return null;
                }

                var result = GlobalCounter.Parse(record);

                return result;
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        /// <summary>
        /// Update the <see cref="GlobalCounter"/>.
        /// </summary>
        public void UpdateCounter
            (
                GlobalCounter counter
            )
        {
            var index = counter.Index;
            if (string.IsNullOrEmpty(index))
            {
                throw new IrbisException();
            }

            counter.Verify(true);

            var saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                var record = counter.Record;
                if (ReferenceEquals(record, null))
                {
                    var expression = $"\"{IndexPrefix}{index}\"";
                    var found = Provider.Search(expression);
                    if (found.Length == 0)
                    {
                        record = counter.ToRecord();
                    }
                    else
                    {
                        record = Provider.ReadRecord(found[0]);
                        if (ReferenceEquals(record, null))
                        {
                            throw new IrbisException();
                        }
                    }

                    record.Database = Name;
                    counter.Record = record;
                }

                Provider.WriteRecord(record);
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
