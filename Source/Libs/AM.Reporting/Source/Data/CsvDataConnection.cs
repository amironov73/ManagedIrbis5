// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* CsvDataConnection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Data;
using System.Data.Common;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents a connection to csv file-based database.
/// </summary>
/// <example>This example shows how to add a new connection to the report.
/// <code>
/// Report report1;
/// CsvDataConnection conn = new CsvDataConnection();
/// conn.CsvFile = @"c:\data.csv";
/// report1.Dictionary.Connections.Add(conn);
/// conn.CreateAllTables();
/// </code>
/// </example>
public class CsvDataConnection
    : DataConnectionBase
{
    #region Properties

    /// <summary>
    /// Gets or sets the path to .csv file.
    /// </summary>
    public string CsvFile
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.CsvFile;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                CsvFile = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets the codepage of the .csv file.
    /// </summary>
    public int Codepage
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.Codepage;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                Codepage = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets the separator of the .csv file.
    /// </summary>
    public string Separator
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.Separator;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                Separator = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets the value indicating that field names should be loaded from the first string of the file.
    /// </summary>
    public bool FieldNamesInFirstString
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.FieldNamesInFirstString;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                FieldNamesInFirstString = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets the value indicating that quotation marks should be removed.
    /// </summary>
    public bool RemoveQuotationMarks
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.RemoveQuotationMarks;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                RemoveQuotationMarks = value
            };
            ConnectionString = builder.ToString();
        }
    }


    /// <summary>
    /// Gets or sets the value indicating that field types fhould be converted.
    /// </summary>
    public bool ConvertFieldTypes
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.ConvertFieldTypes;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                ConvertFieldTypes = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets locale name used to auto-convert numeric fields, e.g. "en-US".
    /// </summary>
    public string NumberFormat
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.NumberFormat;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                NumberFormat = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets locale name used to auto-convert currency fields, e.g. "en-US".
    /// </summary>
    public string CurrencyFormat
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.CurrencyFormat;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                CurrencyFormat = value
            };
            ConnectionString = builder.ToString();
        }
    }

    /// <summary>
    /// Gets or sets locale name used to auto-convert datetime fields, e.g. "en-US".
    /// </summary>
    public string DateTimeFormat
    {
        get
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString);
            return builder.DateTimeFormat;
        }
        set
        {
            var builder = new CsvConnectionStringBuilder (ConnectionString)
            {
                DateTimeFormat = value
            };
            ConnectionString = builder.ToString();
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvDataConnection"/> class.
    /// </summary>
    public CsvDataConnection()
    {
        IsSqlBased = false;
    }

    #endregion

    #region Protected methods

    /// <inheritdoc cref="DataConnectionBase.CreateDataSet"/>
    protected override DataSet CreateDataSet()
    {
        var dataset = base.CreateDataSet();
        var builder = new CsvConnectionStringBuilder (ConnectionString);
        var rawLines = CsvUtils.ReadLines (builder);
        var table = CsvUtils.CreateDataTable (builder, rawLines);
        if (table != null)
        {
            dataset.Tables.Add (table);
        }

        return dataset;
    }

    /// <inheritdoc cref="DataConnectionBase.SetConnectionString"/>
    protected override void SetConnectionString
        (
            string value
        )
    {
        DisposeDataSet();
        base.SetConnectionString (value);
    }

    #endregion

    #region Public methods

    /// <inheritdoc cref="DataConnectionBase.FillTableSchema"/>
    public override void FillTableSchema
        (
            DataTable table,
            string selectCommand,
            CommandParameterCollection parameters
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="DataConnectionBase.FillTableData"/>
    public override void FillTableData
        (
            DataTable table,
            string selectCommand,
            CommandParameterCollection parameters
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="DataConnectionBase.CreateTable"/>
    public override void CreateTable
        (
            TableDataSource source
        )
    {
        Sure.NotNull (source);

        if (DataSet.Tables.Count == 1)
        {
            source.Table = DataSet.Tables[0];
            base.CreateTable (source);
        }
        else
        {
            source.Table = null;
        }
    }

    /// <inheritdoc cref="DataConnectionBase.DeleteTable"/>
    public override void DeleteTable
        (
            TableDataSource source
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="DataConnectionBase.QuoteIdentifier"/>
    public override string QuoteIdentifier
        (
            string value,
            DbConnection connection
        )
    {
        Sure.NotNullNorEmpty (value);

        return value;
    }

    /// <inheritdoc cref="DataConnectionBase.GetTableNames"/>
    public override string[] GetTableNames()
    {
        var result = new string[DataSet.Tables.Count];
        for (var i = 0; i < DataSet.Tables.Count; i++)
        {
            result[i] = DataSet.Tables[i].TableName;
        }

        return result;
    }

    #endregion
}
