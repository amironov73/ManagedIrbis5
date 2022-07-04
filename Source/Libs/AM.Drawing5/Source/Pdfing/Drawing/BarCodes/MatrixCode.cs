// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MatrixCode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes
{
    /// <summary>
    /// Represents the base class of all 2D codes.
    /// </summary>
    public abstract class MatrixCode : CodeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCode"/> class.
        /// </summary>
        public MatrixCode(string text, string encoding, int rows, int columns, XSize size)
            : base(text, size, CodeDirection.LeftToRight)
        {
            _encoding = encoding;
            if (String.IsNullOrEmpty(_encoding))
                _encoding = new String('a', Text.Length);

            if (columns < rows)
            {
                _rows = columns;
                _columns = rows;
            }
            else
            {
                _columns = columns;
                _rows = rows;
            }

            Text = text;
        }

        /// <summary>
        /// Gets or sets the encoding. docDaSt
        /// </summary>
        public string Encoding
        {
            get { return _encoding; }
            set
            {
                _encoding = value;
                _matrixImage = null;
            }
        }
        string _encoding;

        /// <summary>
        /// docDaSt
        /// </summary>
        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                _matrixImage = null;
            }
        }
        int _columns;

        /// <summary>
        /// docDaSt
        /// </summary>
        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                _matrixImage = null;
            }
        }
        int _rows;

        /// <summary>
        /// docDaSt
        /// </summary>
        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _matrixImage = null;
            }
        }

        internal XImage MatrixImage
        {
            get { return _matrixImage; }
            set { _matrixImage = value; }
        }
        XImage _matrixImage;

        /// <summary>
        /// When implemented in a derived class renders the 2D code.
        /// </summary>
        protected internal abstract void Render(XGraphics gfx, XBrush brush, XPoint center);

        /// <summary>
        /// Determines whether the specified string can be used as Text for this matrix code type.
        /// </summary>
        protected override void CheckCode(string text)
        { }
    }
}
