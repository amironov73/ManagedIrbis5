// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SamovarOptions.cs -- опции "самоварного поиска"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Опции "самоварного поиска".
/// </summary>
public sealed class SamovarOptions
{
    #region Properties

    /// <summary>
    /// База данных для поиска.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// Оценщик релевантности записей.
    /// </summary>
    public IRelevanceEvaluator? Evaluator { get; set; }

    /// <summary>
    /// Ограничение на количество выдаваемых записей.
    /// </summary>
    public int OutputLimit { get; set; } = 20;

    #endregion
}
