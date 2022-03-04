// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MssqlScripter.cs -- генерирует базу данных по ее описанию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

/*
    Типы данных в SQL Server объединены в следующие категории:

    * Точные числа;
    * Приблизительные числа;
    * Символьные строки;
    * Символьные строки в Юникоде;
    * Дата и время;
    * Двоичные данные;
    * Прочие типы данных.

    Точные числа:

    * int        - 4 байта (применяем по умолчанию);
    * tinyint    - 1 байт;
    * smallint   - 2 байта;
    * bigint     - 8 байт;
    * decimal    - в зависимости от запрошенной точности от 5 до 17 байт, по умолчанию 9 байт;
    * numeric    - алиас для decimal;
    * bit        - 1 бит;
    * money      - 8 байт;
    * smallmoney - 4 байта.

    Приблизительные числа:

    * float - в зависимости от запрошенной точности от 4 до 8 байт, по умолчанию 8 байт;
    * real  - всегда 4 байта.

    Дата и время:

    * date           - 3 байта;
    * time           - 5 байт;
    * datetime       - 8 байт;
    * datetime2      - от 6 до 8 байт в зависимости от заданной точности;
    * datetimeoffset - 10 байт;
    * smalldatetime  - 4 байта.

    Символьные строки:

    * char    - от 1 до 8000 символов;
    * varchar - от 1 до 8000 символов, переменная длина;
    * text    - переменная длина.

    Символьные строки в Юникоде:

    * nchar    - от 1 до 4000 символов;
    * nvarchar - от 1 до 4000 символов, переменная длина;
    * ntext    - переменная длина.

    Двоичные данные:

    * binary    - от 1 до 8000 байт;
    * varbinary - от 1 до 8000 байт, переменная длина;
    * image     - переменная длина.

    Прочие типы данных:

    * rowversion       - 8 байт;
    * uniqueidentifier - 16 байт (GUID);
    * sql_variant      - до 8000 байт;
    * xml              - до 2 Гб.

 */

namespace AM.Data;

/// <summary>
/// Генерирует базу данных по ее описанию.
/// </summary>
public sealed class MssqlScripter
{
    #region Properties

    /// <summary>
    /// Владелец.
    /// </summary>
    public string Owner { get; set; }= "[dbo]";

    /// <summary>
    /// Создавать, только если не существует.
    /// </summary>
    public bool IfNotExist { get; set; }

    /// <summary>
    /// Удалять, если существует.
    /// </summary>
    public bool DropIfExist { get; set; }

    #endregion

    #region Private members

    private void _Go
        (
            TextWriter output
        )
    {
        output.WriteLine();
        output.WriteLine ("GO");
    }

    private void _Indent
        (
            TextWriter output,
            int width = 4
        )
    {
        for (var i = 0; i < width; i++)
        {
            output.Write (' ');
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Генерация базы данных по ее описанию.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="database">Описание базы данных.</param>
    public void GenerateDatabase
        (
            TextWriter output,
            DatabaseDescriptor database
        )
    {
        Sure.NotNull (output);
        Sure.VerifyNotNull (database);

        if (DropIfExist)
        {
            output.WriteLine ($"drop database if exists [{database.Name}]");
            _Go (output);
        }
        else if (IfNotExist)
        {
            output.WriteLine ($"if not exists (select * from sys.databases where name='{database.Name}')");
        }

        output.WriteLine ($"create database [{database.Name}]");
        _Go (output);
        output.WriteLine();
        output.WriteLine ($"use [{database.Name}]");
        _Go (output);
        output.WriteLine();

        var first = true;
        foreach (var table in database.Tables.ThrowIfNull())
        {
            if (!first)
            {
                output.WriteLine();
            }

            GenerateTable (output, table);
            first = false;
        }
    }

    /// <summary>
    /// Генерация поля таблицы.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="field">Описание поля таблицы.</param>
    public void GenerateField
        (
            TextWriter output,
            FieldDescriptor field
        )
    {
        Sure.NotNull (output);
        Sure.VerifyNotNull (field);

        _Indent (output);
        var fieldName = "[" + field.Name + "]";
        output.Write ($"{fieldName, -16} ");

        string typespec;
        switch (field.Type)
        {
            case DataType.Boolean:
                typespec = "[bit]";
                break;

            case DataType.Integer:
                switch (field.Length)
                {
                    case 0:
                    case 4:
                        typespec = "[int]";
                        break;

                    case 1:
                        typespec = "[tinyint]";
                        break;

                    case 2:
                        typespec = "[smallint]";
                        break;

                    case 8:
                        typespec = "[bigint]";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException (nameof (field.Length));
                }

                break;

            case DataType.Money:
                typespec = "[money]";
                break;

            case DataType.Text:
                if (field.Length < 0)
                {
                    throw new ArgumentOutOfRangeException (nameof (field.Length));
                }
                else if (field.Length == 0)
                {
                    typespec = "[ntext]";
                }
                else
                {
                    typespec = $"[nvarchar]({field.Length})";
                }
                break;

            case DataType.Date:
                switch (field.Length)
                {
                    case 0:
                    case 8:
                        typespec = "[datetime]";
                        break;

                    case 4:
                        typespec = "[smalldatetime]";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException (nameof (field.Length));
                }
                break;

            default:
                throw new ArgumentOutOfRangeException (nameof (field.Type));
        }

        output.Write ($"{typespec, -16} ");

        if (field.Identity)
        {
            output.Write ("identity ");
        }

        if (field.PrimaryKey)
        {
            output.Write ("primary key ");
        }
        else
        {
            if (field.Unique)
            {
                output.Write ("unique ");
            }
            else
            {
                if (field.Indexed)
                {
                    output.Write ($"index [IX_{field.Name}] ");
                }
            }
        }

        output.Write (field.Nullable ? "NULL" : "NOT NULL");

    }

    /// <summary>
    /// Генерация индекса.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="index">Описание индекса.</param>
    public void GenerateIndex
        (
            TextWriter output,
            IndexDescriptor index
        )
    {
        Sure.NotNull (output);
        Sure.VerifyNotNull (index);

        // TODO implement
    }

    /// <summary>
    /// Генерация таблицы по ее описанию.
    /// </summary>
    /// <param name="output">Куда помещать результат.</param>
    /// <param name="table">Описание таблицы.</param>
    public void GenerateTable
        (
            TextWriter output,
            TableDescriptor table
        )
    {
        Sure.NotNull (output);
        Sure.VerifyNotNull (table);

        if (DropIfExist)
        {
            output.WriteLine ($"drop table if exists [{table.Name}]");
        }
        else if (IfNotExist)
        {
            output.WriteLine ($"if not exists (select * from sysobjects where name='{table.Name}' and xtype='U')");
        }

        output.WriteLine ($"create table {Owner}.[{table.Name}]");
        output.WriteLine("(");
        var first = true;
        foreach (var field in table.Fields.ThrowIfNull())
        {
            if (!first)
            {
                output.WriteLine (",");
            }

            GenerateField (output, field);
            first = false;
        }

        if (table.Indexes is not null)
        {
            output.WriteLine (",");
            output.WriteLine();
            foreach (var index in table.Indexes)
            {
                GenerateIndex (output, index);
            }
        }

        output.WriteLine();
        output.WriteLine(")");

        _Go (output);
    }

    #endregion
}
