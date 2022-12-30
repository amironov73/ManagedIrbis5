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
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Barcode.Aztec
{
    /// <summary>
    /// <p>Represents a polynomial whose coefficients are elements of a GF.
    /// Instances of this class are immutable.</p>
    /// <p>Much credit is due to William Rucklidge since portions of this code are an indirect
    /// port of his C++ Reed-Solomon implementation.</p>
    /// </summary>
    /// <author>Sean Owen</author>
    internal sealed class GenericGFPoly
    {
        private readonly GenericGF field;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericGFPoly"/> class.
        /// </summary>
        /// <param name="field">the {@link GenericGF} instance representing the field to use
        /// to perform computations</param>
        /// <param name="coefficients">coefficients as ints representing elements of GF(size), arranged
        /// from most significant (highest-power term) coefficient to least significant</param>
        /// <exception cref="ArgumentException">if argument is null or empty,
        /// or if leading coefficient is 0 and this is not a
        /// constant polynomial (that is, it is not the monomial "0")</exception>
        internal GenericGFPoly (GenericGF field, int[] coefficients)
        {
            if (coefficients.Length == 0)
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
                    this.Coefficients = new int[] { 0 };
                }
                else
                {
                    this.Coefficients = new int[coefficientsLength - firstNonZero];
                    Array.Copy (coefficients,
                        firstNonZero,
                        this.Coefficients,
                        0,
                        this.Coefficients.Length);
                }
            }
            else
            {
                this.Coefficients = coefficients;
            }
        }

        internal int[] Coefficients { get; }

        /// <summary>
        /// degree of this polynomial
        /// </summary>
        internal int Degree => Coefficients.Length - 1;

        /// <summary>
        /// Gets a value indicating whether this <see cref="GenericGFPoly"/> is zero.
        /// </summary>
        /// <value>true iff this polynomial is the monomial "0"</value>
        internal bool isZero => Coefficients[0] == 0;

        /// <summary>
        /// coefficient of x^degree term in this polynomial
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>coefficient of x^degree term in this polynomial</returns>
        internal int getCoefficient (int degree)
        {
            return Coefficients[Coefficients.Length - 1 - degree];
        }

        /// <summary>
        /// evaluation of this polynomial at a given point
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns>evaluation of this polynomial at a given point</returns>
        internal int evaluateAt (int a)
        {
            var result = 0;
            if (a == 0)
            {
                // Just return the x^0 coefficient
                return getCoefficient (0);
            }

            var size = Coefficients.Length;
            if (a == 1)
            {
                // Just the sum of the coefficients
                foreach (var coefficient in Coefficients)
                {
                    result = GenericGF.addOrSubtract (result, coefficient);
                }

                return result;
            }

            result = Coefficients[0];
            for (var i = 1; i < size; i++)
            {
                result = GenericGF.addOrSubtract (field.multiply (a, result), Coefficients[i]);
            }

            return result;
        }

        internal GenericGFPoly addOrSubtract (GenericGFPoly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GenericGFPolys do not have same GenericGF field");
            }

            if (isZero)
            {
                return other;
            }

            if (other.isZero)
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
                sumDiff[i] = GenericGF.addOrSubtract (smallerCoefficients[i - lengthDiff], largerCoefficients[i]);
            }

            return new GenericGFPoly (field, sumDiff);
        }

        internal GenericGFPoly multiply (GenericGFPoly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GenericGFPolys do not have same GenericGF field");
            }

            if (isZero || other.isZero)
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
                    product[i + j] = GenericGF.addOrSubtract (product[i + j],
                        field.multiply (aCoeff, bCoefficients[j]));
                }
            }

            return new GenericGFPoly (field, product);
        }

        internal GenericGFPoly multiply (int scalar)
        {
            if (scalar == 0)
            {
                return field.Zero;
            }

            if (scalar == 1)
            {
                return this;
            }

            var size = Coefficients.Length;
            var product = new int[size];
            for (var i = 0; i < size; i++)
            {
                product[i] = field.multiply (Coefficients[i], scalar);
            }

            return new GenericGFPoly (field, product);
        }

        internal GenericGFPoly multiplyByMonomial (int degree, int coefficient)
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

            return new GenericGFPoly (field, product);
        }

        internal GenericGFPoly[] divide (GenericGFPoly other)
        {
            if (!field.Equals (other.field))
            {
                throw new ArgumentException ("GenericGFPolys do not have same GenericGF field");
            }

            if (other.isZero)
            {
                throw new ArgumentException ("Divide by 0");
            }

            var quotient = field.Zero;
            var remainder = this;

            var denominatorLeadingTerm = other.getCoefficient (other.Degree);
            var inverseDenominatorLeadingTerm = field.inverse (denominatorLeadingTerm);

            while (remainder.Degree >= other.Degree && !remainder.isZero)
            {
                var degreeDifference = remainder.Degree - other.Degree;
                var scale = field.multiply (remainder.getCoefficient (remainder.Degree), inverseDenominatorLeadingTerm);
                var term = other.multiplyByMonomial (degreeDifference, scale);
                var iterationQuotient = field.buildMonomial (degreeDifference, scale);
                quotient = quotient.addOrSubtract (iterationQuotient);
                remainder = remainder.addOrSubtract (term);
            }

            return new GenericGFPoly[] { quotient, remainder };
        }

        public override string ToString()
        {
            var result = new StringBuilder (8 * Degree);
            for (var degree = Degree; degree >= 0; degree--)
            {
                var coefficient = getCoefficient (degree);
                if (coefficient != 0)
                {
                    if (coefficient < 0)
                    {
                        result.Append (" - ");
                        coefficient = -coefficient;
                    }
                    else
                    {
                        if (result.Length > 0)
                        {
                            result.Append (" + ");
                        }
                    }

                    if (degree == 0 || coefficient != 1)
                    {
                        var alphaPower = field.log (coefficient);
                        if (alphaPower == 0)
                        {
                            result.Append ('1');
                        }
                        else if (alphaPower == 1)
                        {
                            result.Append ('a');
                        }
                        else
                        {
                            result.Append ("a^");
                            result.Append (alphaPower);
                        }
                    }

                    if (degree != 0)
                    {
                        if (degree == 1)
                        {
                            result.Append ('x');
                        }
                        else
                        {
                            result.Append ("x^");
                            result.Append (degree);
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
