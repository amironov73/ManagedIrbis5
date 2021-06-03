// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* SyncProviderUtility.cs -- вспомогательные методы для синхронного провайдера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AM;
using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Providers
{
    /// <summary>
    /// Вспомогательные методы для синхронного провайдера.
    /// </summary>
    public static class SyncProviderUtility
    {
        #region Public methods

        /// <summary>
        /// Форматирование записи по ее MFN.
        /// </summary>
        public static string? FormatRecord
            (
                this ISyncProvider conneciton,
                string format,
                int mfn
            )
        {
            throw new NotImplementedException();

        } // method FormatRecord

        /// <summary>
        /// Форматирование записи в клиентском представлении.
        /// </summary>
        public static string? FormatRecord
            (
                this ISyncProvider connection,
                string format,
                Record record
            )
        {
            throw new NotImplementedException();

        } // method FormatRecord

        /// <summary>
        /// Чтение FST-файла как текстового.
        /// </summary>
        public static FstFile? ReadFstFile
            (
                this ISyncProvider connection,
                FileSpecification specification
            )
        {
            var content = connection.ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);

            return FstFile.ParseStream(reader);

        } // method ReadFstFile

        /// <summary>
        /// Чтение меню как текстового файла.
        /// </summary>
        public static MenuFile? ReadMenuFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var content = provider.ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);

            return MenuFile.ParseStream(reader);

        } // method ReadMenuFile

        /// <summary>
        /// Чтение параметрического файла как текстового.
        /// </summary>
        public static ParFile? ReadParFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var text = provider.ReadTextFile(specification);
            if (text is null)
            {
                return default;
            }

            using var reader = new StringReader(text);

            return ParFile.ParseText(reader);

        } // method ReadParFile

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static Record? ReadRecord
            (
                this ISyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return connection.ReadRecord(parameters);

        } // method ReadRecord

        /// <summary>
        /// Чтение указанных записей.
        /// </summary>
        public static Record[]? ReadRecords
            (
                this ISyncProvider connection,
                string database,
                IEnumerable<int> batch
            )
        {
            if (!connection.CheckProviderState())
            {
                return null;
            }

            int[] mfns = batch is int[] array ? array : batch.ToArray();

            switch (mfns.Length)
            {
                case 0:
                    return Array.Empty<Record>();

                case 1:
                    // TODO: use database parameter

                    return Sequence
                        .FromItem(connection.ReadRecord(mfns[0]))
                        .NonNullItems()
                        .ToArray();
            }

            // TODO: implement

            return Array.Empty<Record>();

        } // method ReadRecords


        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public static Record RequireRecord ( this ISyncProvider connection, int mfn ) =>
            connection.ReadRecord(mfn)
            ?? throw new IrbisException($"Record not found: MFN={mfn}");

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public static Record RequireRecord ( this ISyncProvider connection, string expression ) =>
            connection.SearchReadOneRecord(expression)
            ?? throw new IrbisException($"Record not found: expression={expression}");

        /// <summary>
        /// Чтение с сервера текстового файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static string RequireTextFile ( this ISyncProvider connection, FileSpecification specification ) =>
            connection.ReadTextFile(specification)
            ?? throw new IrbisException($"File not found: {specification}");

        /// <summary>
        /// Чтение с сервера FST-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static FstFile RequireFstFile (this ISyncProvider connection, FileSpecification specification) =>
            connection.ReadFstFile(specification)
            ?? throw new IrbisException($"FST not found: {specification}");

        /// <summary>
        /// Чтение с сервера файла меню, которое обязательно должно быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static MenuFile RequireMenuFile (this ISyncProvider connection, FileSpecification specification) =>
            connection.ReadMenuFile(specification)
            ?? throw new IrbisException($"Menu not found: {specification}");

        /// <summary>
        /// Чтение с сервера PAR-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static ParFile RequireParFile (this ISyncProvider connection, FileSpecification specification) =>
            connection.ReadParFile(specification)
            ?? throw new IrbisException($"PAR not found: {specification}");

        /// <summary>
        /// Поиск с последующим чтением одной записи.
        /// </summary>
        public static Record? SearchReadOneRecord
            (
                this ISyncProvider connection,
                string expression
            )
        {
            var parameters = new SearchParameters
            {
                Expression = expression,
                NumberOfRecords = 1
            };
            var found = connection.Search(parameters);

            return found is { Length: 1 }
                ? connection.ReadRecord(found[0].Mfn)
                : default;

        } // method SearchReadOneRecord

        #endregion

    } // class SyncProviderUtility

} // namespace ManagedIrbis.Providers
