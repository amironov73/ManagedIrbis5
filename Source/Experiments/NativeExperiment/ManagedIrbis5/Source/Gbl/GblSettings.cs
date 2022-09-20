// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

/* GblSettings.cs -- settings for GBL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// Настройки для глобальной корректировки.
/// </summary>
public sealed class GblSettings
    : IVerifiable,
        IHandmadeSerializable
{
    #region Constants

    /// <summary>
    /// Разделитель элементов в строке.
    /// </summary>
    private const string Delimiter = IrbisText.IrbisDelimiter;

    #endregion

    #region Properties

    /// <summary>
    /// Актуализировать записи после обработки?
    /// </summary>
    [JsonPropertyName ("actualize")]
    [DisplayName ("Актуализация")]
    [Description ("Актуализировать записи после обработки?")]
    public bool Actualize { get; set; } = true;

    /// <summary>
    /// Выполнить также 'autoin.gbl'?
    /// </summary>
    [JsonPropertyName ("autoin")]
    [DisplayName ("Выполнять autoin?")]
    [Description ("Выполнять также autoin.gbl?")]
    public bool Autoin { get; set; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [JsonPropertyName ("database")]
    [DisplayName ("База данных")]
    [Description ("Имя базы данныхх")]
    public string? Database { get; set; }

    /// <summary>
    /// Имя (серверного) файла.
    /// </summary>
    [JsonPropertyName ("fileName")]
    [DisplayName ("Имя файла")]
    [Description ("Имя файла")]
    public string? FileName { get; set; }

    /// <summary>
    /// MFN первой записи.
    /// </summary>
    [JsonPropertyName ("firstRecord")]
    [DisplayName ("Первая запись")]
    [Description ("MFN первой записи")]
    public int FirstRecord { get; set; } = 1;

    /// <summary>
    /// Включение формально-логического контроля.
    /// </summary>
    [JsonPropertyName ("formalControl")]
    [DisplayName ("ФЛК")]
    [Description ("Формально-логический контроль")]
    public bool FormalControl { get; set; }

    /// <summary>
    /// Максимальный MFN.
    /// </summary>
    /// <remarks>0 означает 'все записи в базе данных'.
    /// </remarks>
    [JsonPropertyName ("maxMfn")]
    [DisplayName ("Максимальный MFN")]
    [Description ("Максимальный MFN")]
    public int MaxMfn { get; set; }

    /// <summary>
    /// Список MFN, подлежащих обработке.
    /// </summary>
    [JsonPropertyName ("mfnList")]
    [DisplayName ("Список MFN")]
    [Description ("Список MFN, подлежащих обработке")]
    public int[]? MfnList { get; set; }

    /// <summary>
    /// Минимальный MFN.
    /// </summary>
    /// <remarks>0 означает 'все записи в базе данных'.
    /// </remarks>
    [JsonPropertyName ("minMfn")]
    [DisplayName ("Минимальный MFN")]
    [Description ("Минимальный MFN")]
    public int MinMfn { get; set; }

    /// <summary>
    /// Количество записей, подлежащих обработке.
    /// </summary>
    [JsonPropertyName ("numberOfRecords")]
    [DisplayName ("Количество записей")]
    [Description ("Количество записей, подлежащиз обработке")]
    public int NumberOfRecords { get; set; }

    /// <summary>
    /// Поисковое выражение, отбирающее записи для обработки.
    /// </summary>
    [JsonPropertyName ("searchExpression")]
    [DisplayName ("Поисковое выражение")]
    [Description ("Поисковое выражение, отбирающее записи для обработки")]
    public string? SearchExpression { get; set; }

    /// <summary>
    /// Операторы глобальной корректировки.
    /// </summary>
    [JsonPropertyName ("statements")]
    [DisplayName ("Операторы")]
    [Description ("Операторы глобальной корректировки")]
    public NonNullCollection<GblStatement> Statements { get; private set; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public GblSettings()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connection">Настроенное подключение.</param>
    public GblSettings
        (
            IIrbisProvider connection
        )
    {
        Sure.NotNull (connection);

        Database = connection.Database; // не надо EnsureDatabase
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connection">Настроенное подключение.</param>
    /// <param name="statements">Операторы ГК.</param>
    public GblSettings
        (
            IIrbisProvider connection,
            IEnumerable<GblStatement> statements
        )
        : this (connection)
    {
        Sure.NotNull ((object?) statements);

        Statements.AddRange (statements);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование клиентского запроса.
    /// </summary>
    public void Encode<T>
        (
            T query
        )
        where T : IQuery
    {
        query.Add (Actualize ? 1 : 0);
        if (!string.IsNullOrEmpty (FileName))
        {
            query.AddAnsi ("@" + FileName);
        }
        else
        {
            var builder = StringBuilderPool.Shared.Get();

            // "!" здесь означает, что передавать будем в UTF-8
            builder.Append ('!');

            // не знаю, что тут означает "0"
            builder.Append ('0');
            builder.Append (Delimiter);
            foreach (var statement in Statements)
            {
                // TODO: сделать подстановку параметров
                // $encoded .=  $settings->substituteParameters(strval($statement));
                builder.Append (statement.EncodeForProtocol());
                builder.Append (Delimiter);
            }

            builder.Append (Delimiter);
            query.AddUtf (builder.ToString());
            StringBuilderPool.Shared.Return (builder);
        }

        // отбор записей на основе поиска
        query.AddUtf (SearchExpression); // поиск по словарю
        query.Add (MinMfn); // нижняя граница MFN
        query.Add (MaxMfn); // верхняя граница MFN

        //query.AddUtf(SequentialExpression); // последовательный поиск

        // TODO поддержка режима "кроме отмеченных"
        if (MfnList is { Length: not 0 })
        {
            query.Add (MfnList.Length);
            foreach (var mfn in MfnList)
            {
                query.Add (mfn);
            }
        }
        else
        {
            var count = MaxMfn - MinMfn + 1;
            query.Add (count);
            for (var mfn = 0; mfn <= MaxMfn; mfn++)
            {
                query.Add (mfn);
            }
        }

        if (!FormalControl)
        {
            query.AddAnsi ("*");
        }

        if (!Autoin)
        {
            query.AddAnsi ("&");
        }
    }

    /// <summary>
    /// Восстановление настроек из JSON-строки.
    /// </summary>
    public static GblSettings FromJson
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var result = JsonConvert.DeserializeObject<GblSettings> (text)
            .ThrowIfNull();

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// для заданного интервала MFN.
    /// </summary>
    public static GblSettings ForInterval
        (
            ISyncProvider connection,
            int minMfn,
            int maxMfn,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NonNegative (minMfn);
        Sure.NonNegative (maxMfn);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            MinMfn = minMfn,
            MaxMfn = maxMfn
        };

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// для заданного интервала MFN.
    /// </summary>
    public static GblSettings ForInterval
        (
            ISyncProvider connection,
            string database,
            int minMfn,
            int maxMfn,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NonNegative (minMfn);
        Sure.NonNegative (maxMfn);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            Database = connection.EnsureDatabase (database),
            MinMfn = minMfn,
            MaxMfn = maxMfn
        };

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// для заданного списка MFN.
    /// </summary>
    public static GblSettings ForList
        (
            ISyncProvider connection,
            IEnumerable<int> mfnList,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) mfnList);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            MfnList = mfnList.ToArray()
        };

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// для заданного спика MFN.
    /// </summary>
    public static GblSettings ForList
        (
            ISyncProvider connection,
            string database,
            IEnumerable<int> mfnList,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) mfnList);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            Database = connection.EnsureDatabase (database),
            MfnList = mfnList.ToArray()
        };

        return result;
    }

    /// <summary>
    /// Сокздание настроек <see cref="GblSettings"/>
    /// для заданного списка MFN.
    /// </summary>
    public static GblSettings ForList
        (
            ISyncProvider connection,
            string database,
            IEnumerable<int> mfnList
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) mfnList);

        var result = new GblSettings (connection)
        {
            Database = connection.EnsureDatabase (database),
            MfnList = mfnList.ToArray()
        };

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// по заданному поисковому выражению.
    /// </summary>
    public static GblSettings ForSearchExpression
        (
            ISyncProvider connection,
            string searchExpression,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            SearchExpression = searchExpression
        };

        return result;
    }

    /// <summary>
    /// Создание настроек <see cref="GblSettings"/>
    /// по заданному поисковому выражению.
    /// </summary>
    public static GblSettings ForSearchExpression
        (
            ISyncProvider connection,
            string database,
            string searchExpression,
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) statements);

        var result = new GblSettings (connection, statements)
        {
            Database = connection.EnsureDatabase (database),
            SearchExpression = searchExpression
        };

        return result;
    }

    /// <summary>
    /// Задание (серверного) имени файла.
    /// </summary>
    public GblSettings SetFileName
        (
            string fileName
        )
    {
        FileName = fileName;

        return this;
    }

    /// <summary>
    /// Задание MFN первой записи и общего количества
    /// обрабатываемых записей.
    /// </summary>
    public GblSettings SetRange
        (
            int firstRecord,
            int numberOfRecords
        )
    {
        Sure.NonNegative (firstRecord);
        Sure.NonNegative (numberOfRecords);

        FirstRecord = firstRecord;
        NumberOfRecords = numberOfRecords;

        return this;
    }

    /// <summary>
    /// Задание поискового выражения.
    /// </summary>
    public GblSettings SetSearchExpression
        (
            string searchExpression
        )
    {
        SearchExpression = searchExpression;

        return this;
    }

    /// <summary>
    /// Представление настроке в виде JSON-строки.
    /// </summary>
    public string ToJson()
    {
        var result = JObject.FromObject (this)
            .ToString (Newtonsoft.Json.Formatting.None);

        return result;
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Actualize = reader.ReadBoolean();
        Autoin = reader.ReadBoolean();
        Database = reader.ReadNullableString();
        FileName = reader.ReadNullableString();
        FirstRecord = reader.ReadPackedInt32();
        FormalControl = reader.ReadBoolean();
        MaxMfn = reader.ReadPackedInt32();
        MfnList = reader.ReadNullableInt32Array();
        MinMfn = reader.ReadPackedInt32();
        NumberOfRecords = reader.ReadPackedInt32();
        SearchExpression = reader.ReadNullableString();
        Statements = reader.ReadNonNullCollection<GblStatement>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.Write (Actualize);
        writer.Write (Autoin);
        writer.WriteNullable (Database);
        writer.WriteNullable (FileName);
        writer.WritePackedInt32 (FirstRecord);
        writer.Write (FormalControl);
        writer.WritePackedInt32 (MaxMfn);
        writer.WriteNullableArray (MfnList);
        writer.WritePackedInt32 (MinMfn);
        writer.WritePackedInt32 (NumberOfRecords);
        writer.WriteNullable (SearchExpression);
        writer.WriteCollection (Statements);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<GblSettings> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Database)
            .Assert (Statements.Count != 0);

        foreach (var statement in Statements)
        {
            statement.Verify (throwOnError);
        }

        return verifier.Result;
    }

    #endregion
}
