// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IRelevanceEvaluator.cs -- интерфейс оценки релевантности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Интерфейс оценки релевантности.
/// </summary>
public interface IRelevanceEvaluator
{
    /// <summary>
    /// Вычисление релеватности указанной записи.
    /// </summary>
    double EvaluateRelevance (Record record);
}
