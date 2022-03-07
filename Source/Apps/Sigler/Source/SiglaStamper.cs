// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SiglaStamper.cs --
 * Ars Magna project, http://arsmagna.ru
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Text.Output;

using ManagedIrbis;
using ManagedIrbis.Providers;

#endregion

namespace Sigler;

/// <summary>
///
/// </summary>
public sealed class SiglaStamper
    : IDisposable
{
    #region Properties

    public SyncConnection Connection { get; }

    public AbstractOutput Output { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SiglaStamper
        (
            string connectionString,
            AbstractOutput output
        )
    {
        Output = output;
        var connection = ConnectionFactory.Shared.CreateSyncConnection();
        connection.ParseConnectionString (connectionString);
        connection.Connect();
        Connection = connection;
    }

    #endregion

    #region Private members

    private Stopwatch? _stopwatch;

    /// <summary>
    /// Форматируем отрезок времени
    /// в виде ЧЧ:ММ:СС.
    /// </summary>
    private static string _FormatTimeSpan
        (
            TimeSpan timeSpan
        )
    {
        var result = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

        return result;
    }

    #endregion

    #region Public methods

    public void ProcessRecord
        (
            Record record,
            string newSigla,
            string number
        )
    {
        var field = record.Fields
            .GetField (910)
            .GetField ('b', number)
            .FirstOrDefault();
        if (ReferenceEquals (field, null))
        {
            Output.WriteLine ("{0}: no 910", number);

            return;
        }

        var existingSigla = field.GetSubFieldValue ('d');
        if (newSigla.SameString (existingSigla))
        {
            Output.Write ("{0} ", record.Mfn);
        }
        else
        {
            if (field.GetSubFieldValue ('a').SameString ("5"))
            {
                field.SetSubFieldValue ('a', "0");
            }

            field.SetSubFieldValue ('d', newSigla);

            Connection.WriteRecord (record, false, true);
            Output.Write ("[{0}] ", record.Mfn);
        }
    }

    public void ProcessNumber
        (
            int index,
            string sigla,
            string number
        )
    {
        number = number.Trim();
        if (string.IsNullOrEmpty (number))
        {
            return;
        }

        if (_stopwatch is not null)
        {
            Output.Write
                (
                    "{0,6}) {1} ",
                    index,
                    _FormatTimeSpan (_stopwatch.Elapsed)
                );
        }

        var mfns = Connection.Search ("\"IN=" + number + "\"");
        if (mfns.Length == 0)
        {
            Output.WriteLine ("{0}: not found", number);

            return;
        }

        Output.Write ("{0}: ", number);

        foreach (var mfn in mfns)
        {
            var record = Connection.ReadRecord (mfn);
            if (record is not null)
            {
                ProcessRecord
                    (
                        record,
                        sigla,
                        number
                    );
            }
        }

        Console.WriteLine();
    }

    public void ProcessFile
        (
            string fileName
        )
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        var sigla = Path.GetFileNameWithoutExtension (fileName);
        var index = 0;

        using var reader = new StreamReader (fileName, Encoding.Default);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            ProcessNumber
                (
                    ++index,
                    sigla,
                    line
                );
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Connection.Dispose();
    }

    #endregion
}
