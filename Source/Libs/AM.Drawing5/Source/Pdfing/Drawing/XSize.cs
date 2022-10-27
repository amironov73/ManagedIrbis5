// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XSize.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Represents a pair of floating-point numbers, typically the width and height of a
    /// graphical object.
    /// </summary>
    [DebuggerDisplay ("{DebuggerDisplay}")]
    [Serializable,
     StructLayout (LayoutKind
         .Sequential)] //, ValueSerializer(typeof(SizeValueSerializer)), TypeConverter(typeof(SizeConverter))]
    public struct XSize : IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the XPoint class with the specified values.
        /// </summary>
        public XSize (double width, double height)
        {
            if (width < 0 || height < 0)
            {
                throw
                    new ArgumentException (
                        "WidthAndHeightCannotBeNegative"); //SR.Get(SRID.Size_WidthAndHeightCannotBeNegative, new object[0]));
            }

            _width = width;
            _height = height;
        }

        /// <summary>
        /// Determines whether two size objects are equal.
        /// </summary>
        public static bool operator == (XSize size1, XSize size2)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return size1.Width == size2.Width && size1.Height == size2.Height;

            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        /// Determines whether two size objects are not equal.
        /// </summary>
        public static bool operator != (XSize size1, XSize size2)
        {
            return !(size1 == size2);
        }

        /// <summary>
        /// Indicates whether this two instance are equal.
        /// </summary>
        public static bool Equals (XSize size1, XSize size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }

            return size1.Width.Equals (size2.Width) && size1.Height.Equals (size2.Height);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals (object? o)
        {
            return o is XSize size && Equals (this, size);
        }

        /// <summary>
        /// Indicates whether this instance and a specified size are equal.
        /// </summary>
        public bool Equals (XSize value)
        {
            return Equals (this, value);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }

            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Parses the size from a string.
        /// </summary>
        public static XSize Parse (string source)
        {
            XSize empty;
            var cultureInfo = CultureInfo.InvariantCulture;
            var helper = new TokenizerHelper (source, cultureInfo);
            var str = helper.NextTokenRequired();
            if (str == "Empty")
            {
                empty = Empty;
            }
            else
            {
                empty = new XSize (Convert.ToDouble (str, cultureInfo),
                    Convert.ToDouble (helper.NextTokenRequired(), cultureInfo));
            }

            helper.LastTokenRequired();
            return empty;
        }

        /// <summary>
        /// Converts this XSize to an XPoint.
        /// </summary>
        public XPoint ToXPoint()
        {
            return new XPoint (_width, _height);
        }

        /// <summary>
        /// Converts this XSize to an XVector.
        /// </summary>
        public XVector ToXVector()
        {
            return new XVector (_width, _height);
        }

        /// <summary>
        /// Converts this XSize to a human readable string.
        /// </summary>
        public override string ToString()
        {
            return ConvertToString (null, null);
        }

        /// <summary>
        /// Converts this XSize to a human readable string.
        /// </summary>
        public string ToString
            (
                IFormatProvider? provider
            )
        {
            return ConvertToString (null, provider);
        }

        /// <summary>
        /// Converts this XSize to a human readable string.
        /// </summary>
        string IFormattable.ToString
            (
                string? format,
                IFormatProvider? provider
            )
        {
            return ConvertToString (format, provider);
        }

        internal string ConvertToString
            (
                string? format,
                IFormatProvider? provider
            )
        {
            if (IsEmpty)
            {
                return "Empty";
            }

            provider ??= CultureInfo.InvariantCulture;
            var numericListSeparator = TokenizerHelper.GetNumericListSeparator (provider);

            // ReSharper disable FormatStringProblem
            return string.Format (provider, "{1:" + format + "}{0}{2:" + format + "}",
                new object[] { numericListSeparator, _width, _height });

            // ReSharper restore FormatStringProblem
        }

        /// <summary>
        /// Returns an empty size, i.e. a size with a width or height less than 0.
        /// </summary>
        public static XSize Empty
        {
            get { return s_empty; }
        }

        static readonly XSize s_empty;

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return _width < 0; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return _width; }
            set
            {
                if (IsEmpty)
                {
                    throw
                        new InvalidOperationException (
                            "CannotModifyEmptySize"); //SR.Get(SRID.Size_CannotModifyEmptySize, new object[0]));
                }

                if (value < 0)
                {
                    throw
                        new ArgumentException (
                            "WidthCannotBeNegative"); //SR.Get(SRID.Size_WidthCannotBeNegative, new object[0]));
                }

                _width = value;
            }
        }

        double _width;

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return _height; }
            set
            {
                if (IsEmpty)
                {
                    throw
                        new InvalidOperationException (
                            "CannotModifyEmptySize"); // SR.Get(SRID.Size_CannotModifyEmptySize, new object[0]));
                }

                if (value < 0)
                {
                    throw
                        new ArgumentException (
                            "HeightCannotBeNegative"); //SR.Get(SRID.Size_HeightCannotBeNegative, new object[0]));
                }

                _height = value;
            }
        }

        double _height;

        /// <summary>
        /// Performs an explicit conversion from XSize to XVector.
        /// </summary>
        public static explicit operator XVector (XSize size)
        {
            return new XVector (size._width, size._height);
        }

        /// <summary>
        /// Performs an explicit conversion from XSize to XPoint.
        /// </summary>
        public static explicit operator XPoint (XSize size)
        {
            return new XPoint (size._width, size._height);
        }

        private static XSize CreateEmptySize()
        {
            var size = new XSize
            {
                _width = double.NegativeInfinity,
                _height = double.NegativeInfinity
            };
            return size;
        }

        static XSize()
        {
            s_empty = CreateEmptySize();
        }

        /// <summary>
        /// Gets the DebuggerDisplayAttribute text.
        /// </summary>
        /// <value>The debugger display.</value>

        // ReSharper disable UnusedMember.Local
        string DebuggerDisplay

            // ReSharper restore UnusedMember.Local
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return string.Format (CultureInfo.InvariantCulture,
                    "size=({2}{0:" + format + "}, {1:" + format + "})",
                    _width, _height, IsEmpty ? "Empty " : "");
            }
        }
    }
}
