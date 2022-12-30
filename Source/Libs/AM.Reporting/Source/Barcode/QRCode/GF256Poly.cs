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
    /*/// <summary> <p>Represents a polynomial whose coefficients are elements of GF(256).
    /// Instances of this class are immutable.</p>
    ///
    /// <p>Much credit is due to William Rucklidge since portions of this code are an indirect
    /// port of his C++ Reed-Solomon implementation.</p>
    ///
    /// </summary>
    /// <author>  Sean Owen
    /// </author>
    /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
    /// </author>*/
    internal sealed class GF256Poly
    {
        internal int[] Coefficients { get; }

        /*/// <returns> degree of this polynomial
        /// </returns>*/
        internal int Degree => Coefficients.Length - 1;

        /*/// <returns> true iff this polynomial is the monomial "0"
        /// </returns>*/
        internal bool Zero => Coefficients[0] == 0;

        //UPGRADE_NOTE: Final was removed from the declaration of 'field '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private GF256 field;

        //UPGRADE_NOTE: Final was removed from the declaration of 'coefficients '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"

        /*/// <param name="field">the {@link GF256} instance representing the field to use
        /// to perform computations
        /// </param>
        /// <param name="coefficients">coefficients as ints representing elements of GF(256), arranged
        /// from most significant (highest-power term) coefficient to least significant
        /// </param>
        /// <throws>  IllegalArgumentException if argument is null or empty, </throws>
        /// <summary> or if leading coefficient is 0 and this is not a
        /// constant polynomial (that is, it is not the monomial "0")
        /// </summary>*/
        internal GF256Poly (GF256 field, int[] coefficients)
        {
            if (coefficients == null || coefficients.Length == 0)
            {
                throw new ArgumentException();
            }

            this.field = field;
            var coefficientsLength = coefficients.Length;
            if (coefficientsLength > 1 && coefficients[0] == 0)
            {
                // Leading term must be non-zero for anything except the constant polynomial "0"
                var firstNonZero = 1;
                while (firstNonZero < coefficientsLength && coefficients[firstNonZero] == 0)
                {
                    firstNonZero++;
                }

                if (firstNonZero == coefficientsLength)
                {
                    this.Coefficients = field.Zero.Coefficients;
                }
                else
                {
                    this.Coefficients = new int[coefficientsLength - firstNonZero];
                    Array.Copy (coefficients, firstNonZero, this.Coefficients, 0, this.Coefficients.Length);
                }
            }
            else
            {
                this.Coefficients = coefficients;
            }
        }

        /*/// <returns> coefficient of x^degree term in this polynomial
        /// </returns>*/
        internal int getCoefficient (int degree)
        {
            return Coefficients[Coefficients.Length - 1 - degree];
        }

        internal GF256Poly addOrSubtract (GF256Poly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GF256Polys do not have same GF256 field");
            }

            if (Zero)
            {
                return other;
            }

            if (other.Zero)
            {
                return this;
            }

            var smallerCoefficients = Coefficients;
            var largerCoefficients = other.Coefficients;
            if (smallerCoefficients.Length > largerCoefficients.Length)
            {
                var temp = smallerCoefficients;
                smallerCoefficients = largerCoefficients;
                largerCoefficients = temp;
            }

            var sumDiff = new int[largerCoefficients.Length];
            var lengthDiff = largerCoefficients.Length - smallerCoefficients.Length;

            // Copy high-order terms only found in higher-degree polynomial's coefficients
            Array.Copy (largerCoefficients, 0, sumDiff, 0, lengthDiff);

            for (var i = lengthDiff; i < largerCoefficients.Length; i++)
            {
                sumDiff[i] = GF256.addOrSubtract (smallerCoefficients[i - lengthDiff], largerCoefficients[i]);
            }

            return new GF256Poly (field, sumDiff);
        }

        internal GF256Poly multiply (GF256Poly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GF256Polys do not have same GF256 field");
            }

            if (Zero || other.Zero)
            {
                return field.Zero;
            }

            var aCoefficients = Coefficients;
            var aLength = aCoefficients.Length;
            var bCoefficients = other.Coefficients;
            var bLength = bCoefficients.Length;
            var product = new int[aLength + bLength - 1];
            for (var i = 0; i < aLength; i++)
            {
                var aCoeff = aCoefficients[i];
                for (var j = 0; j < bLength; j++)
                {
                    product[i + j] = GF256.addOrSubtract (product[i + j], field.multiply (aCoeff, bCoefficients[j]));
                }
            }

            return new GF256Poly (field, product);
        }

        internal GF256Poly multiplyByMonomial (int degree, int coefficient)
        {
            if (degree < 0)
            {
                throw new ArgumentException();
            }

            if (coefficient == 0)
            {
                return field.Zero;
            }

            var size = Coefficients.Length;
            var product = new int[size + degree];
            for (var i = 0; i < size; i++)
            {
                product[i] = field.multiply (Coefficients[i], coefficient);
            }

            return new GF256Poly (field, product);
        }

        internal GF256Poly[] divide (GF256Poly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GF256Polys do not have same GF256 field");
            }

            if (other.Zero)
            {
                throw new ArgumentException ("Divide by 0");
            }

            var quotient = field.Zero;
            var remainder = this;

            var denominatorLeadingTerm = other.getCoefficient (other.Degree);
            var inverseDenominatorLeadingTerm = field.inverse (denominatorLeadingTerm);

            while (remainder.Degree >= other.Degree && !remainder.Zero)
            {
                var degreeDifference = remainder.Degree - other.Degree;
                var scale = field.multiply (remainder.getCoefficient (remainder.Degree), inverseDenominatorLeadingTerm);
                var term = other.multiplyByMonomial (degreeDifference, scale);
                var iterationQuotient = field.buildMonomial (degreeDifference, scale);
                quotient = quotient.addOrSubtract (iterationQuotient);
                remainder = remainder.addOrSubtract (term);
            }

            return new GF256Poly[] { quotient, remainder };
        }
    }
}
