// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    public class XBaseGradientBrush : XBrush
    {
        protected XBaseGradientBrush(XColor color1, XColor color2)
        {
            _color1 = color1;
            _color2 = color2;

        }

        /// <summary>
        /// Gets or sets an XMatrix that defines a local geometric transform for this LinearGradientBrush.
        /// </summary>
        public XMatrix Transform
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        /// <summary>
        /// Translates the brush with the specified offset.
        /// </summary>
        public void TranslateTransform(double dx, double dy)
        {
            _matrix.TranslatePrepend(dx, dy);
        }

        /// <summary>
        /// Translates the brush with the specified offset.
        /// </summary>
        public void TranslateTransform(double dx, double dy, XMatrixOrder order)
        {
            _matrix.Translate(dx, dy, order);
        }

        /// <summary>
        /// Scales the brush with the specified scalars.
        /// </summary>
        public void ScaleTransform(double sx, double sy)
        {
            _matrix.ScalePrepend(sx, sy);
        }

        /// <summary>
        /// Scales the brush with the specified scalars.
        /// </summary>
        public void ScaleTransform(double sx, double sy, XMatrixOrder order)
        {
            _matrix.Scale(sx, sy, order);
        }

        /// <summary>
        /// Rotates the brush with the specified angle.
        /// </summary>
        public void RotateTransform(double angle)
        {
            _matrix.RotatePrepend(angle);
        }

        /// <summary>
        /// Rotates the brush with the specified angle.
        /// </summary>
        public void RotateTransform(double angle, XMatrixOrder order)
        {
            _matrix.Rotate(angle, order);
        }

        /// <summary>
        /// Multiply the brush transformation matrix with the specified matrix.
        /// </summary>
        public void MultiplyTransform(XMatrix matrix)
        {
            _matrix.Prepend(matrix);
        }

        /// <summary>
        /// Multiply the brush transformation matrix with the specified matrix.
        /// </summary>
        public void MultiplyTransform(XMatrix matrix, XMatrixOrder order)
        {
            _matrix.Multiply(matrix, order);
        }

        /// <summary>
        /// Resets the brush transformation matrix with identity matrix.
        /// </summary>
        public void ResetTransform()
        {
            _matrix = new XMatrix();
        }

        //public void SetBlendTriangularShape(double focus);
        //public void SetBlendTriangularShape(double focus, double scale);
        //public void SetSigmaBellShape(double focus);
        //public void SetSigmaBellShape(double focus, double scale);



        //public Blend Blend { get; set; }
        //public bool GammaCorrection { get; set; }
        //public ColorBlend InterpolationColors { get; set; }
        //public XColor[] LinearColors { get; set; }
        //public RectangleF Rectangle { get; }
        //public WrapMode WrapMode { get; set; }
        //private bool interpolationColorsWasSet;

        internal XColor _color1, _color2;
        internal XMatrix _matrix;

    }
}
