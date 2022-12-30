// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Specifies a symbol that will be displayed when a <see cref="CheckBoxObject"/> is in the checked state.
    /// </summary>
    public enum CheckedSymbol
    {
        /// <summary>
        /// Specifies a check symbol.
        /// </summary>
        Check,

        /// <summary>
        /// Specifies a diagonal cross symbol.
        /// </summary>
        Cross,

        /// <summary>
        /// Specifies a plus symbol.
        /// </summary>
        Plus,

        /// <summary>
        /// Specifies a filled rectangle.
        /// </summary>
        Fill
    }

    /// <summary>
    /// Specifies a symbol that will be displayed when a <see cref="CheckBoxObject"/> is in the unchecked state.
    /// </summary>
    public enum UncheckedSymbol
    {
        /// <summary>
        /// Specifies no symbol.
        /// </summary>
        None,

        /// <summary>
        /// Specifies a diagonal cross symbol.
        /// </summary>
        Cross,

        /// <summary>
        /// Specifies a minus symbol.
        /// </summary>
        Minus,

        /// <summary>
        /// Specifies a slash symbol.
        /// </summary>
        Slash,

        /// <summary>
        /// Specifies a back slash symbol.
        /// </summary>
        BackSlash
    }

    /// <summary>
    /// Represents a check box object.
    /// </summary>
    public partial class CheckBoxObject : ReportComponentBase
    {
        #region Fields

        private float checkWidthRatio;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or set a value indicating whether the check box is in the checked state.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Data")]
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets a symbol that will be displayed when the check box is in the checked state.
        /// </summary>
        [DefaultValue (CheckedSymbol.Check)]
        [Category ("Appearance")]
        public CheckedSymbol CheckedSymbol { get; set; }

        /// <summary>
        /// Gets or sets a symbol that will be displayed when the check box is in the unchecked state.
        /// </summary>
        [DefaultValue (UncheckedSymbol.None)]
        [Category ("Appearance")]
        public UncheckedSymbol UncheckedSymbol { get; set; }

        /// <summary>
        /// Gets or sets a color of the check symbol.
        /// </summary>
        [Category ("Appearance")]
        [Editor ("AM.Reporting.TypeEditors.ColorEditor, AM.Reporting", typeof (UITypeEditor))]
        public Color CheckColor { get; set; }

        /// <summary>
        /// Gets or sets a data column name bound to this control.
        /// </summary>
        /// <remarks>
        /// Value must be in the form "[Datasource.Column]".
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.DataColumnEditor, AM.Reporting", typeof (UITypeEditor))]
        public string DataColumn { get; set; }

        /// <summary>
        /// Gets or sets an expression that determines whether to show a check.
        /// </summary>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the check symbol width ratio.
        /// </summary>
        /// <remarks>
        /// Valid values are from 0.2 to 2.
        /// </remarks>
        [DefaultValue (1f)]
        [Category ("Appearance")]
        public float CheckWidthRatio
        {
            get => checkWidthRatio;
            set
            {
                if (value <= 0.2f)
                {
                    value = 0.2f;
                }

                if (value > 2)
                {
                    value = 2;
                }

                checkWidthRatio = value;
            }
        }

        /// <summary>
        /// Gets or sets a value determines whether to hide the checkbox if it is in the unchecked state.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool HideIfUnchecked { get; set; }

        /// <summary>
        /// Gets or sets editable for pdf export
        /// </summary>
        [Category ("Behavior")]
        [DefaultValue (false)]
        public bool Editable { get; set; }

        #endregion

        #region Private Methods

        private bool ShouldSerializeCheckColor()
        {
            return CheckColor != Color.Black;
        }

        private void DrawCheck (FRPaintEventArgs e)
        {
            var drawRect = new RectangleF (AbsLeft * e.ScaleX, AbsTop * e.ScaleY,
                Width * e.ScaleX, Height * e.ScaleY);

            var ratio = Width / (Units.Millimeters * 5);
            drawRect.Inflate (-4 * ratio * e.ScaleX, -4 * ratio * e.ScaleY);
            var pen = e.Cache.GetPen (CheckColor, 1.6f * ratio * CheckWidthRatio * e.ScaleX, DashStyle.Solid);
            var g = e.Graphics;
            var saveSmoothing = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (Checked)
            {
                switch (CheckedSymbol)
                {
                    case CheckedSymbol.Check:
                        g.DrawLines (pen, new PointF[]
                        {
                            new PointF (drawRect.Left, drawRect.Top + drawRect.Height / 10 * 5),
                            new PointF (drawRect.Left + drawRect.Width / 10 * 4,
                                drawRect.Bottom - drawRect.Height / 10),
                            new PointF (drawRect.Right, drawRect.Top + drawRect.Height / 10)
                        });
                        break;

                    case CheckedSymbol.Cross:
                        g.DrawLine (pen, drawRect.Left, drawRect.Top, drawRect.Right, drawRect.Bottom);
                        g.DrawLine (pen, drawRect.Left, drawRect.Bottom, drawRect.Right, drawRect.Top);
                        break;

                    case CheckedSymbol.Plus:
                        g.DrawLine (pen, drawRect.Left, drawRect.Top + drawRect.Height / 2, drawRect.Right,
                            drawRect.Top + drawRect.Height / 2);
                        g.DrawLine (pen, drawRect.Left + drawRect.Width / 2, drawRect.Top,
                            drawRect.Left + drawRect.Width / 2, drawRect.Bottom);
                        break;

                    case CheckedSymbol.Fill:
                        Brush brush = e.Cache.GetBrush (CheckColor);
                        g.FillRectangle (brush, drawRect);
                        break;
                }
            }
            else
            {
                switch (UncheckedSymbol)
                {
                    case UncheckedSymbol.Cross:
                        g.DrawLine (pen, drawRect.Left, drawRect.Top, drawRect.Right, drawRect.Bottom);
                        g.DrawLine (pen, drawRect.Left, drawRect.Bottom, drawRect.Right, drawRect.Top);
                        break;

                    case UncheckedSymbol.Minus:
                        g.DrawLine (pen, drawRect.Left, drawRect.Top + drawRect.Height / 2, drawRect.Right,
                            drawRect.Top + drawRect.Height / 2);
                        break;

                    case UncheckedSymbol.Slash:
                        g.DrawLine (pen, drawRect.Left, drawRect.Bottom, drawRect.Right, drawRect.Top);
                        break;

                    case UncheckedSymbol.BackSlash:
                        g.DrawLine (pen, drawRect.Left, drawRect.Top, drawRect.Right, drawRect.Bottom);
                        break;
                }
            }

            g.SmoothingMode = saveSmoothing;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as CheckBoxObject;
            Checked = src.Checked;
            CheckedSymbol = src.CheckedSymbol;
            UncheckedSymbol = src.UncheckedSymbol;
            CheckColor = src.CheckColor;
            DataColumn = src.DataColumn;
            Expression = src.Expression;
            CheckWidthRatio = src.CheckWidthRatio;
            HideIfUnchecked = src.HideIfUnchecked;
            Editable = src.Editable;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            base.Draw (e);
            DrawCheck (e);
            DrawMarkers (e);
            Border.Draw (e, new RectangleF (AbsLeft, AbsTop, Width, Height));
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as CheckBoxObject;
            base.Serialize (writer);

            if (Checked != c.Checked)
            {
                writer.WriteBool ("Checked", Checked);
            }

            if (CheckedSymbol != c.CheckedSymbol)
            {
                writer.WriteValue ("CheckedSymbol", CheckedSymbol);
            }

            if (UncheckedSymbol != c.UncheckedSymbol)
            {
                writer.WriteValue ("UncheckedSymbol", UncheckedSymbol);
            }

            if (CheckColor != c.CheckColor)
            {
                writer.WriteValue ("CheckColor", CheckColor);
            }

            if (DataColumn != c.DataColumn)
            {
                writer.WriteStr ("DataColumn", DataColumn);
            }

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }

            if (CheckWidthRatio != c.CheckWidthRatio)
            {
                writer.WriteFloat ("CheckWidthRatio", CheckWidthRatio);
            }

            if (HideIfUnchecked != c.HideIfUnchecked)
            {
                writer.WriteBool ("HideIfUnchecked", HideIfUnchecked);
            }

            if (Editable)
            {
                writer.WriteBool ("Editable", Editable);
            }
        }

        #endregion

        #region Report Engine

        /// <inheritdoc/>
        public override string[] GetExpressions()
        {
            List<string> expressions = new List<string>();
            expressions.AddRange (base.GetExpressions());

            if (!string.IsNullOrEmpty (DataColumn))
            {
                expressions.Add (DataColumn);
            }

            if (!string.IsNullOrEmpty (Expression))
            {
                expressions.Add (Expression);
            }

            return expressions.ToArray();
        }

        /// <inheritdoc/>
        public override void GetData()
        {
            base.GetData();
            if (!string.IsNullOrEmpty (DataColumn))
            {
                var value = Report.GetColumnValue (DataColumn);
                var varValue = value == null ? new Variant (0) : new Variant (value);
                Checked = varValue == true || (varValue.IsNumeric && varValue != 0);
            }
            else if (!string.IsNullOrEmpty (Expression))
            {
                var value = Report.Calc (Expression);
                Checked = value is bool b && b == true;
            }

            if (!Checked && HideIfUnchecked)
            {
                Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>CheckBoxObject</b> class with default settings.
        /// </summary>
        public CheckBoxObject()
        {
            CheckColor = Color.Black;
            DataColumn = "";
            Expression = "";
            Checked = true;
            CheckedSymbol = CheckedSymbol.Check;
            UncheckedSymbol = UncheckedSymbol.None;
            checkWidthRatio = 1;
            SetFlags (Flags.HasSmartTag, true);
        }
    }
}
