// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* ReadOnlySytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// This style is used to mark range of text as ReadOnly block
/// </summary>
/// <remarks>You can inherite this style to add visual effects of readonly text</remarks>
public class ReadOnlyStyle
    : Style
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public ReadOnlyStyle()
    {
        IsExportable = false;
    }

    #endregion

    /// <inheritdoc cref="Style.Draw"/>
    public override void Draw (Graphics graphics, Point position, TextRange range)
    {
        //
    }
}
