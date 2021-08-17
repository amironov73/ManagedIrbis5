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
// ReSharper disable UnusedMemberInSuper.Global

/* AsyncProviderUtility.cs -- методы расширения для IAsyncProvider
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AM.Linq;

using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Providers
{
    /// <summary>
    /// Методы расширения для <see cref="IAsyncProvider"/>.
    /// </summary>
    public static class AsyncProviderUtility
    {
        #region Public methods

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        public static async Task<bool> ActualizeDatabaseAsync (this IAsyncProvider connection, string? database = default) =>
            await connection.ActualizeRecordAsync ( new() { Database = database, Mfn = 0 } );

        /// <summary>
        /// Удаление файла на сервере.
        /// </summary>
        public static async Task DeleteServerFileAsync (this IAsyncProvider connection, string fileName) =>
            await connection.FormatRecordAsync ($"&if('+9K{fileName}')", 1);

        /// <summary>
        /// Удаление записи с указанным MFN.
        /// </summary>
        public static async Task<bool> DeleteRecordAsync
            (
                this IAsyncProvider connection,
                int mfn
            )
        {
            var record = await connection.ReadRecordAsync (mfn);
            if (record is null)
            {
                return false;
            }

            if (record.Deleted)
            {
                return true;
            }

            record.Status |= RecordStatus.LogicallyDeleted;

            return await connection.WriteRecordAsync (record, dontParse: true);

        } // method DeleteRecordAsync

        /// <summary>
        /// Форматирование записи по ее MFN.
        /// </summary>
        public static async Task<string?> FormatRecordAsync
            (
                this IAsyncProvider connection,
                string format,
                int mfn
            )
        {
            var parameters = new FormatRecordParameters
            {
                Database = connection.EnsureDatabase(),
                Format = format,
                Mfn = mfn
            };

            return await connection.FormatRecordsAsync(parameters)
                ? parameters.Result.AsSingle()
                : null;

        } // method FormatRecordAsync

        /// <summary>
        /// Форматирование записи в клиентском представлении.
        /// </summary>
        public static async Task<string?> FormatRecordAsync
            (
                this IAsyncProvider connection,
                string format,
                Record record
            )
        {
            var parameters = new FormatRecordParameters
            {
                Database = connection.Database,
                Format = format,
                Record = record
            };

            return await connection.FormatRecordsAsync(parameters)
                ? parameters.Result.AsSingle()
                : null;

        } // method FormatRecordAsync

        /// <summary>
        /// Форматирование записей по их MFN.
        /// </summary>
        public static async Task<string[]?> FormatRecordsAsync
            (
                this IAsyncProvider conneciton,
                int[] mfns,
                string format
            )
        {
            var parameters = new FormatRecordParameters
            {
                Database = conneciton.Database,
                Mfns = mfns,
                Format = format
            };

            return await conneciton.FormatRecordsAsync(parameters)
                ? parameters.Result.AsArray()
                : null;

        } // method FormatRecordsAsync

        /// <summary>
        /// Получение списка баз данных.
        /// </summary>
        public static async Task<DatabaseInfo[]> ListDatabasesAsync
            (
                this IAsyncProvider connection,
                string listFile = "dbnam3.mnu"
            )
        {
            var specification = new FileSpecification
            {
                Path = IrbisPath.Data,
                FileName = listFile
            };
            var menu = await connection.RequireMenuFileAsync(specification);

            return DatabaseInfo.ParseMenu(menu);

        } // method ListDatabasesAsync

        /// <summary>
        /// Блокирование указанной записи.
        /// </summary>
        public static async Task<bool> LockRecordAsync
            (
                this IAsyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Mfn = mfn,
                Lock = true
            };

            return await connection.ReadRecordAsync<NullRecord>(parameters) is not null;

        } // method LockRecordAsync

        /// <summary>
        /// Чтение FST-файла как текстового.
        /// </summary>
        public static async Task<FstFile?> ReadFstFileAsync
            (
                this IAsyncProvider connection,
                FileSpecification specification
            )
        {
            var content = await connection.ReadTextFileAsync(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);

            return FstFile.ParseStream(reader);

        } // method ReadFstFileAsync

        /// <summary>
        /// Чтение меню как текстового файла.
        /// </summary>
        public static async Task<MenuFile?> ReadMenuFileAsync
            (
                this IAsyncProvider provider,
                FileSpecification specification
            )
        {
            var content = await provider.ReadTextFileAsync(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);
            var result = MenuFile.ParseStream(reader);
            result.FileName = specification.FileName;

            return result;

        } // method ReadMenuFileAsync

        /// <summary>
        /// Чтение параметрического файла как текстового.
        /// </summary>
        public static async Task<ParFile?> ReadParFileAsync
            (
                this IAsyncProvider provider,
                FileSpecification specification
            )
        {
            var text = await provider.ReadTextFileAsync(specification);
            if (text is null)
            {
                return default;
            }

            using var reader = new StringReader(text);

            return ParFile.ParseText(reader);

        } // method ReadParFileAsync

        /// <summary>
        /// Чтение "сырой" записи с сервера.
        /// </summary>
        public static async Task<RawRecord?> ReadRawRecordAsync
            (
                this IAsyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return await connection.ReadRecordAsync<RawRecord>(parameters);

        } // method ReadRawRecordAsync

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static async Task<Record?> ReadRecordAsync (this IAsyncProvider connection, ReadRecordParameters parameters) =>
            await connection.ReadRecordAsync<Record>(parameters);

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static async Task<Record?> ReadRecordAsync
            (
                this IAsyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return await connection.ReadRecordAsync(parameters);

        } // method ReadRecordAsync

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static async Task<T?> ReadRecordAsync<T>
            (
                this IAsyncProvider connection,
                int mfn
            )
            where T: class, IRecord, new()
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return await connection.ReadRecordAsync<T>(parameters);

        } // method ReadRecordAsync

        /// <summary>
        /// Чтение указанных записей.
        /// </summary>
        public static async Task<Record[]?> ReadRecordsAsync
            (
                this IAsyncProvider connection,
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
                        .FromItem(await connection.ReadRecordAsync(mfns[0]))
                        .NonNullItems()
                        .ToArray();
            }

            var parameters = new FormatRecordParameters
            {
                Database = database,
                Mfns = mfns,
                Format = IrbisFormat.All
            };
            if (!await connection.FormatRecordsAsync(parameters))
            {
                return null;
            }

            var lines = parameters.Result.AsArray();
            if (lines.Length == 0)
            {
                return null;
            }

            var result = new List<Record>(lines.Length);
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var converted = IrbisText.SplitIrbisToLines(line);
                    if (converted.Length > 3)
                    {
                        var record = new Record();
                        record.Decode(converted[1..]);
                        result.Add(record);
                    }
                }
            }

            return result.ToArray();

        } // method ReadRecordsAsync

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public static async Task<T> RequireRecordAsync<T> ( this IAsyncProvider connection, int mfn )
            where T: class, IRecord, new()
            => await connection.ReadRecordAsync<T>(mfn)
               ?? throw new IrbisException($"Record not found: MFN={mfn}");

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public static async Task<Record> RequireRecordAsync ( this IAsyncProvider connection, string expression ) =>
            await connection.SearchReadOneRecordAsync(expression)
            ?? throw new IrbisException($"Record not found: expression={expression}");

        /// <summary>
        /// Чтение с сервера текстового файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static async Task<string> RequireTextFileAsync ( this IAsyncProvider connection, FileSpecification specification ) =>
            await connection.ReadTextFileAsync(specification)
            ?? throw new IrbisException($"File not found: {specification}");

        /// <summary>
        /// Чтение с сервера FST-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static async Task<FstFile> RequireFstFileAsync (this IAsyncProvider connection, FileSpecification specification) =>
            await connection.ReadFstFileAsync(specification)
            ?? throw new IrbisException($"FST not found: {specification}");

        /// <summary>
        /// Чтение с сервера файла меню, которое обязательно должно быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static async Task<MenuFile> RequireMenuFileAsync (this IAsyncProvider connection, FileSpecification specification) =>
            await connection.ReadMenuFileAsync(specification)
            ?? throw new IrbisException($"Menu not found: {specification}");

        /// <summary>
        /// Чтение с сервера PAR-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public static async Task<ParFile> RequireParFileAsync (this IAsyncProvider connection, FileSpecification specification) =>
            await connection.ReadParFileAsync(specification)
            ?? throw new IrbisException($"PAR not found: {specification}");

        /// <summary>
        /// Поиск с последующим чтением одной записи.
        /// </summary>
        public static async Task<Record?> SearchReadOneRecordAsync
            (
                this IAsyncProvider connection,
                string expression
            )
        {
            var parameters = new SearchParameters
            {
                Expression = expression,
                NumberOfRecords = 1
            };
            var found = await connection.SearchAsync(parameters);

            return found is { Length: 1 }
                ? await connection.ReadRecordAsync(found[0].Mfn)
                : default;

        } // method SearchReadOneRecordAsyn

        /// <summary>
        /// Сохранение/обновление записи в базе данных.
        /// </summary>
        public static async Task<bool> WriteRecordAsync
            (
                this IAsyncProvider connection,
                IRecord record,
                bool actualize = true,
                bool lockRecord = false,
                bool dontParse = false
            )
        {
            var parameters = new WriteRecordParameters
            {
                Record = record,
                Actualize = actualize,
                Lock = lockRecord,
                DontParse = dontParse
            };

            return await connection.WriteRecordAsync(parameters);

        } // method WriteRecordAsync

        #endregion

    } // class AsyncProviderUtility

} // namespace ManagedIrbis.Providers
