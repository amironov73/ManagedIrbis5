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
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a line object.
    /// </summary>
    /// <remarks>
    /// Use the <b>Border.Width</b>, <b>Border.Style</b> and <b>Border.Color</b> properties to set
    /// the line width, style and color. Set the <see cref="Diagonal"/> property to <b>true</b>
    /// if you want to show a diagonal line.
    /// </remarks>
    public partial class LineObject : ReportComponentBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating that the line is diagonal.
        /// </summary>
        /// <remarks>
        /// If this property is <b>false</b>, the line can be only horizontal or vertical.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Appearance")]
        public bool Diagonal { get; set; }

        /// <summary>
        /// Gets or sets the start cap settings.
        /// </summary>
        [Category ("Appearance")]
        public CapSettings StartCap { get; set; }

        /// <summary>
        /// Gets or sets the end cap settings.
        /// </summary>
        [Category ("Appearance")]
        public CapSettings EndCap { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as LineObject;
            Diagonal = src.Diagonal;
            StartCap.Assign (src.StartCap);
            EndCap.Assign (src.EndCap);
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            var g = eventArgs.Graphics;

            // draw marker when inserting a line
            if (Width == 0 && Height == 0)
            {
                g.DrawLine (Pens.Black, AbsLeft * eventArgs.ScaleX - 6, AbsTop * eventArgs.ScaleY, AbsLeft * eventArgs.ScaleX + 6,
                    AbsTop * eventArgs.ScaleY);
                g.DrawLine (Pens.Black, AbsLeft * eventArgs.ScaleX, AbsTop * eventArgs.ScaleY - 6, AbsLeft * eventArgs.ScaleX,
                    AbsTop * eventArgs.ScaleY + 6);
                return;
            }

            var report = Report;
            if (report is { SmoothGraphics: true })
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            var pen = eventArgs.Cache.GetPen (Border.Color, Border.Width * eventArgs.ScaleX, Border.DashStyle);

            var width = Width;
            var height = Height;
            if (!Diagonal)
            {
                if (Math.Abs (width) > Math.Abs (height))
                {
                    height = 0;
                }
                else
                {
                    width = 0;
                }
            }

            var x1 = AbsLeft * eventArgs.ScaleX;
            var y1 = AbsTop * eventArgs.ScaleY;
            var x2 = (AbsLeft + width) * eventArgs.ScaleX;
            var y2 = (AbsTop + height) * eventArgs.ScaleY;

            if (StartCap.Style == CapStyle.None && EndCap.Style == CapStyle.None)
            {
                g.DrawLine (pen, x1, y1, x2, y2);
            }
            else
            {
                // draw line caps manually. It is necessary for correct svg rendering
                var angle = (float)(Math.Atan2 (x2 - x1, y2 - y1) / Math.PI * 180);
                var len = (float)Math.Sqrt ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                var scale = Border.Width * eventArgs.ScaleX;

                var state = g.Save();
                g.TranslateTransform (x1, y1);
                g.RotateTransform (-angle);
                float y = 0;
                GraphicsPath startCapPath = null;
                GraphicsPath endCapPath = null;
                float inset = 0;
                if (StartCap.Style != CapStyle.None)
                {
                    StartCap.GetCustomCapPath (out startCapPath, out inset);
                    y += inset * scale;
                }

                if (EndCap.Style != CapStyle.None)
                {
                    EndCap.GetCustomCapPath (out endCapPath, out inset);
                    len -= inset * scale;
                }

                g.DrawLine (pen, 0, y, 0, len);
                g.Restore (state);

                pen = eventArgs.Cache.GetPen (Border.Color, 1, Border.DashStyle);
                if (StartCap.Style != CapStyle.None)
                {
                    state = g.Save();
                    g.TranslateTransform (x1, y1);
                    g.RotateTransform (180 - angle);
                    g.ScaleTransform (scale, scale);
                    g.DrawPath (pen, startCapPath);
                    g.Restore (state);
                }

                if (EndCap.Style != CapStyle.None)
                {
                    state = g.Save();
                    g.TranslateTransform (x2, y2);
                    g.RotateTransform (-angle);
                    g.ScaleTransform (scale, scale);
                    g.DrawPath (pen, endCapPath);
                    g.Restore (state);
                }
            }

            if (report is { SmoothGraphics: true } && Diagonal)
            {
                g.InterpolationMode = InterpolationMode.Default;
                g.SmoothingMode = SmoothingMode.Default;
            }
        }

        /// <inheritdoc/>
        public override List<ValidationError> Validate()
        {
            var listError = new List<ValidationError>();

            if (IsIntersectingWithOtherObject && !(Parent is ReportComponentBase &&
                                                   !Validator.RectContainInOtherRect (
                                                       (Parent as ReportComponentBase).AbsBounds, AbsBounds)))
            {
                listError.Add (new ValidationError (Name, ValidationError.ErrorLevel.Warning,
                    Res.Get ("Messages,Validator,IntersectedObjects"), this));
            }

            if ((Height < 0 || Width < 0) && Diagonal || (Height <= 0 && Width <= 0))
            {
                listError.Add (new ValidationError (Name, ValidationError.ErrorLevel.Error,
                    Res.Get ("Messages,Validator,IncorrectSize"), this));
            }

            if (Name == "")
            {
                listError.Add (new ValidationError (Name, ValidationError.ErrorLevel.Error,
                    Res.Get ("Messages,Validator,UnnamedObject"), this));
            }

            if (Parent is ReportComponentBase &&
                !Validator.RectContainInOtherRect ((Parent as ReportComponentBase).AbsBounds, AbsBounds))
            {
                listError.Add (new ValidationError (Name, ValidationError.ErrorLevel.Error,
                    Res.Get ("Messages,Validator,OutOfBounds"), this));
            }

            return listError;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            Border.SimpleBorder = true;
            base.Serialize (writer);
            var c = writer.DiffObject as LineObject;

            if (Diagonal != c.Diagonal)
            {
                writer.WriteBool ("Diagonal", Diagonal);
            }

            StartCap.Serialize ("StartCap", writer, c.StartCap);
            EndCap.Serialize ("EndCap", writer, c.EndCap);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LineObject"/> class with default settings.
        /// </summary>
        public LineObject()
        {
            StartCap = new CapSettings();
            EndCap = new CapSettings();
            FlagSimpleBorder = true;
            FlagUseFill = false;
        }
    }
}
