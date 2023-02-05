// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* BorderInfo.cs -- информация о границе контрола
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Информация о границе контрола или его части.
/// </summary>
[TypeConverter (typeof (ExpandableObjectConverter))]
[Editor (typeof (Editor), typeof (UITypeEditor))]
public class BorderInfo
{
    #region Properties

    private const bool DefaultBorder = true;

    ///<summary>
    /// Рисовать границу или нет.
    ///</summary>
    [DefaultValue (DefaultBorder)]
    [Description ("Draw border or not")]
    public bool DrawBorder { get; set; } = DefaultBorder;

    private const bool Default3D = true;

    ///<summary>
    /// Граница трехмерная или нет.
    ///</summary>
    [DefaultValue (Default3D)]
    [Description ("Whether border is 3D?")]
    public bool Draw3D { get; set; } = Default3D;

    private const ButtonBorderStyle DefaultStyle2D = ButtonBorderStyle.Solid;

    ///<summary>
    /// Стиль двухмерной границы.
    ///</summary>
    [DefaultValue (DefaultStyle2D)]
    [Description ("Style of 2D border")]
    public ButtonBorderStyle Style2D { get; set; } = DefaultStyle2D;

    private const string DefaultColor = "Black";

    ///<summary>
    /// Цвет двухмерной границы.
    ///</summary>
    [DefaultValue (typeof (Color), DefaultColor)]
    [Description ("Color for 2D border.")]
    public Color BorderColor { get; set; } = Color.FromName (DefaultColor);

    private const Border3DStyle DefaultStyle3D =
        Border3DStyle.Etched;

    ///<summary>
    /// Стиль трехмерной границы.
    ///</summary>
    [DefaultValue (DefaultStyle3D)]
    [Description ("Style for 3D border")]
    public Border3DStyle Style3D { get; set; } = DefaultStyle3D;

    #endregion

    #region Public methods

    /// <summary>
    /// Отрисовка границы контрола.
    /// </summary>
    public void Draw
        (
            Graphics graphics,
            Rectangle rectangle
        )
    {
        if (DrawBorder)
        {
            if (Draw3D)
            {
                ControlPaint.DrawBorder3D (graphics, rectangle, Style3D);
            }
            else
            {
                ControlPaint.DrawBorder (graphics, rectangle, BorderColor, Style2D);
            }
        }
    }

    #endregion

    #region Designer

    /// <summary>
    /// Editor for <see cref="BorderInfo"/>.
    /// </summary>
    public sealed class Editor 
        : UITypeEditor
    {
        /// <inheritdoc />
        public override UITypeEditorEditStyle GetEditStyle
            (
                ITypeDescriptorContext? context
            )
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <inheritdoc />
        public override object EditValue
            (
                ITypeDescriptorContext? context,
                IServiceProvider? provider,
                object? value
            )
        {
            Sure.NotNull (value);

            var borderInfo = (BorderInfo) value.ThrowIfNull();

            if (provider is null)
            {
                return borderInfo;
            }

            var editorService = (IWindowsFormsEditorService?)
                provider.GetService (typeof (IWindowsFormsEditorService));
            if (editorService is not null)
            {
                var form = new BorderInfoControl
                    (
                        borderInfo,
                        editorService
                    );
                editorService.DropDownControl (form);

                if (form.Result is not null)
                {
                    return form.Result;
                }
            }

            return borderInfo;
        }

        /// <inheritdoc />
        public override void PaintValue
            (
                PaintValueEventArgs eventArgs
            )
        {
            var graphics = eventArgs.Graphics;
            using var font = new Font ("Arial", 8);
            using Brush brush = new SolidBrush (Color.Black);
            graphics.DrawString ("Not implemented", font, brush, 0, 0);
        }

        /// <inheritdoc />
        public override bool GetPaintValueSupported
            (
                ITypeDescriptorContext? context
            )
        {
            return false;
        }
    }

    #endregion
}
