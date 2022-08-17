// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridDrawLayout.cs -- описание взаимного расположения элементов в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Описание взаимного расположения элементов в гриде.
/// </summary>
public sealed class TreeGridDrawLayout
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridDrawLayout()
    {
        Expand = Rectangle.Empty;
        Check = Rectangle.Empty;
        Icon = Rectangle.Empty;
        Text = Rectangle.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Зона, где ожидается клик мышью для развертывания региона.
    /// </summary>
    public Rectangle Expand { get; set; }

    /// <summary>
    /// Зона, где ожидается клик мышью для отметки элемента.
    /// </summary>
    public Rectangle Check { get; set; }

    /// <summary>
    /// Зона с иконкой.
    /// </summary>
    public Rectangle Icon { get; set; }

    /// <summary>
    /// Зона с текстом.
    /// </summary>
    public Rectangle Text { get; set; }

    /// <summary>
    /// Переопределение текста.
    /// </summary>
    public string? TextOverride { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Определение вида клика мышью по гриду
    /// </summary>
    public TreeGridClickKind DetermineClickKind
        (
            Point point
        )
    {
        if (Expand.Contains (point))
        {
            return TreeGridClickKind.Expand;
        }

        if (Check.Contains (point))
        {
            return TreeGridClickKind.Check;
        }

        if (Icon.Contains (point))
        {
            return TreeGridClickKind.Icon;
        }

        if (Text.Contains (point))
        {
            return TreeGridClickKind.Text;
        }

        return TreeGridClickKind.Unknown;
    }

    #endregion
}
