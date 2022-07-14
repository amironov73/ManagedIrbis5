// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ToolStripExtensions.cs -- методы расширения для ToolStrip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ToolStrip"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ToolStripExtensions
{
    #region Public methods

    /// <summary>
    /// Добавление элементов в <see cref="ToolStrip"/>.
    /// </summary>
    public static TToolStrip Items<TToolStrip>
        (
            this TToolStrip strip,
            params ToolStripItem[] items
        )
        where TToolStrip: ToolStrip
    {
        Sure.NotNull (strip);
        Sure.NotNull (items);

        strip.Items.AddRange (items);

        return strip;
    }

    #endregion
}
