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
using AM.Collections;
using AM.IO;
using AM.Linq;

using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Providers;

/// <summary>
/// Вспомогательные методы для синхронного провайдера.
/// </summary>
public static class SyncProviderUtility
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
    public static bool ActualizeDatabase (this ISyncProvider connection, string? database = default) =>
        connection.ActualizeRecord (new () { Database = database, Mfn = 0 });

    /// <summary>
    /// Удаление файла на сервере.
    /// </summary>
    public static void DeleteServerFile (this ISyncProvider connection, string fileName) =>
        connection.FormatRecord ($"&if('+9K{fileName}')", 1);

    /// <summary>
    /// Удаление записи с указанным MFN.
    /// </summary>
    public static bool DeleteRecord
        (
            this ISyncProvider connection,
            int mfn
        )
    {
        var record = connection.ReadRecord (mfn);
        if (record is null)
        {
            return false;
        }

        if (record.Deleted)
        {
            return true;
        }

        record.Status |= RecordStatus.LogicallyDeleted;

        return connection.WriteRecord (record, dontParse: true);
    } // method DeleteRecord

    /// <summary>
    /// Подстановка имени текущей базы данных, если она не задана явно.
    /// </summary>
    public static string EnsureDatabase
        (
            this IIrbisProvider connection,
            string? database = null
        )
        =>
            string.IsNullOrEmpty (database)
                ? string.IsNullOrEmpty (connection.Database)
                    ? throw new ArgumentException (nameof (connection.Database))
                    : connection.Database
                : database;

    /// <summary>
    /// Форматирование записи по ее MFN.
    /// </summary>
    public static string? FormatRecord
        (
            this ISyncProvider connection,
            string format,
            int mfn
        )
    {
        var parameters = new FormatRecordParameters
        {
            Database = connection.Database,
            Format = format,
            Mfn = mfn
        };

        return connection.FormatRecords (parameters)
            ? parameters.Result.AsSingle()
            : null;
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
        var parameters = new FormatRecordParameters
        {
            Database = connection.Database,
            Format = format,
            Record = record
        };

        return connection.FormatRecords (parameters)
            ? parameters.Result.AsSingle()
            : null;
    } // method FormatRecord

    /// <summary>
    /// Форматирование записей по их MFN.
    /// </summary>
    public static string[]? FormatRecords
        (
            this ISyncProvider conneciton,
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

        return conneciton.FormatRecords (parameters)
            ? parameters.Result.AsArray()
            : null;
    } // method FormatRecords

    /// <summary>
    /// Получение списка баз данных.
    /// </summary>
    public static DatabaseInfo[] ListDatabases
        (
            this ISyncProvider connection,
            string listFile = "dbnam3.mnu"
        )
    {
        var specification = new FileSpecification
        {
            Path = IrbisPath.Data,
            FileName = listFile
        };
        var menu = connection.RequireMenuFile (specification);

        return DatabaseInfo.ParseMenu (menu);
    } // method ListDatabases

    /// <summary>
    /// Блокирование указанной записи.
    /// </summary>
    public static bool LockRecord
        (
            this ISyncProvider connection,
            int mfn
        )
    {
        var parameters = new ReadRecordParameters
        {
            Mfn = mfn,
            Lock = true
        };

        return connection.ReadRecord<NullRecord> (parameters) is not null;
    } // method LockRecord

    /// <summary>
    /// Чтение всех терминов с указанным префиксом.
    /// </summary>
    public static Term[] ReadAllTerms
        (
            this ISyncProvider connection,
            string prefix
        )
    {
        if (!connection.CheckProviderState())
        {
            return Array.Empty<Term>();
        }

        prefix = prefix.ToUpperInvariant();
        var result = new List<Term>();
        var startTerm = prefix;
        var flag = true;
        while (flag)
        {
            var terms = connection.ReadTerms (startTerm, 1024);
            if (terms.IsNullOrEmpty())
            {
                break;
            }

            var startIndex = 0;
            if (result.Count != 0)
            {
                var lastTerm = result[^1];
                var firstTerm = terms![0];
                if (firstTerm.Text == lastTerm.Text)
                {
                    startIndex = 1;
                }
            }

            for (var i = startIndex; i < terms!.Length; i++)
            {
                var term = terms[i];
                var text = term.Text;
                if (string.IsNullOrEmpty (text))
                {
                    break;
                }

                if (!text.StartsWith (prefix))
                {
                    flag = false;
                    break;
                }

                result.Add (term);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение FST-файла как текстового.
    /// </summary>
    public static FstFile? ReadFstFile
        (
            this ISyncProvider connection,
            FileSpecification specification
        )
    {
        var content = connection.ReadTextFile (specification);
        if (content is null)
        {
            return default;
        }

        using var reader = new StringReader (content);

        return FstFile.ParseStream (reader);
    } // method ReadFstFile

    /// <summary>
    /// Чтение INI-файла как текстового.
    /// </summary>
    public static IniFile? ReadIniFile
        (
            this ISyncProvider provider,
            FileSpecification specification
        )
    {
        var content = provider.ReadTextFile (specification);
        if (content is null)
        {
            return default;
        }

        using var reader = new StringReader (content);
        var result = new IniFile { FileName = specification.FileName };
        result.Read (reader);

        return result;
    } // method ReadIniFile

    /// <summary>
    /// Чтение меню как текстового файла.
    /// </summary>
    public static MenuFile? ReadMenuFile
        (
            this ISyncProvider provider,
            FileSpecification specification
        )
    {
        var content = provider.ReadTextFile (specification);
        if (content is null)
        {
            return default;
        }

        using var reader = new StringReader (content);
        var result = MenuFile.ParseStream (reader);
        result.FileName = specification.FileName;

        return result;
    }

    /// <summary>
    /// Чтение параметрического файла как текстового.
    /// </summary>
    public static ParFile? ReadParFile
        (
            this ISyncProvider provider,
            FileSpecification specification
        )
    {
        var text = provider.ReadTextFile (specification);
        if (text is null)
        {
            return default;
        }

        using var reader = new StringReader (text);

        return ParFile.ParseText (reader);
    }

    /// <summary>
    /// Чтение "сырой" записи с сервера.
    /// </summary>
    public static RawRecord? ReadRawRecord
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

        return connection.ReadRecord<RawRecord> (parameters);
    }

    /// <summary>
    /// Чтение записи с сервера.
    /// </summary>
    public static Record? ReadRecord
        (
            this ISyncProvider connection,
            ReadRecordParameters parameters
        )
    {
        return connection.ReadRecord<Record> (parameters);
    }

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

        return connection.ReadRecord (parameters);
    }

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
                    .FromItem (connection.ReadRecord (mfns[0]))
                    .NonNullItems()
                    .ToArray();
        }

        var parameters = new FormatRecordParameters
        {
            Database = database,
            Mfns = mfns,
            Format = IrbisFormat.All
        };
        if (!connection.FormatRecords (parameters))
        {
            return null;
        }

        var lines = parameters.Result.AsArray();
        if (lines.Length == 0)
        {
            return null;
        }

        var result = new List<Record> (lines.Length);
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty (line))
            {
                var converted = IrbisText.SplitIrbisToLines (line);
                if (converted.Length > 3)
                {
                    var record = new Record();
                    record.Decode (converted[1..]);
                    result.Add (record);
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение терминов словаря.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="startTerm">Параметры терминов.</param>
    /// <param name="numberOfTerms">Максимальное число терминов.</param>
    /// <returns>Массив прочитанных терминов.</returns>
    public static Term[]? ReadTerms
        (
            this ISyncProvider connection,
            string startTerm,
            int numberOfTerms
        )
    {
        var parameters = new TermParameters
        {
            Database = connection.Database,
            StartTerm = startTerm,
            NumberOfTerms = numberOfTerms
        };

        return connection.ReadTerms (parameters);
    }

    /// <summary>
    /// Чтение с сервера записи, которая обязательно должна быть.
    /// </summary>
    /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
    public static Record RequireRecord (this ISyncProvider connection, int mfn) =>
        connection.ReadRecord (mfn)
        ?? throw new IrbisException ($"Record not found: MFN={mfn}");

    /// <summary>
    /// Чтение с сервера записи, которая обязательно должна быть.
    /// </summary>
    /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
    public static Record RequireRecord (this ISyncProvider connection, string expression) =>
        connection.SearchReadOneRecord (expression)
        ?? throw new IrbisException ($"Record not found: expression={expression}");

    /// <summary>
    /// Чтение с сервера текстового файла, который обязательно должен быть.
    /// </summary>
    /// <exception cref="FileNotFoundException">Файл отсутствует или другая ошибка при чтении.</exception>
    public static string RequireTextFile (this ISyncProvider connection, FileSpecification specification) =>
        connection.ReadTextFile (specification)
        ?? throw new IrbisException ($"File not found: {specification}");

    /// <summary>
    /// Чтение с сервера FST-файла, который обязательно должен быть.
    /// </summary>
    /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
    public static FstFile RequireFstFile (this ISyncProvider connection, FileSpecification specification) =>
        connection.ReadFstFile (specification)
        ?? throw new IrbisException ($"FST not found: {specification}");

    /// <summary>
    /// Чтение с сервера INI-файла, который обязательно должен быть.
    /// </summary>
    /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
    public static IniFile RequireIniFile (this ISyncProvider connection, FileSpecification specification) =>
        connection.ReadIniFile (specification)
        ?? throw new IrbisException ($"INI not found: {specification}");

    /// <summary>
    /// Чтение с сервера файла меню, которое обязательно должно быть.
    /// </summary>
    /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
    public static MenuFile RequireMenuFile (this ISyncProvider connection, FileSpecification specification) =>
        connection.ReadMenuFile (specification)
        ?? throw new IrbisException ($"Menu not found: {specification}");

    /// <summary>
    /// Чтение с сервера PAR-файла, который обязательно должен быть.
    /// </summary>
    /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
    public static ParFile RequireParFile
        (
            this ISyncProvider connection,
            FileSpecification specification
        )
    {
        return connection.ReadParFile (specification)
               ?? throw new IrbisException ($"PAR not found: {specification}");
    }

    /// <summary>
    /// Упрощенный поиск.
    /// </summary>
    public static int[] Search
        (
            this ISyncProvider connection,
            string expression
        )
    {
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression
        };
        var lines = connection.Search (parameters);
        var result = FoundItem.ToMfn (lines);

        return result;
    }

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
            Database = connection.Database.ThrowIfNull(),
            NumberOfRecords = 1
        };
        var found = connection.Search (parameters);

        return found is { Length: 1 }
            ? connection.ReadRecord (found[0].Mfn)
            : default;
    }

    /// <summary>
    /// Сохранение/обновление записи в базе данных.
    /// </summary>
    public static bool WriteRecord
        (
            this ISyncProvider connection,
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

        return connection.WriteRecord (parameters);
    }

    #endregion
}
