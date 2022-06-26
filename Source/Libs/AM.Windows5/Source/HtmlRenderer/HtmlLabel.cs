// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlLabel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core;
using AM.Windows.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer;

/// <summary>
/// Provides HTML rendering using the text property.<br/>
/// WPF control that will render html content in it's client rectangle.<br/>
/// Using <see cref="AutoSize"/> and <see cref="AutoSizeHeightOnly"/> client can control how the html content effects the
/// size of the label. Either case scrollbars are never shown and html content outside of client bounds will be clipped.
/// MaxWidth/MaxHeight and MinWidth/MinHeight with AutoSize can limit the max/min size of the control<br/>
/// The control will handle mouse and keyboard events on it to support html text selection, copy-paste and mouse clicks.<br/>
/// </summary>
/// <remarks>
/// See <see cref="HtmlControl"/> for more info.
/// </remarks>
public class HtmlLabel 
    : HtmlControl
{
    #region Dependency properties

    public static readonly DependencyProperty AutoSizeProperty = DependencyProperty.Register("AutoSize", typeof(bool), typeof(HtmlLabel), new PropertyMetadata(true, OnDependencyProperty_valueChanged));
    public static readonly DependencyProperty AutoSizeHeightOnlyProperty = DependencyProperty.Register("AutoSizeHeightOnly", typeof(bool), typeof(HtmlLabel), new PropertyMetadata(false, OnDependencyProperty_valueChanged));

    #endregion


    /// <summary>
    /// Init.
    /// </summary>
    static HtmlLabel()
    {
        BackgroundProperty.OverrideMetadata(typeof(HtmlLabel), new FrameworkPropertyMetadata(Brushes.Transparent));
    }

    /// <summary>
    /// Automatically sets the size of the label by content size
    /// </summary>
    [Category("Layout")]
    [Description("Automatically sets the size of the label by content size.")]
    public bool AutoSize
    {
        get { return (bool)GetValue(AutoSizeProperty); }
        set { SetValue(AutoSizeProperty, value); }
    }

    /// <summary>
    /// Automatically sets the height of the label by content height (width is not effected).
    /// </summary>
    [Category("Layout")]
    [Description("Automatically sets the height of the label by content height (width is not effected)")]
    public virtual bool AutoSizeHeightOnly
    {
        get { return (bool)GetValue(AutoSizeHeightOnlyProperty); }
        set { SetValue(AutoSizeHeightOnlyProperty, value); }
    }


    #region Private methods

    /// <summary>
    /// Perform the layout of the html in the control.
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        if (_htmlContainer != null)
        {
            using (var ig = new GraphicsAdapter())
            {
                var horizontal = Padding.Left + Padding.Right + BorderThickness.Left + BorderThickness.Right;
                var vertical = Padding.Top + Padding.Bottom + BorderThickness.Top + BorderThickness.Bottom;

                var size = new RSize(constraint.Width < Double.PositiveInfinity ? constraint.Width - horizontal : 0, constraint.Height < Double.PositiveInfinity ? constraint.Height - vertical : 0);
                var minSize = new RSize(MinWidth < Double.PositiveInfinity ? MinWidth - horizontal : 0, MinHeight < Double.PositiveInfinity ? MinHeight - vertical : 0);
                var maxSize = new RSize(MaxWidth < Double.PositiveInfinity ? MaxWidth - horizontal : 0, MaxHeight < Double.PositiveInfinity ? MaxHeight - vertical : 0);

                var newSize = HtmlRendererUtils.Layout(ig, _htmlContainer.HtmlContainerInt, size, minSize, maxSize, AutoSize, AutoSizeHeightOnly);

                constraint = new Size(newSize.Width + horizontal, newSize.Height + vertical);
            }
        }

        if (double.IsPositiveInfinity(constraint.Width) || double.IsPositiveInfinity(constraint.Height))
            constraint = Size.Empty;

        return constraint;
    }

    /// <summary>
    /// Handle when dependency property value changes to update the underline HtmlContainer with the new value.
    /// </summary>
    private static void OnDependencyProperty_valueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var control = dependencyObject as HtmlLabel;
        if (control != null)
        {
            if (e.Property == AutoSizeProperty)
            {
                if ((bool)e.NewValue)
                {
                    dependencyObject.SetValue(AutoSizeHeightOnlyProperty, false);
                    control.InvalidateMeasure();
                    control.InvalidateVisual();
                }
            }
            else if (e.Property == AutoSizeHeightOnlyProperty)
            {
                if ((bool)e.NewValue)
                {
                    dependencyObject.SetValue(AutoSizeProperty, false);
                    control.InvalidateMeasure();
                    control.InvalidateVisual();
                }
            }
        }
    }

    #endregion
}
