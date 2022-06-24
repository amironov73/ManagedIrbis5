// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RFontFamily.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for platform specific font family object - define the available font families to use.<br/>
/// Required for custom fonts handling: fonts that are not installed on the system.
/// </summary>
public abstract class RFontFamily
{
    /// <summary>
    /// Gets the name of this Font Family.
    /// </summary>
    public abstract string Name { get; }
}
