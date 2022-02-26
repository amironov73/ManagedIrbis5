// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IGraph.cs -- интерфейс графа
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Rdf;

/// <summary>
/// Интерфейс графа.
/// </summary>
public interface IGraph
{
    /// <summary>
    /// Добавляет триплет в граф.
    /// </summary>
    bool Assert (Triple triple);

    /// <summary>
    /// Удаляет триплет из графа.
    /// </summary>
    bool Retract (Triple triple);
}
