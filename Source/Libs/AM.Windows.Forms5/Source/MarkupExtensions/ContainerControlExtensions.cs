// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ContainerControlExtensions.cs -- методы расширения для ContainerControl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ContainerControl"/>.
/// </summary>
public static class ContainerControlExtensions
{
    #region Public methods

    /// <summary>
    /// Установка активного контрола.
    /// </summary>
    public static TContainer ActiveControl<TContainer>
        (
            this TContainer container,
            Control control
        )
        where TContainer: ContainerControl
    {
        Sure.NotNull (container);
        Sure.NotNull (control);

        container.ActiveControl = control;

        return container;
    }

    /// <summary>
    /// Установка автоматического изменения масштаба.
    /// </summary>
    public static TContainer AutoScaleDimensions<TContainer>
        (
            this TContainer container,
            SizeF size
        )
        where TContainer: ContainerControl
    {
        Sure.NotNull (container);

        container.AutoScaleDimensions = size;

        return container;
    }

    /// <summary>
    /// Установка автоматического изменения масштаба.
    /// </summary>
    public static TContainer AutoScaleMode<TContainer>
        (
            this TContainer container,
            AutoScaleMode mode
        )
        where TContainer: ContainerControl
    {
        Sure.NotNull (container);
        Sure.Defined (mode);

        container.AutoScaleMode = mode;

        return container;
    }

    /// <summary>
    /// Установка автоматической валидации.
    /// </summary>
    public static TContainer AutoValidate<TContainer>
        (
            this TContainer container,
            AutoValidate validate
        )
        where TContainer: ContainerControl
    {
        Sure.NotNull (container);
        Sure.Defined (validate);

        container.AutoValidate = validate;

        return container;
    }

    #endregion
}
