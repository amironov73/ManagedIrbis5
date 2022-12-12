// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* GblBuilder.cs -- упрощенное построение заданий на глобальную корректировку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using ManagedIrbis.Providers;

#endregion

// метод можно сделать статическим
#pragma warning disable CA1822

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// <para>Инструмент для упрощённого построения заданий на
/// глобальную корректировку.</para>
/// <para>Пример построения и выполнения задания:</para>
/// <code>
/// GblResult result = new GblBuilder()
///        .Add("3079", "'1'")
///        .Delete("3011")
///        .Execute
///             (
///                 connection,
///                 "IBIS",
///                 new[] {30, 32, 34}
///             );
/// Console.WriteLine
///     (
///         "Processed {0} records",
///         result.RecordsProcessed
///     );
/// foreach (ProtocolLine line in result.Protocol)
/// {
///     Console.WriteLine(line);
/// }
/// </code>
/// </summary>
public sealed class GblBuilder
{
    #region Private members

    private const string Filler = "XXXXXXXXXXXXXXXXX";
    private const string All = "*";

    private readonly List<GblStatement> _statements = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление произвольной команды.
    /// </summary>
    public GblBuilder AddStatement
        (
            string code,
            string? parameter1,
            string? parameter2,
            string? format1,
            string? format2
        )
    {
        Sure.NotNullNorEmpty (code);

        var item = new GblStatement
        {
            Command = VerifyCode (code),
            Parameter1 = parameter1,
            Parameter2 = parameter2,
            Format1 = format1,
            Format2 = format2
        };
        _statements.Add (item);

        return this;
    }

    /// <summary>
    /// Команда "ADD".
    /// </summary>
    public GblBuilder Add
        (
            string field,
            string value
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (value);

        return AddStatement
            (
                GblCode.Add,
                VerifyField (field),
                All,
                VerifyValue (value),
                Filler
            );
    }

    /// <summary>
    /// Команда "ADD".
    /// </summary>
    public GblBuilder Add
        (
            string field,
            string repeat,
            string value
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (repeat);
        Sure.NotNullNorEmpty (value);

        return AddStatement
            (
                GblCode.Add,
                VerifyField (field),
                VerifyRepeat (repeat),
                VerifyValue (value),
                Filler
            );
    }

    /// <summary>
    /// Команда "CHA".
    /// </summary>
    public GblBuilder Change
        (
            string field,
            string fromValue,
            string toValue
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (fromValue);
        Sure.NotNullNorEmpty (toValue);

        return AddStatement
            (
                GblCode.Change,
                VerifyField (field),
                All,
                VerifyValue (fromValue),
                VerifyValue (toValue)
            );
    }

    /// <summary>
    /// Команда "CHA".
    /// </summary>
    public GblBuilder Change
        (
            string field,
            string repeat,
            string fromValue,
            string toValue
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (repeat);
        Sure.NotNullNorEmpty (fromValue);
        Sure.NotNullNorEmpty (toValue);

        return AddStatement
            (
                GblCode.Change,
                VerifyField (field),
                VerifyRepeat (repeat),
                VerifyValue (fromValue),
                VerifyValue (toValue)
            );
    }

    /// <summary>
    /// Команда "DEL".
    /// </summary>
    public GblBuilder Delete
        (
            string field,
            string repeat
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (repeat);

        return AddStatement
            (
                GblCode.Delete,
                VerifyField (field),
                VerifyRepeat (repeat),
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Команда "DEL".
    /// </summary>
    public GblBuilder Delete
        (
            string field
        )
    {
        Sure.NotNullNorEmpty (field);

        return AddStatement
            (
                GblCode.Delete,
                VerifyField (field),
                All,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Команда "DELR".
    /// </summary>
    public GblBuilder DeleteRecord()
    {
        return AddStatement
            (
                GblCode.DeleteRecord,
                Filler,
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Выполнение глобальной корректировки на указанной базе данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            string database
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (database);

        return new GlobalCorrector
                (
                    connection,
                    connection.EnsureDatabase (database)
                )
            .ProcessWholeDatabase
                (
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки на текущей базе данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        return new GlobalCorrector
                (
                    connection,
                    connection.EnsureDatabase()
                )
            .ProcessWholeDatabase
                (
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки на результатах поиска
    /// по заданной базе данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            string database,
            string searchExpression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (database);
        Sure.NotNullNorEmpty (searchExpression);

        return new GlobalCorrector
                (
                    connection,
                    connection.EnsureDatabase (database)
                )
            .ProcessSearchResult
                (
                    searchExpression,
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки на заданном интервале
    /// записей указанной базы данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            string database,
            int fromMfn,
            int toMfn
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (database);
        Sure.NonNegative (fromMfn);
        Sure.NonNegative (toMfn);

        return new GlobalCorrector
                (
                    connection,
                    connection.EnsureDatabase (database)
                )
            .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки на заданном интервале записей.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            int fromMfn,
            int toMfn
        )
    {
        Sure.NotNull (connection);
        Sure.NonNegative (fromMfn);
        Sure.NonNegative (toMfn);

        return new GlobalCorrector
                (
                    connection,
                    connection.Database.ThrowIfNull()
                )
            .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки для заданного набора
    /// записей в указанной базе данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            string database,
            IEnumerable<int> recordset
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (database);
        Sure.NotNull (recordset);

        return new GlobalCorrector
                (
                    connection,
                    database
                )
            .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
    }

    /// <summary>
    /// Выполнение глобальной корректировки для указанного набора записей
    /// в текущей базе данных.
    /// </summary>
    public GblResult Execute
        (
            ISyncProvider connection,
            IEnumerable<int> recordset
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (recordset);

        return new GlobalCorrector
                (
                    connection,
                    connection.Database.ThrowIfNull()
                )
            .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
    }

    /// <summary>
    /// Команда "FI" - закрывает "IF".
    /// </summary>
    public GblBuilder Fi()
    {
        return AddStatement
            (
                GblCode.Fi,
                Filler,
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Команла "IF".
    /// </summary>
    public GblBuilder If
        (
            string condition
        )
    {
        Sure.NotNullNorEmpty (condition);

        return AddStatement
            (
                GblCode.If,
                VerifyValue (condition),
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Команда "IF".
    /// </summary>
    public GblBuilder If
        (
            string condition,
            params GblStatement[] statements
        )
    {
        Sure.NotNullNorEmpty (condition);
        Sure.NotNull (statements);

        If (condition);
        _statements.AddRange (statements);

        return Fi();
    }

    /// <summary>
    /// Команда "IF" с вложенным построителем.
    /// </summary>
    public GblBuilder If
        (
            string condition,
            GblBuilder builder
        )
    {
        Sure.NotNullNorEmpty (condition);
        Sure.NotNull (builder);

        return If
            (
                condition,
                builder.ToStatements()
            );
    }

    /// <summary>
    /// Пустой комментарий.
    /// </summary>
    public GblBuilder Nop()
    {
        return AddStatement
            (
                GblCode.Comment,
                Filler,
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Однострочный комментарий.
    /// </summary>
    public GblBuilder Nop
        (
            string comment
        )
    {
        return AddStatement
            (
                GblCode.Comment,
                VerifyValue (comment),
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Двустрочный комментарий.
    /// </summary>
    public GblBuilder Nop
        (
            string comment1,
            string comment2
        )
    {
        return AddStatement
            (
                GblCode.Comment,
                VerifyValue (comment1),
                VerifyValue (comment2),
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Команда "REP".
    /// </summary>
    public GblBuilder Replace
        (
            string field,
            string repeat,
            string toValue
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (repeat);
        Sure.NotNullNorEmpty (toValue);

        return AddStatement
            (
                GblCode.Replace,
                VerifyField (field),
                VerifyRepeat (repeat),
                VerifyValue (toValue),
                Filler
            );
    }

    /// <summary>
    /// Команда "REP".
    /// </summary>
    public GblBuilder Replace
        (
            string field,
            string toValue
        )
    {
        Sure.NotNullNorEmpty (field);
        Sure.NotNullNorEmpty (toValue);

        return AddStatement
            (
                GblCode.Replace,
                VerifyField (field),
                All,
                VerifyValue (toValue),
                Filler
            );
    }

    /// <summary>
    /// Построение массива команд.
    /// </summary>
    public GblStatement[] ToStatements()
    {
        return _statements.ToArray();
    }

    /// <summary>
    /// Команда "UNDO"
    /// </summary>
    public GblBuilder Undo
        (
            string version
        )
    {
        Sure.NotNullNorEmpty (version);

        return AddStatement
            (
                GblCode.Undo,
                VerifyParameter (version),
                Filler,
                Filler,
                Filler
            );
    }

    /// <summary>
    /// Верификация кода команды.
    /// </summary>
    public string VerifyCode
        (
            string code
        )
    {
        Sure.NotNullNorEmpty (code);

        // TODO some verification?

        return code;
    }

    /// <summary>
    /// Верификация спецификации поля/подполя для команды.
    /// </summary>
    public string VerifyField
        (
            string field
        )
    {
        Sure.NotNullNorEmpty (field);

        // TODO some verification?

        return field;
    }

    /// <summary>
    /// Верификация спецификации формата для команды.
    /// </summary>
    public string VerifyFormat
        (
            string format
        )
    {
        Sure.NotNullNorEmpty (format);

        // TODO some verification?
        // TODO: применить

        return format;
    }

    /// <summary>
    /// Верификация параметра для команды.
    /// </summary>
    public string VerifyParameter
        (
            string parameter
        )
    {
        Sure.NotNullNorEmpty (parameter);

        // TODO some verification?

        return parameter;
    }

    /// <summary>
    /// Верификация спецификации повторения поля для команды.
    /// </summary>
    public string VerifyRepeat
        (
            string repeat
        )
    {
        Sure.NotNullNorEmpty (repeat);

        // TODO some verification?

        return repeat;
    }

    /// <summary>
    /// Верификация значения для команды.
    /// </summary>
    public string VerifyValue
        (
            string value
        )
    {
        Sure.NotNullNorEmpty (value);

        // TODO some verification?

        return value;
    }

    #endregion
}
