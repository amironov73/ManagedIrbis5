// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BorderFlags3D.cs -- указание, как рисовать внешнюю и внутреннюю рамки окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Указание, как рисовать внешнюю и внутреннюю рамки окна.
/// </summary>
[Flags]
public enum BorderFlags3D
{
    /// <summary>
    /// Приподнятый внешний край.
    /// </summary>
    BDR_RAISEDOUTER = 0x0001,

    /// <summary>
    /// Утопленный внешний край.
    /// </summary>
    BDR_SUNKENOUTER = 0x0002,

    /// <summary>
    /// Приподнятый внутренний край.
    /// </summary>
    BDR_RAISEDINNER = 0x0004,

    /// <summary>
    /// Утопленный внутренний край.
    /// </summary>
    BDR_SUNKENINNER = 0x0008,

    /// <summary>
    /// Приподнятый внешний и утопленный внутренний края.
    /// </summary>
    BDR_OUTER = BDR_RAISEDOUTER | BDR_SUNKENOUTER,

    /// <summary>
    /// Приподнятый внутренний и утопленный внешний края.
    /// </summary>
    BDR_INNER = BDR_RAISEDINNER | BDR_SUNKENINNER,

    /// <summary>
    /// Приподнятые внутренний и внешний края.
    /// </summary>
    BDR_RAISED = BDR_RAISEDOUTER | BDR_RAISEDINNER,

    /// <summary>
    /// Утопленные внутренний и внешний края.
    /// </summary>
    BDR_SUNKEN = BDR_SUNKENOUTER | BDR_SUNKENINNER,

    /// <summary>
    /// Комбинация BDR_RAISEDOUTER и BDR_RAISEDINNER.
    /// </summary>
    EDGE_RAISED = BDR_RAISEDOUTER | BDR_RAISEDINNER,

    /// <summary>
    /// Комбинация BDR_SUNKENOUTER и BDR_SUNKENINNER.
    /// </summary>
    EDGE_SUNKEN = BDR_SUNKENOUTER | BDR_SUNKENINNER,

    /// <summary>
    /// Комбинация BDR_SUNKENOUTER и BDR_RAISEDINNER.
    /// </summary>
    EDGE_ETCHED = BDR_SUNKENOUTER | BDR_RAISEDINNER,

    /// <summary>
    /// Комбинация BDR_RAISEDOUTER и BDR_SUNKENINNER.
    /// </summary>
    EDGE_BUMP = BDR_RAISEDOUTER | BDR_SUNKENINNER
}
