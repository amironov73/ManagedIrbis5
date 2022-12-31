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
using System.ComponentModel;
using System.Collections.Generic;

using AM.Reporting.Utils;

using System.Drawing;
using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting.Gauge
{
    /// <summary>
    /// Represents a gauge object.
    /// </summary>
    public partial class GaugeObject : ReportComponentBase, ICloneable
    {
        #region Fields

        private double maximum;
        private double minimum;
        private double value;
        private GaugeLabel label;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the minimal value of gauge.
        /// </summary>
        [Category ("Layout")]
        public double Minimum
        {
            get => minimum;
            set
            {
                if (value < maximum)
                {
                    minimum = value;
                    if (this.value < minimum)
                    {
                        this.value = minimum;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximal value of gauge.
        /// </summary>
        [Category ("Layout")]
        public double Maximum
        {
            get => maximum;
            set
            {
                if (value > minimum)
                {
                    maximum = value;
                    if (this.value > maximum)
                    {
                        this.value = maximum;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current value of gauge.
        /// </summary>
        [Category ("Layout")]
        public double Value
        {
            get => value;
            set
            {
                if ((value >= minimum) && (value <= maximum))
                {
                    this.value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets scale of gauge.
        /// </summary>
        [Category ("Appearance")]
        [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
        [Editor ("AM.Reporting.TypeEditors.ScaleEditor, AM.Reporting", typeof (UITypeEditor))]
        public GaugeScale Scale { get; set; }

        /// <summary>
        /// Gets or sets pointer of gauge.
        /// </summary>
        [Category ("Appearance")]
        [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
        [Editor ("AM.Reporting.TypeEditors.PointerEditor, AM.Reporting", typeof (UITypeEditor))]
        public GaugePointer Pointer { get; set; }

        /// <summary>
        /// Gets or sets gauge label.
        /// </summary>
        [Category ("Appearance")]
        [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
        [Editor ("AM.Reporting.TypeEditors.LabelEditor, AM.Reporting", typeof (UITypeEditor))]
        public virtual GaugeLabel Label
        {
            get => label;
            set => label = value;
        }

        /// <summary>
        /// Gets or sets an expression that determines the value of gauge object.
        /// </summary>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Expression { get; set; }

        /// <summary>
        /// Gets a value that specifies is gauge vertical or not.
        /// </summary>
        [Browsable (false)]
        public bool Vertical => Width < Height;

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeObject"/> class.
        /// </summary>
        public GaugeObject()
        {
            minimum = 0;
            maximum = 100;
            value = 10;
            Scale = new GaugeScale (this);
            Pointer = new GaugePointer (this);
            label = new GaugeLabel (this);
            Expression = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeObject"/> class.
        /// </summary>
        /// <param name="minimum">Minimum value of gauge.</param>
        /// <param name="maximum">Maximum value of gauge.</param>
        /// <param name="value">Current value of gauge.</param>
        public GaugeObject (double minimum, double maximum, double value)
        {
            this.minimum = minimum;
            this.maximum = maximum;
            this.value = value;
            Scale = new GaugeScale (this);
            Pointer = new GaugePointer (this);
            label = new GaugeLabel (this);
            Expression = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeObject"/> class.
        /// </summary>
        /// <param name="minimum">Minimum value of gauge.</param>
        /// <param name="maximum">Maximum value of gauge.</param>
        /// <param name="value">Current value of gauge.</param>
        /// <param name="scale">Scale of gauge.</param>
        /// <param name="pointer">Pointer of gauge.</param>
        public GaugeObject (double minimum, double maximum, double value, GaugeScale scale, GaugePointer pointer)
        {
            this.minimum = minimum;
            this.maximum = maximum;
            this.value = value;
            this.Scale = scale;
            this.Pointer = pointer;
            label = new GaugeLabel (this);
            Expression = "";
        }

        #endregion // Constructors

        #region Report Engine

        /// <inheritdoc/>
        public override string[] GetExpressions()
        {
            List<string> expressions = new List<string>();
            expressions.AddRange (base.GetExpressions());

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

            if (!string.IsNullOrEmpty (Expression))
            {
                var val = Report.Calc (Expression);
                if (val != null)
                {
                    try
                    {
                        Value = Converter.StringToFloat (val.ToString());
                    }
                    catch
                    {
                        Value = 0.0;
                    }
                }
            }
        }

        #endregion // Report Engine

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as GaugeObject;
            Maximum = src.Maximum;
            Minimum = src.Minimum;
            Value = src.Value;
            Expression = src.Expression;
            Scale.Assign (src.Scale);
            Pointer.Assign (src.Pointer);
            Label.Assign (src.Label);
        }

        /// <summary>
        /// Draws the gauge.
        /// </summary>
        /// <param name="eventArgs">Draw event arguments.</param>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            base.Draw (eventArgs);
            Scale.Draw (eventArgs);
            Pointer.Draw (eventArgs);
            Border.Draw (eventArgs, new RectangleF (AbsLeft, AbsTop, Width, Height));
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as GaugeObject;
            base.Serialize (writer);

            if (Maximum != c.Maximum)
            {
                writer.WriteDouble ("Maximum", Maximum);
            }

            if (Minimum != c.Minimum)
            {
                writer.WriteDouble ("Minimum", Minimum);
            }

            if (Value != c.Value)
            {
                writer.WriteDouble ("Value", Value);
            }

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }

            if (Scale != c.Scale)
            {
                Scale.Serialize (writer, "Scale", c.Scale);
            }

            if (Pointer != c.Pointer)
            {
                Pointer.Serialize (writer, "Pointer", c.Pointer);
            }

            if (Label != c.Label)
            {
                Label.Serialize (writer, "Label", c.Label);
            }
        }

        /// <summary>
        /// Clone Gauge Object
        /// </summary>
        /// <returns> clone of this object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion // Public Methods
    }
}
