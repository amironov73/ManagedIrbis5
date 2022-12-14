// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* PenInfo.cs -- информация о пере.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Информация о пере.
/// </summary>
[TypeConverter (typeof (ExpandableObjectConverter))]
[Editor (typeof (Editor), typeof (UITypeEditor))]
public class PenInfo
{
    #region Properties

    private const PenAlignment DefaultAlignment = PenAlignment.Center;

    ///<summary>
    /// Alingnment.
    ///</summary>
    [DefaultValue (DefaultAlignment)]
    public PenAlignment Alignment { get; set; } = DefaultAlignment;

    private const string DefaultColor = "Black";

    ///<summary>
    /// Color.
    ///</summary>
    [DefaultValue (typeof (Color), DefaultColor)]
    public Color Color { get; set; } = Color.FromName (DefaultColor);

    private const DashCap DefaultDashCap = DashCap.Flat;

    ///<summary>
    /// Dash.
    ///</summary>
    [DefaultValue (DefaultDashCap)]
    public DashCap DashCap { get; set; } = DefaultDashCap;

    private const DashStyle DefaultDashStyle = DashStyle.Solid;

    ///<summary>
    /// Dash.
    ///</summary>
    [DefaultValue (DefaultDashStyle)]
    public DashStyle DashStyle { get; set; } = DefaultDashStyle;

    private const LineCap DefaultEndCap = LineCap.Flat;

    ///<summary>
    ///
    ///</summary>
    [DefaultValue (DefaultEndCap)]
    public LineCap EndCap { get; set; } = DefaultEndCap;

    private const LineJoin DefaultLineJoin = LineJoin.Bevel;

    ///<summary>
    /// Line join.
    ///</summary>
    [DefaultValue (DefaultLineJoin)]
    public LineJoin LineJoin { get; set; } = DefaultLineJoin;

    private const LineCap DefaultStartCap = LineCap.Flat;

    ///<summary>
    ///
    ///</summary>
    [DefaultValue (DefaultStartCap)]
    public LineCap StartCap { get; set; } = DefaultStartCap;

    private const float DefaultWidth = 1.0f;

    ///<summary>
    /// Pen width.
    ///</summary>
    [DefaultValue (DefaultWidth)]
    public float Width { get; set; } = DefaultWidth;

    #endregion

    #region Public methods

    /// <summary>
    /// Convert <see cref="PenInfo"/> to <see cref="Pen"/>.
    /// </summary>
    public virtual Pen ToPen()
    {
        Pen result = new Pen (Color, Width)
        {
            Alignment = Alignment,
            DashStyle = DashStyle,
            EndCap = EndCap,
            LineJoin = LineJoin,
            StartCap = StartCap,
            DashCap = DashCap
        };

        return result;
    }

    #endregion

    #region Editor

    /// <summary>
    /// Editor for <see cref="PenInfo"/>.
    /// </summary>
    public class Editor
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
            var penInfo = (PenInfo)value!;

            if (provider is null)
            {
                return penInfo;
            }

            var edSvc = (IWindowsFormsEditorService?)provider.GetService
                (
                    typeof (IWindowsFormsEditorService)
                );
            if (edSvc != null)
            {
                var form = new PenInfoControl
                    (
                        penInfo,
                        edSvc
                    );
                edSvc.DropDownControl (form);

                if (form.Result != null)
                {
                    return form.Result;
                }
            }

            return penInfo;
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
