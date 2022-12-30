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
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Barcode
{
    /// <summary>
    /// Generates the Pharmacode barcode.
    /// </summary>
    public class BarcodePharmacode : LinearBarcodeBase
    {
        /// <summary>
        /// Gets or sets the value indicating that quiet zone must be shown.
        /// </summary>
        [DefaultValue (true)]
        public bool QuietZone { get; set; }

        /// <inheritdoc/>
        public override bool IsNumeric => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodePharmacode"/> class with default settings.
        /// </summary>
        public BarcodePharmacode()
        {
            QuietZone = true;
        }

        internal override string GetPattern()
        {
            var value = ulong.Parse (text);
            value += 1;
            var binary = Convert.ToString ((long)value, 2);

            if (binary.StartsWith ("1"))
            {
                binary = binary.Remove (0, 1);
            }

            const string space = "2";
            var result = new StringBuilder();

            if (QuietZone)
            {
                result.Append (space);
            }

            foreach (var c in binary)
            {
                switch (c)
                {
                    case '0':
                        result.Append ("5");
                        result.Append (space);
                        break;
                    case '1':
                        result.Append ("7");
                        result.Append (space);
                        break;
                }
            }

            if (!QuietZone && result.ToString().EndsWith (space))
            {
                result.Remove (result.Length - space.Length, space.Length);
            }

            return result.ToString();
        }

        /// <inheritdoc/>
        public override void Assign (BarcodeBase source)
        {
            base.Assign (source);
            var src = source as BarcodePharmacode;
            QuietZone = src.QuietZone;
        }

        internal override void Serialize (Utils.FRWriter writer, string prefix, BarcodeBase diff)
        {
            base.Serialize (writer, prefix, diff);
            if (diff is not BarcodePharmacode c || QuietZone != c.QuietZone)
            {
                writer.WriteBool (prefix + "QuietZone", QuietZone);
            }
        }
    }
}
