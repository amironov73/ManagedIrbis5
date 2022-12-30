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

namespace AM.Reporting.Import.RDL
{
    /// <summary>
    /// The RDL Size units.
    /// </summary>
    public static class SizeUnits
    {
        #region Constants

        /// <summary>
        /// Specifies the units measured in millimeters.
        /// </summary>
        public const string Millimeter = "mm";

        /// <summary>
        /// Specifies the units measured in centimeters.
        /// </summary>
        public const string Centimeter = "cm";

        /// <summary>
        /// Specifies the units measured in inches.
        /// </summary>
        public const string Inch = "in";

        /// <summary>
        /// Specifies the units measured in points.
        /// </summary>
        public const string Point = "pt";

        /// <summary>
        /// Specifies the units measured in picas.
        /// </summary>
        public const string Pica = "pc";

        #endregion // Constants
    }

    /// <summary>
    /// Defines the constants used to convert between RDL Size and pixels.
    /// </summary>
    /// <remarks>
    /// To convert pixels to inches, use the code:
    /// <code>inches = pixels / SizeUnitsP.Inch;</code>
    /// To convert inches to pixels, use the code:
    /// <code>pixels = inches * SizeUnitsP.Inch;</code>
    /// </remarks>
    public static class SizeUnitsP
    {
        #region Fields

        /// <summary>
        /// The number of pixels in one millimeter.
        /// </summary>
        public static float Millimeter = AM.Reporting.Utils.Units.Millimeters;

        /// <summary>
        /// The number of pixels in one centimeter.
        /// </summary>
        public static float Centimeter = AM.Reporting.Utils.Units.Centimeters;

        /// <summary>
        /// The number of pixels in one inch.
        /// </summary>
        public static float Inch = AM.Reporting.Utils.Units.Inches;

        /// <summary>
        /// The number of pixels in one point.
        /// </summary>
        public static float Point = AM.Reporting.Utils.Units.Millimeters * SizeUnitsM.Point;

        /// <summary>
        /// The number of pixels in one pica.
        /// </summary>
        public static float Pica = AM.Reporting.Utils.Units.Millimeters * SizeUnitsM.Pica;

        #endregion // Fields
    }

    /// <summary>
    /// Defines the constants used to convert between RDL Size and millimeters.
    /// </summary>
    /// <remarks>
    /// To convert millimeters to inches, use the code:
    /// <code>inches = millimeters / SizeUnitsM.Inch;</code>
    /// To convert inches to millimeters, use the code:
    /// <code>millimeters = inches * SizeUnitsM.Inch;</code>
    /// </remarks>
    public static class SizeUnitsM
    {
        #region Constants

        /// <summary>
        /// The number of millimeters in one centimeter.
        /// </summary>
        public const float Centimeter = 10;

        /// <summary>
        /// The number of millimeters in one inch.
        /// </summary>
        public const float Inch = 25.4f;

        /// <summary>
        /// The number of millimeters in one point.
        /// </summary>
        public const float Point = 0.3528f;

        /// <summary>
        /// The number of millimeters in one pica.
        /// </summary>
        public const float Pica = 4.2336f;

        #endregion // Constants
    }
}
