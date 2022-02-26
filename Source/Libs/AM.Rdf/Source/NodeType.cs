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

/* NodeType.cs -- типы узлов
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Rdf;

/// <summary>
/// Типы узлов.
/// </summary>
public enum NodeType
{
    /// <summary>
    /// Пустой.
    /// </summary>
    Blank = 0,

    /// <summary>
    /// URI.
    /// </summary>
    Uri = 1,

    /// <summary>
    /// Литерал.
    /// </summary>
    Literal = 2,

    /// <summary>
    /// Графовый литерал.
    /// </summary>
    GraphLiteral = 3,

    /// <summary>
    /// Переменная.
    /// </summary>
    Variable = 4
}
