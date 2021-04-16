// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DrawingContext.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    /// Контекст рисования
    /// </summary>
    public class DrawingContext
    {
        #region Properties

        //public PrinterInfo Printer { get; set; }

        public CardInfo Card { get; set; }

        // public HumanInfo Human { get; set; }

        public Graphics Graphics { get; set; }

        #endregion

        #region Private members

        private string _Evaluator(Match match)
        {
            string name = match.Groups["name"].Value;

            switch (name)
            {
                case "Date":
                    return DateTime.Today.ToShortDateString();

                case "Time":
                    return DateTime.Now.ToShortTimeString();

                case "DateTime":
                    return DateTime.Now.ToString(CultureInfo.CurrentCulture);

                case "Year":
                    return DateTime.Today.Year
                        .ToString(CultureInfo.InvariantCulture);
            }

            /*
            PropertyInfo property = typeof(HumanInfo).GetProperty(name);
            object value = property.GetValue
            (
                Human,
                null
            );

            return ReferenceEquals(value, null)
                ? string.Empty
                : value.ToString();

                */

            return string.Empty;
        }

        #endregion

        #region Public methods

        public string ExpandText(string input)
        {
            Regex regex = new Regex(@"{(?<name>\w+)}");
            string result = regex.Replace
            (
                input,
                _Evaluator
            );
            return result;
        }

        #endregion
    }
}
