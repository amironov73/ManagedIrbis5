// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HistoryProvider.cs -- история входов/выходов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// История входов-выходов.
/// </summary>
internal sealed class HistoryProvider
{
    #region Construction

    // /// <summary>
    // /// Конструктор.
    // /// </summary>
    // public HistoryProvider
    //     (
    //         ILogger<HistoryProvider> logger
    //     )
    // {
    //     _logger = logger;
    // }

    #endregion

    #region Private members

    // private readonly ILogger _logger;

    private static string ConvertTime
        (
            string? source
        )
    {
        var span = IrbisDate.ConvertStringToTime (source);
        return span == default ? "--" : span.ToHourString();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Обработка запроса.
    /// </summary>
    public IResult HandleRequest
        (
            HttpContext _
        )
    {
        var result = new List<HistoryEntry>();
        using var connection = Utility.ConnectToIrbis();
        if (connection is null)
        {
            return Results.Json (result);
        }

        var today = IrbisDate.TodayText;
        var expression = $"VS={today}/*";
        var postingsParameters = new PostingParameters
        {
            Database = connection.EnsureDatabase(),
            Terms = new[] { expression }
        };
        var postings = connection.ReadPostings (postingsParameters);
        if (postings is null || postings.Length == 0)
        {
            return Results.Json (result);
        }

        var mfns = postings.Select (it => it.Mfn)
            .Order()
            .Distinct().ToArray();
        var records = connection.ReadRecords (connection.EnsureDatabase(), mfns);
        if (records is null || records.Length == 0)
        {
            return Results.Json (result);
        }

        var department = Utility.GetDepartment();
        foreach (var record in records)
        {
            var reader = ReaderInfo.Parse (record);
            foreach (var field in record.EnumerateField (VisitInfo.Tag))
            {
                var visit = VisitInfo.Parse (field);
                if (
                        visit.DateGivenString == today
                        && visit.Department == department
                        && visit.IsVisit
                    )
                {
                    result.Add (new ()
                    {
                        Ticket = reader.Ticket,
                        TimeIn = ConvertTime (visit.TimeIn),
                        TimeOut = ConvertTime (visit.TimeOut),
                        Name = reader.FullName
                    });
                }
            }
        }

        result.Sort (HistoryEntry.CompareReverse);
        return Results.Json (result);

        // var fake = HistoryEntry.FakeEntries();
        // return Results.Json (fake);
    }

    #endregion
}
