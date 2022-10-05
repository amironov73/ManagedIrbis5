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

using Microsoft.Extensions.DependencyInjection;
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
    public string SearchExpression { get; }

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

        _logger = serviceProvider.GetRequiredService<ILogger<StandardRelevanceEvaluator>>();

        Coefficients = new List<RelevanceCoefficient>();
        SearchExpression = searchExpression;
        ServiceProvider = serviceProvider;

        ExtraneousRelevance = 1;
        Multiplier = 2;
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    private double EvaluateSubfield
        (
            SubField subField,
            double supposedValue
        )
    {
        var value = subField.Value;

        if (string.IsNullOrWhiteSpace (value))
        {
            return 0;
        }

        if (value.Contains (SearchExpression))
        {
            if (value.SameString (SearchExpression))
            {
                return Multiplier * supposedValue;
            }

            return supposedValue;
        }

        return 0.0;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация для стандартного каталога IBIS.
    /// </summary>
    public void InitializeForIbis()
    {
        // попадание в заглавие или в имя автора
        Coefficients.Add (new (10) { Fields = { 200, 700, 701, 710 }});

        // попадание редактора
        Coefficients.Add (new (7) { Fields = { 702 }});

        // TODO добавить рубрики и ключевые слова
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
