// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global

/* Cumulator.cs -- умеет кумулировать номера журналов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Linq;
using AM.Text.Ranges;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Умеет кумулировать номера журналов.
/// </summary>
public sealed class Cumulator
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Cumulator
        (
            ILogger<Cumulator> logger
        )
    {
        _logger = logger;
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    #endregion

    #region Public methods

    /// <summary>
    /// Кумуляция номеров.
    /// </summary>
    public IList<MagazineCumulation> Cumulate
        (
            IReadOnlyList<MagazineIssueInfo> issues,
            CumulationMethod method
        )
    {
        Sure.NotNull ((object) issues);
        method.NotUsed();

        var result = new List<MagazineCumulation>();
        var years = issues.Select (i => i.Year)
            .NonEmptyLines()
            .TrimLines()
            .NonEmptyLines()
            .Distinct()
            .OrderBy (s => s)
            .ToArray();
        foreach (var year in years)
        {
            var numbers = issues
                .Where (i => i.Year == year)
                .Select (i => i.Number)
                .NonEmptyLines()
                .Distinct()
                .OrderBy (s => s)
                .ToArray();
            var cumulated = NumberRangeCollection.Cumulate (numbers).ToString();

            _logger.LogInformation ("{Year}: {Cumulated}", year, cumulated);

            // record.Add (909, 'q', year, 'h', cumulated);
            var cumulation = new MagazineCumulation
            {
                Year = year,
                Numbers = cumulated
            };
            result.Add (cumulation);
        }

        return result;
    }

    #endregion
}
