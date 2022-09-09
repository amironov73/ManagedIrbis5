// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FormatterEnvironment.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.Layout;

/// <summary>
///
/// </summary>
internal class FormatterEnvironment
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public XFont? Font { get; set; }

    /// <summary>
    ///
    /// </summary>
    public XBrush? Brush { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double LineSpace { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double CyAscent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double CyDescent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public double SpaceWidth { get; set; }

    #endregion
}
