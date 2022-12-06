// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XStyleSimulations.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Describes the simulation style of a font.
/// </summary>
[Flags]
public enum XStyleSimulations  // Identical to WpfStyleSimulations.
{
    /// <summary>
    /// No font style simulation.
    /// </summary>
    None = 0,

    /// <summary>
    /// Bold style simulation.
    /// </summary>
    BoldSimulation = 1,

    /// <summary>
    /// Italic style simulation.
    /// </summary>
    ItalicSimulation = 2,

    /// <summary>
    /// Bold and Italic style simulation.
    /// </summary>
    BoldItalicSimulation = ItalicSimulation | BoldSimulation,
}
