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

#endregion

#nullable enable

namespace AM.Reporting.Barcode.QRCode
{
    /*/// <summary> <p>Implements Reed-Solomon enbcoding, as the name implies.</p>
    ///
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>  William Rucklidge
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal sealed class ReedSolomonEncoder
    {
        //UPGRADE_NOTE: Final was removed from the declaration of 'field '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private GF256 field;

        //UPGRADE_NOTE: Final was removed from the declaration of 'cachedGenerators '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private System.Collections.ArrayList cachedGenerators;

        public ReedSolomonEncoder (GF256 field)
        {
            if (!GF256.QR_CODE_FIELD.Equals (field))
            {
                throw new ArgumentException ("Only QR Code is supported at this time");
            }

            this.field = field;
            cachedGenerators = System.Collections.ArrayList.Synchronized (new System.Collections.ArrayList (10));
            cachedGenerators.Add (new GF256Poly (field, new int[] { 1 }));
        }

        private GF256Poly buildGenerator (int degree)
        {
            if (degree >= cachedGenerators.Count)
            {
                var lastGenerator = (GF256Poly)cachedGenerators[cachedGenerators.Count - 1];
                for (var d = cachedGenerators.Count; d <= degree; d++)
                {
                    var nextGenerator =
                        lastGenerator.multiply (new GF256Poly (field, new int[] { 1, field.exp (d - 1) }));
                    cachedGenerators.Add (nextGenerator);
                    lastGenerator = nextGenerator;
                }
            }

            return (GF256Poly)cachedGenerators[degree];
        }

        public void encode (int[] toEncode, int ecBytes)
        {
            if (ecBytes == 0)
            {
                throw new ArgumentException ("No error correction bytes");
            }

            var dataBytes = toEncode.Length - ecBytes;
            if (dataBytes <= 0)
            {
                throw new ArgumentException ("No data bytes provided");
            }

            var generator = buildGenerator (ecBytes);
            var infoCoefficients = new int[dataBytes];
            Array.Copy (toEncode, 0, infoCoefficients, 0, dataBytes);
            var info = new GF256Poly (field, infoCoefficients);
            info = info.multiplyByMonomial (ecBytes, 1);
            var remainder = info.divide (generator)[1];
            var coefficients = remainder.Coefficients;
            var numZeroCoefficients = ecBytes - coefficients.Length;
            for (var i = 0; i < numZeroCoefficients; i++)
            {
                toEncode[dataBytes + i] = 0;
            }

            Array.Copy (coefficients, 0, toEncode, dataBytes + numZeroCoefficients, coefficients.Length);
        }
    }
}
