// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StandardRelevanceEvaluator.cs -- стандартный оценщик релевантности записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Стандартный оценщик релеватности записей.
/// </summary>
public sealed class StandardRelevanceEvaluator
    : IRelevanceEvaluator
{
    #region Properties

    /// <summary>
    /// Список коэффициентов релевантности.
    /// </summary>
    public IList<RelevanceCoefficient> Coefficients { get; }

    /// <summary>
    /// Поисковое выражение.
    /// </summary>
    public string SearchExpression { get; private set; }

    /// <summary>
    /// Термины поискового запроса.
    /// </summary>
    public IReadOnlyList<string> Terms { get; private set; }

    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Релеватность для упоминаний в посторонних полях.
    /// </summary>
    public double ExtraneousRelevance { get; set; }

    /// <summary>
    /// Мультипликатор для случая полного совпадения.
    /// </summary>
    public double Multiplier { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StandardRelevanceEvaluator
        (
            IServiceProvider serviceProvider,
            string searchExpression
        )
    {
        Sure.NotNull (serviceProvider);
        Sure.NotNullNorEmpty (searchExpression);

        // чтобы компилятор не жаловался
        SearchExpression = string.Empty;
        Terms = Array.Empty<string>();

        _logger = LoggingUtility.GetLogger
            (
                serviceProvider,
                typeof (StandardRelevanceEvaluator)
            );

        Coefficients = new List<RelevanceCoefficient>();
        SetSearchExpression (searchExpression);
        ServiceProvider = serviceProvider;

        InitializeForIbis();
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    private double EvaluateText
        (
            string text,
            string term,
            double supposedValue
        )
    {
        if (text.Contains (term, StringComparison.OrdinalIgnoreCase))
        {
            return string.Compare (text, term, StringComparison.OrdinalIgnoreCase) == 0
                ? supposedValue * Multiplier
                : supposedValue;
        }

        return 0.0;
    }

    private double EvaluateSubfield
        (
            SubField subField,
            double supposedValue
        )
    {
        var text = subField.Value;
        var result = 0.0;

        if (string.IsNullOrWhiteSpace (text))
        {
            return 0;
        }

        foreach (var term in Terms)
        {
            result += EvaluateText (text, term, supposedValue);
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация для стандартного каталога IBIS.
    /// </summary>
    public void InitializeForIbis()
    {
        var settings = RelevanceSettings.ForIbis();

        ExtraneousRelevance = settings.ExtraneousRelevance;
        Multiplier = settings.Multiplier;
        Coefficients.Clear();
        foreach (var coefficient in settings.Coefficients)
        {
            Coefficients.Add (coefficient);
        }
    }

    /// <summary>
    /// Установка
    /// </summary>
    /// <param name="searchExpression"></param>
    public void SetSearchExpression
        (
            string searchExpression
        )
    {
        Sure.NotNullNorEmpty (searchExpression);

        var program = SearchProgram.Parse (searchExpression);
        var terms = SearchQueryUtility.ExtractTerms (program);
        Terms = SearchQueryUtility.StripTerms (terms);

        SearchExpression = searchExpression;
    }

    #endregion

    #region IRelevanceEvaluator members

    /// <inheritdoc cref="IRelevanceEvaluator.EvaluateRelevance"/>
    public double EvaluateRelevance
        (
            Record record
        )
    {
        Sure.NotNull (record);

        _logger.LogTrace (nameof (EvaluateRelevance));

        var result = 0.0;
        foreach (var coefficient in Coefficients)
        {
            foreach (var tag in coefficient.Fields)
            {
                foreach (var field in record.EnumerateField (tag))
                {
                    foreach (var subfield in field.Subfields)
                    {
                        result += EvaluateSubfield (subfield, coefficient.Value);
                    }
                }
            }
        }

        foreach (var field in record.Fields)
        {
            foreach (var subfield in field.Subfields)
            {
                result += EvaluateSubfield (subfield, ExtraneousRelevance);
            }
        }

        return result;
    }

    #endregion
}
