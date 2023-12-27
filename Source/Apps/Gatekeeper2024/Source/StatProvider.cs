// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StatProvider.cs -- статистика входа-выхода
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
/// Статистика входа-выхода.
/// </summary>
internal sealed class StatProvider
{
    #region Public methods

    /// <summary>
    /// Обработка запроса.
    /// </summary>
    public IResult HandleRequest
        (
            HttpContext _
        )
    {
        var result = new List<StatEntry>();
        using var connection = Utility.ConnectToIrbis();
        if (connection is null)
        {
            return Results.Json (result);
        }

        // непростой подсчет посетителей, прошедших через турникет
        var today = IrbisDate.TodayText;
        var term = $"VS={today}/*";
        var postingsParameters = new PostingParameters
        {
            Database = connection.EnsureDatabase(),
            Terms = new[] { term }
        };
        var postings = connection.ReadPostings (postingsParameters);
        var visitCount = 0;
        var department = Utility.GetDepartment();
        var person = Utility.GetPerson();
        if (postings is not null)
        {
            var mfns = postings.Select (it => it.Mfn)
                .Order()
                .Distinct()
                .ToArray();
            var records = connection.ReadRecords
                (
                    connection.EnsureDatabase(),
                    mfns
                );
            if (records is not null)
            {
                foreach (var record in records)
                {
                    foreach (var field in record.EnumerateField (VisitInfo.Tag))
                    {
                        var visit = VisitInfo.Parse (field);
                        if (
                                visit.IsVisit
                                && visit.DateGivenString == today
                                && visit.Department == department
                                && visit.Responsible == person
                            )
                        {
                            visitCount++;
                        }
                    }
                }
            }
        }

        result.Add (new ()
        {
            Title = "Посещений",
            Value = visitCount.ToInvariantString()
        });

        // Количество читателей, находящихся в данный момент в библиотеке
        const string expression = "VIS=$";
        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = expression
        };
        var readers = connection.Search (searchParameters);
        var insiderCount = readers?.Length ?? 0;
        result.Add (new ()
        {
            Title = "В библиотеке",
            Value = insiderCount.ToInvariantString()
        });


        // Длина очереди на отправку
        var queueDirectory = Utility.GetQueueDirectory();
        var files = Directory.GetFiles (queueDirectory);
        result.Add (new ()
        {
            Title = "Очередь на отправку",
            Value = files.Length.ToInvariantString()
        });

        // фейки нужны при отладке
        // var fake = StatEntry.FakeEntries();

        return Results.Json (result);
    }

    #endregion
}
