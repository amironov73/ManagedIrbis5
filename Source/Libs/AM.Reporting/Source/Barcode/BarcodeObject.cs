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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;
using AM.Reporting.Code;

using System.Windows.Forms;
using System.Drawing.Design;

using AM.Reporting.Barcode.QRCode;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// Represents a barcode object.
    /// Represents a barcode object.
    /// </summary>
    /// <remarks>
    /// The instance of this class represents a barcode. Here are some common
    /// actions that can be performed with this object:
    /// <list type="bullet">
    ///   <item>
    ///     <description>To select the type of barcode, use the <see cref="Barcode"/> property.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>To specify a static barcode data, use the <see cref="Text"/> property.
    ///       You also may use the <see cref="DataColumn"/> or <see cref="Expression"/> properties
    ///       to specify dynamic value for a barcode.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>To set a barcode orientation, use the <see cref="Angle"/> property.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>To specify the size of barcode, set the <see cref="AutoSize"/> property
    ///       to <b>true</b> and use the <see cref="Zoom"/> property to zoom the barcode.
    ///       If <see cref="AutoSize"/> property is set to <b>false</b>, you need to specify the
    ///       size using the <see cref="ComponentBase.Width">Width</see> and
    ///       <see cref="ComponentBase.Height">Height</see> properties.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <example>This example shows how to configure the BarcodeObject to display PDF417 barcode.
    /// <code>
    /// BarcodeObject barcode;
    /// ...
    /// barcode.Barcode = new BarcodePDF417();
    /// (barcode.Barcode as BarcodePDF417).CompactionMode = CompactionMode.Text;
    /// </code>
    /// </example>
    public partial class BarcodeObject : ReportComponentBase
    {
        /// <summary>
        /// Specifies the horizontal alignment of a Barcode object. Works only when autosize is on.
        /// </summary>
        public enum Alignment
        {
            /// <summary>
            /// Specifies that the barcode is aligned to the left of the original layout.
            /// </summary>
            Left,

            /// <summary>
            /// Specifies that the barcode is aligned to the center of the original layout.
            /// </summary>
            Center,

            /// <summary>
            /// Specifies that the barcode is aligned to the right of the original layout.
            /// </summary>
            Right
        }


        #region Fields

        private BarcodeBase barcode;
        private string savedText;
        private RectangleF origRect;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the barcode type.
        /// </summary>
        [Category ("Appearance")]
        public BarcodeBase Barcode
        {
            get => barcode;
            set
            {
                if (value == null)
                {
                    value = new Barcode39();
                }

                barcode = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of a Barcode object.
        /// </summary>
        [DefaultValue (Alignment.Left)]
        [Category ("Appearance")]
        public Alignment HorzAlign { get; set; }

        /// <summary>
        /// Gets or sets the symbology name.
        /// </summary>
        /// <remarks>
        /// The following symbology names are supported:
        /// <list type="bullet">
        ///   <item><description>"2/5 Interleaved"</description></item>
        ///   <item><description>"2/5 Industrial"</description></item>
        ///   <item><description>"2/5 Matrix"</description></item>
        ///   <item><description>"Codabar"</description></item>
        ///   <item><description>"Code128"</description></item>
        ///   <item><description>"Code39"</description></item>
        ///   <item><description>"Code39 Extended"</description></item>
        ///   <item><description>"Code93"</description></item>
        ///   <item><description>"Code93 Extended"</description></item>
        ///   <item><description>"EAN8"</description></item>
        ///   <item><description>"EAN13"</description></item>
        ///   <item><description>"MSI"</description></item>
        ///   <item><description>"PostNet"</description></item>
        ///   <item><description>"UPC-A"</description></item>
        ///   <item><description>"UPC-E0"</description></item>
        ///   <item><description>"UPC-E1"</description></item>
        ///   <item><description>"Supplement 2"</description></item>
        ///   <item><description>"Supplement 5"</description></item>
        ///   <item><description>"PDF417"</description></item>
        ///   <item><description>"Datamatrix"</description></item>
        ///   <item><description>"QRCode"</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// barcode.SymbologyName = "PDF417";
        /// (barcode.Barcode as BarcodePDF417).CompactionMode = CompactionMode.Text;
        /// </code>
        /// </example>
        [Browsable (false)]
        public string SymbologyName
        {
            get => Barcode.Name;
            set
            {
                if (SymbologyName != value)
                {
                    var bartype = Barcodes.GetType (value);
                    Barcode = Activator.CreateInstance (bartype) as BarcodeBase;
                    Text = barcode.GetDefaultValue();
                }
            }
        }

        /// <summary>
        /// Gets or sets the angle of barcode, in degrees.
        /// </summary>
        [DefaultValue (0)]
        [Category ("Appearance")]
        public int Angle { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the barcode should handle its width automatically.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool AutoSize { get; set; }

        /// <summary>
        /// Gets or sets a data column name bound to this control.
        /// </summary>
        /// <remarks>
        /// Value must be in the form "Datasource.Column".
        /// </remarks>
        [Category ("Data")]
        public string DataColumn { get; set; }

        /// <summary>
        /// Gets or sets an expression that contains the barcode data.
        /// </summary>
        [Category ("Data")]
        public string Expression { get; set; }

        /// <summary>
        /// Enable or disable of using an expression in Text
        /// </summary>
        [Category ("Data")]
        [DefaultValue (false)]
        public bool AllowExpressions { get; set; }

        /// <summary>
        /// Gets or sets brackets for using in expressions
        /// </summary>
        [Category ("Data")]
        public string Brackets { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the barcode should display a human-readable text.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool ShowText { get; set; }

        /// <summary>
        /// Gets or sets the barcode data.
        /// </summary>
        [Category ("Data")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets padding within the BarcodeObject.
        /// </summary>
        [Category ("Layout")]
        public Padding Padding { get; set; }

        /// <summary>
        /// Gets or sets a zoom of the barcode.
        /// </summary>
        [DefaultValue (1f)]
        [Category ("Appearance")]
        public float Zoom { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether it is necessary to hide the object if the
        /// barcode data is empty.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool HideIfNoData { get; set; }

        /// <summary>
        /// Gets or sets the text that will be displayed if the barcode data is empty.
        /// </summary>
        [Category ("Data")]
        public string NoDataText { get; set; }

        /// <summary>
        /// Gets or sets values for forced use of a bitmap image instead of a vector
        /// </summary>
        [Category ("Appearance")]
        [DefaultValue (false)]
        public bool AsBitmap { get; set; }

        #endregion

        #region Private Methods

        private void SetBarcodeProperties()
        {
            barcode.Initialize (Text, ShowText, Angle, Zoom);
        }

        private void DrawBarcode (FRPaintEventArgs e)
        {
            var displayRect = new RectangleF (
                (AbsLeft + Padding.Left) * e.ScaleX,
                (AbsTop + Padding.Top) * e.ScaleY,
                (Width - Padding.Horizontal) * e.ScaleX,
                (Height - Padding.Vertical) * e.ScaleY);

            var g = e.Graphics;
            var state = g.Save();
            try
            {
                var report = Report;
                if (report != null)
                {
                    if (report.SmoothGraphics)
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }

                    g.TextRenderingHint = report.GetTextQuality();
                }

                barcode.DrawBarcode (g, displayRect);
            }
            finally
            {
                g.Restore (state);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize current BarcodeObject as Swiss QR.
        /// </summary>
        /// <param name="parameters">Parameters of swiss qr.</param>
        public void CreateSwissQR (QRSwissParameters parameters)
        {
            var swiss = new QRSwiss (parameters);
            Barcode = new BarcodeQR();
            Barcode.text = swiss.Pack();
            Text = swiss.Pack();
            ShowText = false;
        }

        public void UpdateAutoSize()
        {
            SetBarcodeProperties();
            var size = Barcode.CalcBounds();
            size.Width *= Zoom;
            size.Height *= Zoom;
            if (AutoSize)
            {
                if (Angle is 0 or 180)
                {
                    Width = size.Width + Padding.Horizontal;
                    if (size.Height > 0)
                    {
                        Height = size.Height + Padding.Vertical;
                    }
                }
                else if (Angle is 90 or 270)
                {
                    Height = size.Width + Padding.Vertical;
                    if (size.Height > 0)
                    {
                        Width = size.Height + Padding.Horizontal;
                    }
                }

                RelocateAlign();
            }
        }

        /// <summary>
        /// Relocate BarcodeObject based on alignment
        /// </summary>
        public void RelocateAlign()
        {
            if (HorzAlign == Alignment.Left || origRect == RectangleF.Empty)
            {
                return;
            }

            switch (HorzAlign)
            {
                case Alignment.Center:
                {
                    Left = origRect.Left + (origRect.Width / 2) - Width / 2;
                    break;
                }
                case Alignment.Right:
                {
                    Left = origRect.Right - Width;
                    break;
                }
            }

            origRect = RectangleF.Empty;
        }

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as BarcodeObject;
            Barcode = src.Barcode.Clone();
            Angle = src.Angle;
            AutoSize = src.AutoSize;
            DataColumn = src.DataColumn;
            Expression = src.Expression;
            Text = src.Text;
            ShowText = src.ShowText;
            Padding = src.Padding;
            Zoom = src.Zoom;
            HideIfNoData = src.HideIfNoData;
            NoDataText = src.NoDataText;
            Brackets = src.Brackets;
            AllowExpressions = src.AllowExpressions;
            AsBitmap = src.AsBitmap;
            HorzAlign = src.HorzAlign;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            var error = false;
            var errorText = "";

            if (string.IsNullOrEmpty (Text))
            {
                error = true;
                errorText = NoDataText;
            }
            else
            {
                try
                {
                    UpdateAutoSize();
                }
                catch (Exception ex)
                {
                    error = true;
                    errorText = ex.Message;
                }
            }

            base.Draw (eventArgs);
            if (!error)
            {
                DrawBarcode (eventArgs);
            }
            else
            {
                if (IsDesigning && Text.StartsWith (Brackets.Split (',')[0]) && Text.EndsWith (Brackets.Split (',')[1]))
                {
                    try
                    {
                        barcode.text = barcode.GetDefaultValue();
                        DrawBarcode (eventArgs);
                        error = false;
                    }
                    catch
                    {
                        error = true;
                    }
                }

                if (error)
                {
                    eventArgs.Graphics.DrawString (errorText, DrawUtils.DefaultReportFont, Brushes.Red,
                        new RectangleF (AbsLeft * eventArgs.ScaleX, AbsTop * eventArgs.ScaleY, Width * eventArgs.ScaleX, Height * eventArgs.ScaleY));
                }
            }

            DrawMarkers (eventArgs);
            Border.Draw (eventArgs, new RectangleF (AbsLeft, AbsTop, Width, Height));
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as BarcodeObject;
            base.Serialize (writer);

            if (Angle != c.Angle)
            {
                writer.WriteInt ("Angle", Angle);
            }

            if (AutoSize != c.AutoSize)
            {
                writer.WriteBool ("AutoSize", AutoSize);
            }

            if (DataColumn != c.DataColumn)
            {
                writer.WriteStr ("DataColumn", DataColumn);
            }

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }

            if (Text != c.Text)
            {
                writer.WriteStr ("Text", Text);
            }

            if (ShowText != c.ShowText)
            {
                writer.WriteBool ("ShowText", ShowText);
            }

            if (Padding != c.Padding)
            {
                writer.WriteValue ("Padding", Padding);
            }

            if (Zoom != c.Zoom)
            {
                writer.WriteFloat ("Zoom", Zoom);
            }

            if (HideIfNoData != c.HideIfNoData)
            {
                writer.WriteBool ("HideIfNoData", HideIfNoData);
            }

            if (NoDataText != c.NoDataText)
            {
                writer.WriteStr ("NoDataText", NoDataText);
            }

            if (AllowExpressions != c.AllowExpressions)
            {
                writer.WriteBool ("AllowExpressions", AllowExpressions);
            }

            if (Brackets != c.Brackets)
            {
                writer.WriteStr ("Brackets", Brackets);
            }

            if (AsBitmap != c.AsBitmap)
            {
                writer.WriteBool ("AsBitmap", AsBitmap);
            }

            if (HorzAlign != c.HorzAlign)
            {
                writer.WriteValue ("HorzAlign", HorzAlign);
            }

            Barcode.Serialize (writer, "Barcode.", c.Barcode);
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
            else
            {
                if (AllowExpressions && !string.IsNullOrEmpty (Brackets))
                {
                    string[] brackets = Brackets.Split (',');

                    // collect expressions found in the text
                    expressions.AddRange (CodeUtils.GetExpressions (Text, brackets[0], brackets[1]));
                }
            }

            return expressions.ToArray();
        }

        /// <inheritdoc/>
        public override void SaveState()
        {
            base.SaveState();
            savedText = Text;
        }

        /// <inheritdoc/>
        public override void RestoreState()
        {
            base.RestoreState();
            Text = savedText;
        }

        /// <inheritdoc/>
        public override void GetData()
        {
            base.GetData();
            if (!string.IsNullOrEmpty (DataColumn))
            {
                var value = Report.GetColumnValue (DataColumn);
                Text = value == null ? "" : value.ToString();
            }
            else if (!string.IsNullOrEmpty (Expression))
            {
                var value = Report.Calc (Expression);
                Text = value == null ? "" : value.ToString();
            }
            else
            {
                // process expressions
                if (AllowExpressions && !string.IsNullOrEmpty (Brackets))
                {
                    string[] bracket_arr = Brackets.Split (',');
                    var args = new FindTextArgs
                    {
                        Text = new FastString (Text),
                        OpenBracket = bracket_arr[0],
                        CloseBracket = bracket_arr[1]
                    };
                    var expressionIndex = 0;
                    while (args.StartIndex < args.Text.Length)
                    {
                        var expression = CodeUtils.GetExpression (args, false);
                        if (expression == null)
                        {
                            break;
                        }

                        var value = Report.Calc (expression).ToString();
                        args.Text = args.Text.Remove (args.StartIndex, args.EndIndex - args.StartIndex);
                        args.Text = args.Text.Insert (args.StartIndex, value);
                        args.StartIndex += value.Length;
                        expressionIndex++;
                    }

                    Text = args.Text.ToString();
                }
            }

            if (Visible)
            {
                Visible = !string.IsNullOrEmpty (Text) || !HideIfNoData;
            }

            if (Visible)
            {
                try
                {
                    origRect = Bounds;
                    UpdateAutoSize();
                }
                catch
                {
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeObject"/> class with default settings.
        /// </summary>
        public BarcodeObject()
        {
            Barcode = new Barcode39();
            AutoSize = true;
            DataColumn = "";
            Expression = "";
            Text = Barcode.GetDefaultValue();
            ShowText = true;
            Padding = new Padding();
            Zoom = 1;
            HideIfNoData = true;
            NoDataText = "";
            AllowExpressions = false;
            Brackets = "[,]";
            SetFlags (Flags.HasSmartTag, true);
        }
    }


    internal static class Barcodes
    {
#if READONLY_STRUCTS
        internal readonly struct BarcodeItem
#else
        internal struct BarcodeItem
#endif
        {
            public readonly Type objType;
            public readonly string barcodeName;

            public BarcodeItem (Type objType, string barcodeName)
            {
                this.objType = objType;
                this.barcodeName = barcodeName;
            }
        }

        public readonly static BarcodeItem[] Items =
        {
            new BarcodeItem (typeof (Barcode2of5Interleaved), "2/5 Interleaved"),
            new BarcodeItem (typeof (Barcode2of5Industrial), "2/5 Industrial"),
            new BarcodeItem (typeof (Barcode2of5Matrix), "2/5 Matrix"),
            new BarcodeItem (typeof (BarcodeDeutscheIdentcode), "Deutsche Identcode"),
            new BarcodeItem (typeof (BarcodeDeutscheLeitcode), "Deutshe Leitcode"),
            new BarcodeItem (typeof (BarcodeITF14), "ITF-14"),
            new BarcodeItem (typeof (BarcodeCodabar), "Codabar"),
            new BarcodeItem (typeof (Barcode128), "Code128"),
            new BarcodeItem (typeof (Barcode39), "Code39"),
            new BarcodeItem (typeof (Barcode39Extended), "Code39 Extended"),
            new BarcodeItem (typeof (Barcode93), "Code93"),
            new BarcodeItem (typeof (Barcode93Extended), "Code93 Extended"),
            new BarcodeItem (typeof (BarcodeEAN8), "EAN8"),
            new BarcodeItem (typeof (BarcodeEAN13), "EAN13"),
            new BarcodeItem (typeof (BarcodeMSI), "MSI"),
            new BarcodeItem (typeof (BarcodePostNet), "PostNet"),
            new BarcodeItem (typeof (BarcodeJapanPost4StateCode), "Japan Post 4 State Code"),
            new BarcodeItem (typeof (BarcodeUPC_A), "UPC-A"),
            new BarcodeItem (typeof (BarcodeUPC_E0), "UPC-E0"),
            new BarcodeItem (typeof (BarcodeUPC_E1), "UPC-E1"),
            new BarcodeItem (typeof (BarcodeSupplement2), "Supplement 2"),
            new BarcodeItem (typeof (BarcodeSupplement5), "Supplement 5"),
            new BarcodeItem (typeof (BarcodePDF417), "PDF417"),
            new BarcodeItem (typeof (BarcodeDatamatrix), "Datamatrix"),
            new BarcodeItem (typeof (BarcodeQR), "QR Code"),
            new BarcodeItem (typeof (BarcodeAztec), "Aztec"),
            new BarcodeItem (typeof (BarcodePlessey), "Plessey"),
            new BarcodeItem (typeof (BarcodeEAN128), "GS1-128 (UCC/EAN-128)"),
            new BarcodeItem (typeof (BarcodeGS1Omnidirectional), "GS1 DataBar Omnidirectional"),
            new BarcodeItem (typeof (BarcodeGS1Limited), "GS1 DataBar Limited"),
            new BarcodeItem (typeof (BarcodeGS1Stacked), "GS1 DataBar Stacked"),
            new BarcodeItem (typeof (BarcodeGS1StackedOmnidirectional), "GS1 DataBar Stacked Omnidirectional"),
            new BarcodeItem (typeof (BarcodePharmacode), "Pharmacode"),
            new BarcodeItem (typeof (BarcodeIntelligentMail), "Intelligent Mail (USPS)"),
            new BarcodeItem (typeof (BarcodeMaxiCode), "MaxiCode")
        };

        public static string GetName (Type type)
        {
            foreach (var item in Items)
            {
                if (item.objType == type)
                {
                    return item.barcodeName;
                }
            }

            return "";
        }

        public static Type? GetType (string name)
        {
            foreach (var item in Items)
            {
                if (item.barcodeName == name)
                {
                    return item.objType;
                }
            }

            return null;
        }

        public static string[] GetDisplayNames()
        {
            List<string> result = new List<string>();
            var res = new MyRes ("ComponentMenu,Barcode,Barcodes");
            for (var i = 0; i < Items.Length; i++)
            {
                result.Add (res.Get ("Barcode" + i.ToString()));
            }

            return result.ToArray();
        }

        public static string[] GetSymbologyNames()
        {
            List<string> result = new List<string>();
            foreach (var item in Items)
            {
                result.Add (item.barcodeName);
            }

            return result.ToArray();
        }
    }
}
