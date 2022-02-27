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

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Linq;
using AM.Text.Ranges;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Умеет кумулировать номера журналов.
/// </summary>
public sealed class Cumulator
{
    #region Private members

    /// <summary>
    /// Подготовка значения подполя.
    /// </summary>
    private static string? _PrepareValue
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        var result = value.Trim().ToUpperInvariant().EmptyToNull();

        return result;
    }

    /// <summary>
    /// Кумуляция по указанному критерию отбора.
    /// </summary>
    private CumulatedIssues[] Cumulate
        (
            IReadOnlyList<CumulationData>? issues,
            Func<CumulationData, string?> selector
        )
    {
        if (issues is null || issues.Count == 0)
        {
            return Array.Empty<CumulatedIssues>();
        }

        var result = new List<CumulatedIssues>();

        var keys = issues.Select (selector)
            .Select (_PrepareValue)
            .Distinct()
            .OrderBy (s => s)
            .ToArray();

        foreach (var key in keys)
        {
            var cumulated = new List<CumulationData>();
            foreach (var issue in issues)
            {
                var value = _PrepareValue (selector (issue));
                if (value == key)
                {
                    cumulated.Add (issue);
                }
            }

            var one = new CumulatedIssues()
            {
                Key = key,
                Issues = cumulated.ToArray()
            };
            result.Add (one);
        }

        return result.ToArray();
    }

    private static string _CumulateNumbers
        (
            IEnumerable<CumulationData?>? issues
        )
    {
        if (issues is null)
        {
            return string.Empty;
        }

        var numbers = issues
            .NonNullItems()
            .Select (issue => issue.Number)
            .NonEmptyLines();
        var result = NumberRangeCollection.Cumulate (numbers).ToString();

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Извлечение экземпляров для последующей кумуляции.
    /// </summary>
    public CumulationData[] ExtractCumulationData
        (
            IReadOnlyList<MagazineIssueInfo> issues
        )
    {
        Sure.NotNull (issues);

        var result = new List<CumulationData>();
        foreach (var issue in issues)
        {
            if (issue.Exemplars is { } exemplars)
            {
                foreach (var exemplar in exemplars)
                {
                    var data = new CumulationData()
                    {
                        Number = issue.Number,
                        Place = exemplar.Place,
                        Volume = issue.Volume,
                        Year = issue.Year,
                        Complect = exemplar.Number
                    };
                    result.Add (data);
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Кумуляция выпусков строго по годам/томам.
    /// </summary>
    public IList<MagazineCumulation> CumulateByYear
        (
            IReadOnlyList<MagazineIssueInfo> issues
        )
    {
        Sure.NotNull ((object) issues);

        var result = new List<MagazineCumulation>();
        var data = ExtractCumulationData (issues);
        var byYear = Cumulate (data, d => d.Year);
        foreach (var year in byYear)
        {
            var byVolume = Cumulate (year.Issues, d => d.Volume);
            foreach (var volume in byVolume)
            {
                var numbers = _CumulateNumbers (volume.Issues);
                if (!string.IsNullOrEmpty (numbers))
                {
                    var one = new MagazineCumulation
                    {
                        Year = year.Key,
                        Volume = volume.Key,
                        Numbers = numbers
                    };
                    result.Add (one);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Кумуляция выпусков по месту хранения.
    /// </summary>
    public IList<MagazineCumulation> CumulateByPlace
        (
            IReadOnlyList<MagazineIssueInfo> issues
        )
    {
        Sure.NotNull ((object) issues);

        var result = new List<MagazineCumulation>();
        var data = ExtractCumulationData (issues);
        var byYear = Cumulate (data, d => d.Year);
        foreach (var year in byYear)
        {
            var byVolume = Cumulate (year.Issues, d => d.Volume);
            foreach (var volume in byVolume)
            {
                var byPlace = Cumulate (volume.Issues, d => d.Place);
                foreach (var place in byPlace)
                {
                    var numbers = _CumulateNumbers (place.Issues);
                    if (!string.IsNullOrEmpty (numbers))
                    {
                        var one = new MagazineCumulation
                        {
                            Year = year.Key,
                            Volume = volume.Key,
                            Place = place.Key,
                            Numbers = numbers
                        };
                        result.Add (one);
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Кумуляция выпусков по номеру комплекта.
    /// </summary>
    public IList<MagazineCumulation> CumulateByComplect
        (
            IReadOnlyList<MagazineIssueInfo> issues
        )
    {
        Sure.NotNull ((object) issues);

        var result = new List<MagazineCumulation>();
        var data = ExtractCumulationData (issues);
        var byYear = Cumulate (data, d => d.Year);
        foreach (var year in byYear)
        {
            var byVolume = Cumulate (year.Issues, d => d.Volume);
            foreach (var volume in byVolume)
            {
                var byComplect = Cumulate (volume.Issues, d => d.Complect);
                foreach (var complect in byComplect)
                {
                    var byPlace = Cumulate (complect.Issues, d => d.Place);
                    foreach (var place in byPlace)
                    {
                        var numbers = _CumulateNumbers (place.Issues);
                        if (!string.IsNullOrEmpty (numbers))
                        {
                            var one = new MagazineCumulation
                            {
                                Year = year.Key,
                                Volume = volume.Key,
                                Place = place.Key,
                                ComplectNumber = complect.Key,
                                Numbers = numbers
                            };
                            result.Add (one);
                        }
                    }
                }
            }
        }

        return result;
    }

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

        return method switch
        {
            CumulationMethod.Year => CumulateByYear (issues),
            CumulationMethod.Place => CumulateByPlace (issues),
            CumulationMethod.Complect => CumulateByComplect (issues),
            _ => throw new ArgumentOutOfRangeException (nameof (method))
        };
    }

    #endregion
}
